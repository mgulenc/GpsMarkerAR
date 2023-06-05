using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Utils;
using TMPro;
using UnityEngine;

public class DisplayEventTargetImage : MonoBehaviour
{

    public LocationStatus citizenLocation;
    public Vector2d EventPosition;
    public int EventID;
    public string Name;
    public int Speed;

    private void OnMouseDown()
    {
        ArSceneUI.Instance.messagePanel.gameObject.SetActive(true);
        ArSceneUI.Instance.messageText.text = "Sie haben ein AR-Objekt berührt.";

    }

}
