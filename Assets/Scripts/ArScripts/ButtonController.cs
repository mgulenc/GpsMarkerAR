using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GameObject[] buttonsToToggle;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ToggleButtons);
    }

    private void ToggleButtons()
    {
        foreach (GameObject button in buttonsToToggle)
        {
            button.SetActive(!button.activeSelf);
        }
    }
}
