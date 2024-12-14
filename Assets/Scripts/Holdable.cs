using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    public bool isHeld = false;

    public virtual void PickUp()
    {
        isHeld = true;
    }

    public virtual void PutDown()
    {
        isHeld = false;
    }
}
