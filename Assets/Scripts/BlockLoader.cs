using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockLoader : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    private void Awake()
    {
        foreach (GameObject b in Resources.LoadAll<GameObject>("Blocks"))
        {
            GameObject button = Instantiate(buttonPrefab, gameObject.transform);
            button.GetComponent<BlockButton>().SetBlock(b);
            button.GetComponentInChildren<Text>().text = b.name;
        }

        LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
        gameObject.SetActive(false);
    }
}
