using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using Mapbox.Utils;
using System;

public class PoiPointer : MonoBehaviour
{
    public static PoiPointer Instance;
    public void Awake()
    {
        Instance = this;
    }

//    [SerializeField] private float rotationSpeed = 50f;
//    [SerializeField] private float amplitude = 0f;
//    [SerializeField] private float freuquency = 0.50f;

    public LocationStatus citizenLocation;
    public Vector2d PointPosition;
    public int poiID;
    public Feature feature;

    // Update is called once per frame
    void Update()
    {
        PointerPosition();
    }

    private void PointerPosition()
    {
        // transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        // transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.fixedTime * Mathf.PI * freuquency) * amplitude) + 2, transform.position.z);
        transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }

    private void OnMouseDown()
    {
        UIManager.Instance.eventPanel.SetActive(true);

        citizenLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
        var currentCitizenLocation = new GeoCoordinatePortable.GeoCoordinate(citizenLocation.GetLocationLat(), citizenLocation.GetLocationLon());
        var pointLocation = new GeoCoordinatePortable.GeoCoordinate(PointPosition[0], PointPosition[1]);
        var distance = currentCitizenLocation.GetDistanceTo(pointLocation);

        string energyType = DownloadManager.Instance.SelectedEnergyType == EnergyType.PV ? "zur PV-Anlage" : DownloadManager.Instance.SelectedEnergyType == EnergyType.Wind ? "zum Windrad" : "zur Biomasse";
        string distanceUnit = distance < 1000 ? "m" : "km";
        string formattedDistance = Math.Round(distance / (distanceUnit == "km" ? 1000 : 1), 2).ToString();
        string powerUnit = DownloadManager.Instance.SelectedEnergyType == EnergyType.PV ? "kWp" : "kW";

        string eventText = $"Ihr aktueller Standort: {currentCitizenLocation}\n\n" +
            $"Ihre Entfernung {energyType} ca. {formattedDistance} {distanceUnit}\n" +
            $"Energieleistung: {feature.properties.inst_leistung} {powerUnit}\n";

        if (DownloadManager.Instance.SelectedEnergyType == EnergyType.PV || DownloadManager.Instance.SelectedEnergyType == EnergyType.Bio)
        {
            feature.properties.adresse = feature.properties.strasse_flur_lang;
        }

        eventText += $"Adresse: {feature.properties.adresse} {feature.properties.plz}";
        UIManager.Instance.eventTextField.text = eventText;
    }
}

