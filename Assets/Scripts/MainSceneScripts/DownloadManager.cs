using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using Button = UnityEngine.UI.Button;


public class DownloadManager : MonoBehaviour
{
    [TextArea(5, 10000)]
    public string jsonFromWeb;
    [TextArea(5, 10000)]
    public string jsonWgsFormat;
    [SerializeField] private Toggle isWindrad;
    [SerializeField] private Toggle isBioMasse;
    [SerializeField] private Toggle isPV;
    [SerializeField] private Button DownloadBtn;
    [SerializeField] private Button EnergieQuelleKarteBtn;
    [SerializeField] private TextMeshProUGUI LoadStatusText;
    [SerializeField] private string url;
    public EnergyType SelectedEnergyType;
    public static DownloadManager Instance;
    public FeatureCollection featureCollection;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DownloadBtn.onClick.AddListener(DownloadBtn_Click);
        isWindrad.onValueChanged.AddListener(OnWindradToggleValueChanged);
        isBioMasse.onValueChanged.AddListener(OnBioMasseToggleValueChanged);
        isPV.onValueChanged.AddListener(OnWasserKraftToggleValueChanged);
    }

    private void Update()
    {
        if (!isWindrad.isOn && !isBioMasse.isOn && !isPV.isOn)
        {
            DownloadBtn.gameObject.SetActive(true);
            EnergieQuelleKarteBtn.gameObject.SetActive(false);
        }
    }

    private void OnWindradToggleValueChanged(bool value)
    {
        if (value)
        {
            isBioMasse.interactable = false;
            isPV.interactable = false;
            LoadStatusText.text = "";
            EnergieQuelleKarteBtn.gameObject.SetActive(false);
            DownloadBtn.gameObject.SetActive(true);
        }
        else
        {
            isBioMasse.interactable = true;
            isPV.interactable = true;
        }
    }

    private void OnBioMasseToggleValueChanged(bool value)
    {
        if (value)
        {
            isWindrad.interactable = false;
            isPV.interactable = false;
            LoadStatusText.text = "";
            EnergieQuelleKarteBtn.gameObject.SetActive(false);
            DownloadBtn.gameObject.SetActive(true);
        }
        else
        {
            isWindrad.interactable = true;
            isPV.interactable = true;
        }
    }

    private void OnWasserKraftToggleValueChanged(bool value)
    {
        if (value)
        {
            isWindrad.interactable = false;
            isBioMasse.interactable = false;
            LoadStatusText.text = "";
            EnergieQuelleKarteBtn.gameObject.SetActive(false);
            DownloadBtn.gameObject.SetActive(true);
        }
        else
        {
            isWindrad.interactable = true;
            isBioMasse.interactable = true;
        }
    }

    private void DownloadBtn_Click()
    {
        if (isWindrad.isOn)
        {
            // this.SelectedEnergzTzpe = EnergzTzpe.Wind;
            ChangeStatusToDownloading();
            url = "https://fbinter.stadt-berlin.de/fb/wfs/data/senstadt/s_wind_standort?service=wfs&version=2.0.0&request=GetFeature&typeNames=fis:s_wind_standort&outputFormat=application/json";
            StartCoroutine(GetOnlineEnergyData(url, EnergyType.Wind));
            Debug.Log("WINDRAD REQUEST");
            SelectedEnergyType = EnergyType.Wind;
        }
        else if(isBioMasse.isOn)
        {
            ChangeStatusToDownloading();
            url = "https://fbinter.stadt-berlin.de/fb/wfs/data/senstadt/s_biom_anlgr30kw?service=wfs&version=2.0.0&request=GetFeature&typeNames=fis:s_biom_anlgr30kw&outputFormat=application/json&filter=%3C?xml%20version=%221.0%22?%3E%20%3CFilter%20xmlns=%22http://www.opengis.net/ogc%22%20xmlns:v=%22http://data.linz.govt.nz/ns/v%22%20xmlns:gml=%22http://www.opengis.net/gml%22%3E%3CPropertyIsGreaterThanOrEqualTo%3E%3CPropertyName%3Einst_leistung%3C/PropertyName%3E%3CLiteral%3E500%3C/Literal%3E%3C/PropertyIsGreaterThanOrEqualTo%3E%3C/Filter%3E";
            StartCoroutine(GetOnlineEnergyData(url,EnergyType.Bio));
            Debug.Log("BIOMASSE REQUEST");
            SelectedEnergyType = EnergyType.Bio;
        }
        else if(isPV.isOn)
        {
            ChangeStatusToDownloading();
            url = "https://fbinter.stadt-berlin.de/fb/wfs/data/senstadt/s08_09_01_ortab30kw?service=wfs&version=2.0.0&request=GetFeature&typeNames=fis:s08_09_01_ortab30kw&outputFormat=application/json&filter=%3C?xml%20version=%221.0%22?%3E%20%3CFilter%20xmlns=%22http://www.opengis.net/ogc%22%20xmlns:v=%22http://data.linz.govt.nz/ns/v%22%20xmlns:gml=%22http://www.opengis.net/gml%22%3E%3CPropertyIsGreaterThanOrEqualTo%3E%3CPropertyName%3Einst_leistung%3C/PropertyName%3E%3CLiteral%3E500%3C/Literal%3E%3C/PropertyIsGreaterThanOrEqualTo%3E%3C/Filter%3E";
            StartCoroutine(GetOnlineEnergyData(url, EnergyType.PV));
            Debug.Log("PV REQUEST");
            SelectedEnergyType = EnergyType.PV;
        }
        else
        {
            LoadStatusText.text = "Bitte wähle einer der dargestellten Optionen aus..";
            LoadStatusText.color = Color.white;
        }
    }

    IEnumerator GetOnlineEnergyData(string url, EnergyType energyType)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Request successful
            Debug.Log("GET request for EE-Quellen successful!");
            Debug.Log("Response from Berlin-WFS: " + request.downloadHandler.text);
            jsonFromWeb = request.downloadHandler.text;
            yield return SendGetRequestToConvert(ConvertLatLonToEpsg4326());
            ChangeStatusToDownloadComplete(energyType);
        }
        else
        {
            // Request failed
            ChangeStatusNoInternet();
            Debug.LogError("GET request failed: " + request.error);
        }
    }

    IEnumerator SendGetRequestToConvert(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Request successful
            Debug.Log("GET request for the converting of coordinates successful!");
            Debug.Log("Response from Coordinates : " + request.downloadHandler.text);
            jsonWgsFormat = request.downloadHandler.text;
        }
        else
        {
            // Request failed
            Debug.LogError("GET request failed: " + request.error);
        }
    }

    private string ConvertLatLonToEpsg4326()
    {
        string urlForConvertManyPoints = "https://epsg.io/trans?data=";

        List<Vector2> coordinates = ExtractCoordinates(jsonFromWeb); // Annahme: Die Liste der Koordinaten ist bereits vorhanden
        
        for (int i = 0; i < coordinates.Count; i++)
        {
            Vector2 coordinate = coordinates[i];

            // Ersetze Kommas durch Punkte in den Koordinaten
            string xCoordinate = coordinate.x.ToString().Replace(",", ".");
            string yCoordinate = coordinate.y.ToString().Replace(",", ".");

            // Füge die Koordinaten in den URL-String ein
            urlForConvertManyPoints += xCoordinate + "," + yCoordinate;

            // Füge ein Semikolon hinzu, wenn es nicht das letzte Element ist
            if (i < coordinates.Count - 1)
            {
                urlForConvertManyPoints += ";";
            }
        }

        // Füge die übrigen URL-Parameter hinzu
        urlForConvertManyPoints += "&s_srs=25833&t_srs=4326";

        //string urlForConvert = "https://epsg.io/trans?x=" + longitude + "&y=" + latitude + "&s_srs=25833&t_srs=4326";
        Debug.Log("URL for the Converting : " + urlForConvertManyPoints);

        return urlForConvertManyPoints;
    }
    
    public static List<Vector2> ExtractCoordinates(string json)
    {
        FeatureCollection renewableEnergy = JsonConvert.DeserializeObject<FeatureCollection>(json);
        // Deserialisiere das JSON in eine erneuerbare Quelle

        List<Vector2> coordinateList = new List<Vector2>();

        // Iteriere über alle Features und extrahiere die Koordinaten
        foreach (Feature feature in renewableEnergy.features)
        {
            double x = feature.geometry.coordinates[0];
            double y = feature.geometry.coordinates[1];

            Vector2 coordinate = new Vector2((float)x, (float)y);
            coordinateList.Add(coordinate);
        }
        
        return coordinateList;
    }

    private void ChangeStatusToDownloadComplete(EnergyType energyType)
    {
        FeatureCollection featureCollection = JsonConvert.DeserializeObject<FeatureCollection>(jsonFromWeb);

        LoadStatusText.text = featureCollection.totalFeatures + " " + energyType + " - Energieanlagen in Ihrer Nähe";

        LoadStatusText.color = Color.white;

        DownloadBtn.gameObject.SetActive(false);

        EnergieQuelleKarteBtn.gameObject.SetActive(true);

    }

    private void ChangeStatusNoInternet()
    {
        LoadStatusText.text = "no internet connection or another problem...";
        LoadStatusText.color = Color.red;
    }

    private void ChangeStatusToDownloading()
    {
        LoadStatusText.text = "Downloading...";
        LoadStatusText.color = Color.yellow;
    }

}