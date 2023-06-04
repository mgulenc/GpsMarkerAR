using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Button screenshotBtn;
    public Button videoRecordStartBtn;
    public Button videoRecordStopBtn;

    private void Start()
    {
        screenshotBtn.onClick.AddListener(TakeScreenshot);
        videoRecordStartBtn.onClick.AddListener(StartVideoRecording);
        videoRecordStopBtn.onClick.AddListener(StopVideoRecording);
    }

    public void TakeScreenshot()
    {
        Screenshot.TakeScreenShot();
    }

    public void StartVideoRecording()       
    {
        AndroidUtils.Singleton.StartRecording();
        Debug.Log("video recording started...");
    }

    public void StopVideoRecording()
    {
        AndroidUtils.Singleton.StopRecording();
        Debug.Log("video recording stopped...");
    }
}
