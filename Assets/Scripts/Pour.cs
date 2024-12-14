using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pour : Holdable
{
    public float inclinationThreshold;
    public float basePouringSpeed;
    public GameObject pouringPoint;

    void Update()
    {
        if (isHeld)
        {
            float inclination = Vector3.Angle(Vector3.up, transform.up);
            if (inclination > inclinationThreshold)
            {
                if (Physics.Raycast(pouringPoint.transform.position, Vector3.down, out RaycastHit hit, float.MaxValue, PlayerData.GetInstance().cupLayer))
                {
                    Cup cup = PlayerData.GetInstance().cup;
                    if (cup != null && hit.collider.gameObject.Equals(cup.gameObject))
                        cup.IncreaseCoffee(inclination * basePouringSpeed * Time.deltaTime);
                }
            }
        }
    }

    public override void PickUp()
    {
        base.PickUp();
        PlayerData.GetInstance().pour = this;
    }

    public override void PutDown()
    {
        base.PutDown();
        PlayerData.GetInstance().pour = null;
    }
}
