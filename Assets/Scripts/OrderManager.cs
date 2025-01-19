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
    public Order currentOrder = null;

    private static OrderManager instance = null;
    private GameObject cup = null;
    private GameObject food = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        currentOrder = new Order();
        currentOrder.coffee = new Vector2(0.33f, 0.66f);
        currentOrder.food = "cookie";
    }

    public static OrderManager GetInstance() => instance;

    public void GenerateOrder()
    {
        if (currentOrder != null) return;

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

            if (PlayerData.LayerMaskContains(PlayerData.GetInstance().foodLayer, item.gameObject.layer))
                food = item.gameObject;
        }

        if (cup != null)
        {
            if (currentOrder.food.Equals(""))
                CompleteOrder();
            else
            {
                if (food != null && food.name.Contains(currentOrder.food)) 
                    CompleteOrder();
            }
        }
    }

    private void CompleteOrder()
    {
        // What happens when order is completed
        print("Order was compeleted");

        Destroy(cup);
        cup = null;

        if (food != null)
        {
            Destroy(food);
            food = null;
        }

        currentOrder = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(servingPoint.transform.position, halfBoxSizes * 2);
    }
}
