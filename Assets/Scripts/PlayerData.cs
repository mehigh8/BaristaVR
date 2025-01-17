using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public Cup cup = null;
    public Pour pour = null;
    public LayerMask cupLayer;
    public HandUIManager handUIManager;

    private static PlayerData instance;

    void OnEnable()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public static PlayerData GetInstance() { return instance; }
}
