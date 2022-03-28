using UnityEngine;
using UnityEngine.EventSystems;
 
/// <summary>
/// toggle block choosing panel on and off
/// </summary>
public class PanelButton : MonoBehaviour
{
    [SerializeField] private GameObject panel;
     public void TogglePickerPanel()
    {
        if (panel == null)
        {
            Debug.Log("no panel set");
            return;
        }
        panel.SetActive(!panel.activeSelf);
    }
}
