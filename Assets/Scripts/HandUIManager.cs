using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR;

public class HandUIManager : MonoBehaviour
{
    public GameObject uiObject;
    public TMP_Text recipeText;

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
    }
}
