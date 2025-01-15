using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cup : Holdable
{
    public GameObject coffee;
    public Renderer coffeeRenderer;
    public Image coffeeIndicator;
    public Image milkIndicator;
    public Color coffeeColor;
    public Color milkColor;

    private bool isFull = false;

    public float coffeeAmount;
    public float milkAmount;
    public float totalAmount;

    private void Update()
    {
        coffeeAmount = coffeeIndicator.transform.localScale.y;
        milkAmount = milkIndicator.transform.localScale.y;
        totalAmount = coffeeAmount + milkAmount;

        if (totalAmount > 0.01f)
        {
            print("Sunt aici");
            Color color = Color.Lerp(coffeeColor, milkColor, ColorMixFunction(coffeeAmount / totalAmount));
            coffeeRenderer.material.color = color;
        }
    }

    private float ColorMixFunction(float x)
    {
        return Mathf.Pow(1f / Mathf.Exp(x), 3);
    }

    public override void PickUp()
    {
        base.PickUp();
        PlayerData.GetInstance().cup = this;
    }

    public override void PutDown()
    {
        base.PutDown();
        PlayerData.GetInstance().cup = null;
    }

    private void IncreaseLiquid(float quantity)
    {
        coffee.transform.localScale += new Vector3(0, quantity, 0);
        if (coffee.transform.localScale.y > 1)
        {
            isFull = true;
            coffee.transform.localScale = new Vector3(coffee.transform.localScale.x, 1, coffee.transform.localScale.z);
        }
    }

    public void IncreaseCoffee(float quantity)
    {
        IncreaseLiquid(quantity);

        if (!isFull)
        {
            coffeeIndicator.transform.localScale += new Vector3(0, quantity, 0);
            if (coffeeIndicator.transform.localScale.y > 1)
                coffeeIndicator.transform.localScale = new Vector3(coffeeIndicator.transform.localScale.x, 1, coffeeIndicator.transform.localScale.z);
        }
    }

    public void IncreaseMilk(float quantity)
    {
        IncreaseLiquid(quantity);

        if (!isFull)
        {
            milkIndicator.transform.localScale += new Vector3(0, quantity, 0);
            if (milkIndicator.transform.localScale.y > 1)
                milkIndicator.transform.localScale = new Vector3(milkIndicator.transform.localScale.x, 1, milkIndicator.transform.localScale.z);
        }
    }
}
