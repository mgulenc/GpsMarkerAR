using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void LoadARScene()
    {
        SceneManager.LoadScene("ArScene");
    }
    public void LoadPOIScene()
    {
        SceneManager.LoadScene("POIScene");
    }
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void ExitApplication()
    {
        Application.Quit();
        Debug.Log("Application quit");
    }

}