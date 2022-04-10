using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// toggle block choosing panel on and off
/// </summary>
public class PanelButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    [SerializeField] private string text1, text2;

    void Awake()
    {
        GetComponentInChildren<Text>().text = text1;
    }
    public void TogglePickerPanel()
    {
        foreach (GameObject panel in panels)
        {
            if (panel == null)
            {
                Debug.Log("no panel set");
                return;
            }
            panel.SetActive(!panel.activeSelf);
        }

        Text label = GetComponentInChildren<Text>();
        if (label.text == text1)
        {
            label.text = text2;
        }
        else
        {
            label.text = text1;
        } 
    }

    public void TogglePanel()
    {
        if (panels[0] == null)
        {
            Debug.Log("no panel set");
            return;
        }
        panels[0].SetActive(!panels[0].activeSelf);
    }
}
