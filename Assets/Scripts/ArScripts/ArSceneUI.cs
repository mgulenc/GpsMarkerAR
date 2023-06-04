using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArSceneUI : MonoBehaviour
{
    //public TextMeshPro messageText;
    public TextMeshProUGUI messageText;
    public GameObject messagePanel;
    public static ArSceneUI Instance;
    public void Awake()
    {
        Instance = this;
    }
}
