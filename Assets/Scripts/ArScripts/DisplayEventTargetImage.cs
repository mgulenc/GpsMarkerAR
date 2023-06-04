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
        /*
        citizenLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
        var currentCitizenLocation = new GeoCoordinatePortable.GeoCoordinate(citizenLocation.GetLocationLat(), citizenLocation.GetLocationLon());
        var eventLocation = new GeoCoordinatePortable.GeoCoordinate(EventPosition[0], EventPosition[1]);
        Debug.Log("hello " + EventPosition[0] + " , " + EventPosition[1] + "\nEvent ID: " + EventID);
        var distance = currentCitizenLocation.GetDistanceTo(eventLocation);

        UIManager.Instance.eventPanel.SetActive(true);

        if (distance < 1000)
        {
            UIManager.Instance.eventTextField.text = "Your location : " + currentCitizenLocation + "\n\nDistance of the target " + EventID + " to your location : " + Math.Round(distance, 2) + " meter";
        }
        else
        {
            UIManager.Instance.eventTextField.text = "Your location : " + currentCitizenLocation + "\n\nDistance of the target " + EventID + " to your location : " + Math.Round((distance / 1000), 2) + " km";
        }
        */
        ArSceneUI.Instance.messagePanel.gameObject.SetActive(true);
        ArSceneUI.Instance.messageText.text = "Wenn Sie diese Nachricht lesen, haben Sie wahrscheinlich das AR-Objekt berührt.";

    }

}
