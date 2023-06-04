using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI eventTextField;
    public Button exitBtn;
    public static UIManager Instance;
    public GameObject eventPanel;
    public Text poiMessageTxt;

    private void Start()
    {
        UpdatePoiTextMessage();
    }

    public void Awake()
    {
        Instance = this;
    }

    public void Exit_EventCanvas()
    {
        eventPanel.SetActive(false);
    }

    private void UpdatePoiTextMessage()
    {
        if (DownloadManager.Instance.SelectedEnergyType == EnergyType.Wind)
        {
            poiMessageTxt.text = "In der Umgebung von Berlin werden alle Windräder als POIs angezeigt. Berlin ist als Bundesland nicht besonders reich an Windenergieanlagen." +
                                 "\n\nWenn Sie sich in der Nähe eines beliebigen POIs befinden, können Sie mit dem Button 'Zur AR-Szene wechseln' in den AR-Modus wechseln " +
                                 "und die Energiequelle auf Ihrem Kamerabildschirm an ihrem Standort betrachten.Um eine andere Art von EE-Quelllen anzusehen, bitte gehen Sie auf die Startseite.";
        }
        else if (DownloadManager.Instance.SelectedEnergyType == EnergyType.Bio)
        {
            poiMessageTxt.text = "Aufgrund der Vielzahl von Biomasseanlagen in der Umgebung wurden Biomassen mit einer Leistung von über 500 kW gefiltert und auf der Karte dargestellt." +
                                 "\n\nWenn Sie sich in der Nähe eines beliebigen POIs befinden, können Sie mit dem Button 'Zur AR-Szene wechseln' in den AR-Modus wechseln " +
                                 "und die Energiequelle auf Ihrem Kamerabildschirm an ihrem Standort betrachten.Um eine andere Art von EE-Quelllen anzusehen, bitte gehen Sie auf die Startseite.";
        }
        else
        {
            poiMessageTxt.text = "Aufgrund der Vielzahl von PV-Anlagen in der Umgebung wurden Photovoltaikanlagen mit einer Leistung von über 500 kW gefiltert und auf der Karte dargestellt." +
                                 "\n\nWenn Sie sich in der Nähe eines beliebigen POIs befinden, können Sie mit dem Button 'Zur AR-Szene wechseln' in den AR-Modus wechseln " +
                                 "und die Energiequelle auf Ihrem Kamerabildschirm an ihrem Standort betrachten.Um eine andere Art von EE-Quelllen anzusehen, bitte gehen Sie auf die Startseite.";
        }
    }
}
