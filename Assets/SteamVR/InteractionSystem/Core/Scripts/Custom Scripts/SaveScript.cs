using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveScript : MonoBehaviour
{
    [SerializeField] private string path = "levelSave";
    public List<GameObject> blockList = new List<GameObject>();
    private SaveObject saveObject;                        //object to encapsulate list for saving using json
    private string saveJson;                                //the json string for saving level layout

    [SerializeField] private GameObject blockPrefab;

    void Start()
    {
        saveObject = new SaveObject();
        blockList = new List<GameObject>();
    }

void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SaveLevel();
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevel();
        }
    }

    /// <summary>
    /// save current level to json file when e is pressed
    /// </summary>
    public void SaveLevel()
    {
        saveObject = new SaveObject();
        if (saveObject == null) Debug.Log("saveobj went wrong");

        foreach (GameObject b in blockList)
        {
            if (b != null)
            {
                BlockStruct bs = new BlockStruct(b.transform.position, b.transform.rotation, b.tag, b.transform.lossyScale);
                //bs.SetStruct(b.transform.position, b.transform.rotation, BlockType.Cube);
                if (bs != null)
                {
                    saveObject.blocks.Add(bs);
                }
                else
                {
                    Debug.Log("something went wrong creating a struct");
                }
            }
        }

        saveJson = saveObject.ToSaveString();

        string url = Path.Combine(Application.dataPath, path);
        StreamWriter streamWriter = new StreamWriter(url, false);

        streamWriter.Write(saveJson);
        streamWriter.Flush();
        streamWriter.Close();

        Debug.Log("save");
    }

    /// <summary>
    /// load level fron json file when L is pressed
    /// </summary>
    public void LoadLevel()
    {
        string url = Path.Combine(Application.dataPath, path);
        StreamReader streamReader = new StreamReader(url, false);
        if (streamReader != null)
        {
            saveJson = streamReader.ReadToEnd();
            streamReader.Close();

            JsonUtility.FromJsonOverwrite(saveJson, saveObject);

            saveObject.ReadyLoad();

            foreach (GameObject block in blockList)
            {
                Destroy(block);
            }
            blockList = new List<GameObject>();

            foreach (BlockStruct b in saveObject.blocks)
            {
                if (b != null)
                {
                    if (b.typeString == null)
                    {
                        blockPrefab = Resources.Load<GameObject>("Blocks/Cube");
                    }
                    else
                    {
                        blockPrefab = Resources.Load<GameObject>("Blocks/" + b.typeString);
                    }

                    if (blockPrefab != null)
                    {
                        GameObject block = Instantiate(blockPrefab, b.Position(), b.Rotation());
                        if (b.Scale() != Vector3.zero)
                            block.transform.localScale = b.Scale();

                        blockList.Add(block);
                    }
                    else
                    {
                        Debug.Log("prefab block not found");
                    }
                }
            }
        }
        else
        {
            Debug.Log("no file found");
        }
        Debug.Log("load");
    }

    /// <summary>
    /// Add block to list containing all blocks in the level
    /// </summary>
    /// <param name="b">block to add</param>
    public void AddBlock(GameObject b)
    {
        if (!blockList.Contains(b)){
            blockList.Add(b);
        }
    }

    /// <summary>
    /// Remove block from list containing all blocks in the level
    /// </summary>
    /// <param name="b">block to remove</param>
    public void RemoveBlock(GameObject b)
    {
        if (blockList.Contains(b))
            blockList.Remove(b);
    }

    [Serializable]
    public enum BlockType
    {
        Cube,
        Plane,
        Sphere
    }
}
