using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockLoader : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    private void Awake()
    {
        //create button for each block type in reference folder
        foreach (GameObject b in Resources.LoadAll<GameObject>("Blocks"))
        {
            GameObject button = Instantiate(buttonPrefab, gameObject.transform);
            button.GetComponent<BlockButton>().SetBlock(b);
            button.GetComponentInChildren<Text>().text = b.name;
        }

        //reset the layout to form a nice grid
        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        gameObject.SetActive(false);
    }
}
