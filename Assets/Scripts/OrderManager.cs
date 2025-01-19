using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private TextMeshProUGUI scoreRef;
    public float chanceForFood;
    public Order currentOrder;
    [HideInInspector] public QueueManager queueManager;

    private static OrderManager instance = null;
    private GameObject cup = null;
    private GameObject food = null;
    private bool hasOrder;
    private int score = 0;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        hasOrder = false;
        scoreObjects = new List<TextMeshProUGUI>();
    }

    public static OrderManager GetInstance() => instance;

    public void GenerateOrder()
    {
        if (hasOrder)
            return;
        hasOrder = true;
        currentOrder = new Order();
        int orderNumber = Random.Range(0, 4);
        currentOrder.coffee = new Vector2(0.33f * orderNumber, 0);
        int milkAmount = Random.Range(0, (int)((1 - currentOrder.coffee.x) / 0.33f) + 1);
        currentOrder.coffee.y = milkAmount * 0.33f;
        if (currentOrder.coffee.magnitude < 0.01f)
            currentOrder.coffee = Random.Range(0, 2) == 1 ? new Vector2(0.33f, 0) : new Vector2(0, 0.33f);
        if (Random.value < chanceForFood)
            currentOrder.food = possibleFoodItems[Random.Range(0, possibleFoodItems.Count)].name;
        else
            currentOrder.food = "";
    }

    [SerializeField] private float fadeSpeed;
    [SerializeField] private float moveSpeed;
    private void MoveScores()
    {
        for (int i = 0; i < scoreObjects.Count; i++)
        {
            scoreObjects[i].color = Color.Lerp(scoreObjects[i].color, new Color(scoreObjects[i].color.r, scoreObjects[i].color.g, scoreObjects[i].color.b, 0), fadeSpeed);
            scoreObjects[i].transform.position += Vector3.up * Time.deltaTime * moveSpeed;
            if (scoreObjects[i].color.a <= 0.01)
            {
                Destroy(scoreObjects[i].transform.parent.gameObject);
                scoreObjects.RemoveAt(i);
                i--;
            }
        }
    }
    
    private void Update()
    {
        MoveScores();
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

        if (cup != null && cup.GetComponent<Cup>().totalAmount > 0.01f)
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
        ScoreOrder();

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

    private List<TextMeshProUGUI> scoreObjects;
    private void ScoreOrder()
    {
        int calculatedScore = 50;
        
        float coffeeDif = Mathf.Abs(currentOrder.coffee.x - cup.GetComponent<Cup>().coffeeAmount) * 50;
        float milkDif = Mathf.Abs(currentOrder.coffee.y - cup.GetComponent<Cup>().milkAmount) * 50;

        calculatedScore -= (int)(Mathf.Round(coffeeDif) + Mathf.Round(milkDif));
        
        score += calculatedScore;
        scoreRef.text = score.ToString();
        TextMeshProUGUI instantiatedObject = Instantiate(scorePrefab, new Vector3(138f, 2f, 40f), Quaternion.identity).GetComponentInChildren<TextMeshProUGUI>();
        if (calculatedScore > 45)
            instantiatedObject.text = "Perfect! +" + calculatedScore;
        else if (calculatedScore > 35)
            instantiatedObject.text = "Great! +" + calculatedScore;
        else if (calculatedScore > 20)
            instantiatedObject.text = "Good! +" + calculatedScore;
        else if (calculatedScore > 0)
            instantiatedObject.text = "Ok! +" + calculatedScore;
        else
            instantiatedObject.text = "Bad! " + calculatedScore;
        scoreObjects.Add(instantiatedObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(servingPoint.transform.position, halfBoxSizes * 2);
    }
}
