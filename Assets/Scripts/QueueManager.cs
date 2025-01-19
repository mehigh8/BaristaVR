using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{

    [SerializeField] private GameObject[] clientPrefabs;
    
    [SerializeField] private Transform exitPoint;
    [SerializeField] private Transform[] doors;

    [SerializeField] private int maxClients;
    [SerializeField] private float clientCooldown;
    [SerializeField] private float clientMoveSpeed;
    [SerializeField] private float clientRotateSpeed;
    [SerializeField] private float doorRotateSpeed;

    private List<GameObject> clients;
    private List<Vector3> destinations;

    private void DoorsLogic()
    {
        float targetRot = 0;
        foreach (GameObject client in clients)
        {
            if (Vector3.Distance(client.transform.position, doors[0].position) < 3)
            {
                targetRot = 120f;
                break;
            }
        }

        doors[0].rotation = Quaternion.Lerp(doors[0].rotation, Quaternion.Euler(new Vector3(0, targetRot, 0)), doorRotateSpeed);
        doors[1].rotation = Quaternion.Lerp(doors[1].rotation, Quaternion.Euler(new Vector3(0, -targetRot, 0)), doorRotateSpeed);
    }

    private void CreateClient()
    {
        clients.Add(Instantiate(clientPrefabs[Random.Range(0, clientPrefabs.Length - 1)], exitPoint.position, Quaternion.identity));
        if (destinations.Count > 0)
            destinations.Add(destinations[destinations.Count - 1] + Vector3.forward / 2);
        else
            destinations.Add(transform.position);
    }
    
    public void OrderComplete()
    {
        // complete order
        for (int i = destinations.Count - 1; i > 0; i--)
            destinations[i] = destinations[i - 1];
        destinations[0] = exitPoint.position;
    }
    
    private void MoveClients()
    {
        for (int i = 0; i < clients.Count; i++)
        {
            Vector3 destinationForward = (destinations[i] - clients[i].transform.position).normalized;
            float angle = Vector3.Angle(destinationForward, Vector3.forward);
            clients[i].transform.rotation = Quaternion.Lerp(clients[i].transform.rotation, Quaternion.Euler(new Vector3(0, angle, 0)) , clientRotateSpeed);
            
            if (Vector3.Distance(destinations[i], clients[i].transform.position) < 0.1f)
                clients[i].GetComponent<Animator>().Play("metarig|Idle");
            else
            {
                clients[i].GetComponent<Animator>().Play("metarig|Walk");
                clients[i].transform.position = Vector3.Lerp(clients[i].transform.position, destinations[i], clientMoveSpeed);
            }
            
            if (destinations[i] == transform.position && Vector3.Distance(clients[i].transform.position, destinations[i]) < 0.5f)
            {
                OrderManager.GetInstance().GenerateOrder();
            }
            if (destinations[i] == exitPoint.position &&
                Vector3.Distance(clients[i].transform.position, destinations[i]) < 0.5f)
            {
                Destroy(clients[i]);
                clients.RemoveAt(i);
                destinations.RemoveAt(i);
                i--;
            }
        }
    }

    private IEnumerator ClientSpawning()
    {
        CreateClient();
        yield return new WaitForSeconds(clientCooldown);
        while (clients.Count >= maxClients)
            yield return 0;
        StartCoroutine(ClientSpawning());
    }
    
    void Start()
    {
        clients = new List<GameObject>();
        destinations = new List<Vector3>();
        StartCoroutine(ClientSpawning());
        OrderManager.GetInstance().queueManager = this;
    }

    void Update()
    {
        DoorsLogic();
        MoveClients();
        
        // temp
        if (Input.GetKeyDown(KeyCode.F))
            OrderComplete();
    }
}
