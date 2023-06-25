using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ARLocation;
using Mapbox.Examples;
using Mapbox.Utils;
using TMPro;
using UnityEngine;
using static ARLocation.WebMapLoader;

public class DisplayAREnergyData : MonoBehaviour
{
    private WebMapLoader webMapLoader;

    private void Start()
    {
        webMapLoader = WebMapLoader.Instance; // eine Instanz des WebMapLoader   
    }

    private void OnMouseDown()
    {
        ArSceneUI.Instance.messagePanel.gameObject.SetActive(true); // aktiviert das Message Panel



        GameObject touchedObject = gameObject; // Das berührte Objekt

        DataEntry touchedEntry = GetEntryFromTouchedObject(touchedObject); // bringt den zugehörigen DataEntry für das berührte Objekt


        if (touchedEntry != null)
        {
            if (DownloadManager.Instance.SelectedEnergyType == EnergyType.PV)
            {
                // zeigt die Infos des berührten Objekts(Photovoltaik) im Message Text an
                ArSceneUI.Instance.messageText.text = "Energieart: " + touchedEntry.name + "\nInstalierte Leistung: " + touchedEntry.inst_leistung + " kWp\nAdresse: " + touchedEntry.adresse;
            }
            else
            {
                // zeigt die Infos des berührten Objekts(Windrad oder Biomasse) im Message Text an
                ArSceneUI.Instance.messageText.text = "Energieart: " + touchedEntry.name + "\nInstalierte Leistung: " + touchedEntry.inst_leistung + " kW\nAdresse: " + touchedEntry.adresse;
            }
        }
    }

    private DataEntry GetEntryFromTouchedObject(GameObject touchedObject)
    {
        PlaceAtLocation placeAtLocation = touchedObject.GetComponent<PlaceAtLocation>(); // holt das PlaceAtLocation-Komponente des berührten Objekts
        if (placeAtLocation != null)
        {
            double touchedLat = placeAtLocation.Location.Latitude;  // holt die Breitengradkoordinate des berührten Objekts
            double touchedLng = placeAtLocation.Location.Longitude; // holt die Längengradkoordinate des berührten Objekts

            // läuft DataEntries durch und sucht nach einem Eintrag mit den gleichen Koordinaten
            foreach (var entry in webMapLoader._dataEntries)
            {
                if (entry.lat == touchedLat && entry.lng == touchedLng)
                {
                    return entry; //gibt den gefundenen DataEntry zurück
                }
            }
        }

        return null; // Wenn kein Eintrag dann null zurück
    }

}


