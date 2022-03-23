using UnityEngine;
using UnityEngine.EventSystems;
 
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
