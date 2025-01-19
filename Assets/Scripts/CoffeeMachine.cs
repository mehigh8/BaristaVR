using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    public GameObject pouringPoint;
    public float basePouringSpeed;

    private bool isPressed = false;

    private void Update()
    {
        if (isPressed)
        {
            if (Physics.Raycast(pouringPoint.transform.position, Vector3.down, out RaycastHit hit, float.MaxValue, PlayerData.GetInstance().cupLayer))
            {
                Cup cup = hit.collider.gameObject.GetComponent<Cup>();
                if (cup != null)
                    cup.IncreaseCoffee(basePouringSpeed * Time.deltaTime);
            }
        }
    }

    public void Press()
    {
        print("am apasat");
        isPressed = true;
    }

    public void Unpress()
    {
        print("nu am mai apasat");
        isPressed = false;
    }
}
