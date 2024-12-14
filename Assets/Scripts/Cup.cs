using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : Holdable
{
    public GameObject coffee;

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

    public void IncreaseCoffee(float quantity)
    {
        coffee.transform.localScale += new Vector3(0, quantity, 0);
        if (coffee.transform.localScale.y > 1)
            coffee.transform.localScale = new Vector3(coffee.transform.localScale.x, 1, coffee.transform.localScale.z);
    }
}
