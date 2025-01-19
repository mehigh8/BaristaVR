using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR;

public class HandUIManager : MonoBehaviour
{
    public GameObject uiObject;
    public GameObject coffeeIndicator;
    public GameObject milkIndicator;
    public TMP_Text foodText;

    private bool isShown = false;

    private void OnEnable()
    {
        SteamVR_Actions._default.OpenUI.onChange += OnOpenUIChanged;
    }

    private void OnDisable()
    {
        SteamVR_Actions._default.OpenUI.onChange -= OnOpenUIChanged;
    }

    private void OnOpenUIChanged(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool openUIState)
    {
        uiObject.SetActive(openUIState);
        isShown = openUIState;
    }

    private void Update()
    {
        if (isShown)
            uiObject.transform.LookAt(uiObject.transform.position - (Camera.main.transform.position - uiObject.transform.position));

        if (OrderManager.GetInstance() != null) {
            coffeeIndicator.transform.localScale = new Vector3(coffeeIndicator.transform.localScale.x, OrderManager.GetInstance().currentOrder.coffee.x, coffeeIndicator.transform.localScale.z);
            milkIndicator.transform.localScale = new Vector3(milkIndicator.transform.localScale.x, OrderManager.GetInstance().currentOrder.coffee.y, milkIndicator.transform.localScale.z);

            if (OrderManager.GetInstance().currentOrder.food.Equals(""))
                foodText.text = "";
            else
                foodText.text = "+\n" + OrderManager.GetInstance().currentOrder.food;
        }
    }
}
