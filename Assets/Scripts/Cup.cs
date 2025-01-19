using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cup : Holdable
{
    public GameObject coffee;
    public Renderer coffeeRenderer;
    public Transform coffeeIndicator;
    public Transform milkIndicator;
    public Color coffeeColor;
    public Color milkColor;
    [SerializeField] private Transform canvasPivot;
    
    private bool isFull = false;

    public float coffeeAmount;
    public float milkAmount;
    public float totalAmount;
    private GameObject playerCam;
    private void Start()
    {
        playerCam = Camera.main.gameObject;
        if (playerCam)
            print("got gamera");
        else
            print("failed to get cam");
    }

    private void RotateCanvas()
    {
        canvasPivot.forward = -(playerCam.transform.position - canvasPivot.position).normalized;
    }
    
    private void Update()
    {
        RotateCanvas();
        coffeeAmount = coffeeIndicator.localScale.y;
        milkAmount = milkIndicator.localScale.y;
        totalAmount = coffeeAmount + milkAmount;
        
        if (totalAmount > 0.01f)
        {
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
            coffeeIndicator.localScale += new Vector3(0, quantity, 0);
            if (coffeeIndicator.localScale.y > 1)
                coffeeIndicator.localScale = new Vector3(coffeeIndicator.localScale.x, 1, coffeeIndicator.localScale.z);
        }
    }

    public void IncreaseMilk(float quantity)
    {
        IncreaseLiquid(quantity);

        if (!isFull)
        {
            milkIndicator.localScale += new Vector3(0, quantity, 0);
            if (milkIndicator.localScale.y > 1)
                milkIndicator.localScale = new Vector3(milkIndicator.localScale.x, 1, milkIndicator.localScale.z);
        }
    }
}
