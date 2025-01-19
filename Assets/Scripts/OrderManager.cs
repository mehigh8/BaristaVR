using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [System.Serializable]
    public class Order
    {
        [Tooltip("(coffee, milk)")]
        public Vector2 coffee;
        public string food;
    }

    public GameObject servingPoint;
    public Vector3 halfBoxSizes;
    public List<GameObject> possibleFoodItems = new List<GameObject>();
    public float chanceForFood;
    public Order currentOrder;
    [HideInInspector] public QueueManager queueManager;

    private static OrderManager instance = null;
    private GameObject cup = null;
    private GameObject food = null;
    private bool hasOrder;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        hasOrder = false;
    }

    public static OrderManager GetInstance() => instance;

    public void GenerateOrder()
    {
        if (hasOrder)
            return;
        hasOrder = true;
        currentOrder = new Order();
        int orderNumber = Random.Range(0, 4);
        currentOrder.coffee = new Vector2(0.33f * orderNumber, 1f - (0.33f * orderNumber));
        if (Random.value < chanceForFood)
            currentOrder.food = possibleFoodItems[Random.Range(0, possibleFoodItems.Count)].name;
        else
            currentOrder.food = "";
    }

    private void Update()
    {
        TryCompleteOrder();
    }

    public void TryCompleteOrder()
    {
        Collider[] items = Physics.OverlapBox(servingPoint.transform.position, halfBoxSizes, Quaternion.identity);

        cup = null;
        food = null;
        foreach (Collider item in items)
        {
            if (PlayerData.LayerMaskContains(PlayerData.GetInstance().cupLayer, item.gameObject.layer))
                cup = item.gameObject;

            if (PlayerData.LayerMaskContains(PlayerData.GetInstance().foodLayer, item.gameObject.layer) && item.name.Contains(currentOrder.food))
                food = item.gameObject;
        }

        if (cup != null)
        {
            if (currentOrder.food.Equals(""))
                CompleteOrder();
            else
            {
                if (food != null) 
                    CompleteOrder();
            }
        }
    }

    private void CompleteOrder()
    {
        // What happens when order is completed
        queueManager.OrderComplete();
        print("Order was compeleted");

        Destroy(cup);
        cup = null;

        if (food != null)
        {
            Destroy(food);
            food = null;
        }

        currentOrder = null;
        hasOrder = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(servingPoint.transform.position, halfBoxSizes * 2);
    }
}
