using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public static void TakeScreenShot()
    {
        string folderPath;

#if UNITY_EDITOR
        folderPath = "Assets/Resources/Screenshots/"; // the path of your project folder
        if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
            System.IO.Directory.CreateDirectory(folderPath);  // it will get created

      //ScreenCapture.CaptureScreenshot(folderPath + "/Screenshot_" + step.Name.Substring(0,15) + "..._ " + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png", 2);
        ScreenCapture.CaptureScreenshot(folderPath + "/Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png", 2);

#endif

#if UNITY_ANDROID && !UNITY_EDITOR
            ScreenCapture.CaptureScreenshot("/Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png", 2);
#endif
        //StepContent.Instance.ToggleWarningText.text = "A screenshot was taken!";
        Debug.Log("A screenshot was taken!");
    }
}


