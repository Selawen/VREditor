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

    //ToDo: implemet more blck types
    [SerializeField] private GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        saveObject = new SaveObject();
        blockList = new List<GameObject>();
}

// Update is called once per frame
void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            saveObject = new SaveObject();
            if (saveObject == null) Debug.Log("saveobj went wrong");

            foreach (GameObject b in blockList)
            {
                if (b != null)
                {
                    BlockStruct bs = new BlockStruct(b.transform.position, b.transform.rotation, BlockType.Cube);
                    //bs.SetStruct(b.transform.position, b.transform.rotation, BlockType.Cube);
                    if (bs != null)
                    {
                        saveObject.blocks.Add(bs);
                    } else
                    {
                        Debug.Log("somesting went wrong creating a struct");
                    }
                }
            }

            //saveJson = JsonUtility.ToJson(saveObject);
            saveJson = saveObject.ToSaveString();
            
            string url = Path.Combine(Application.dataPath, path);
            StreamWriter streamWriter = new StreamWriter(url, false);

            streamWriter.Write(saveJson);
            streamWriter.Flush();
            streamWriter.Close();

            Debug.Log("save");
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (cubePrefab != null)
            {
                string url = Path.Combine(Application.dataPath, path);
                StreamReader streamReader = new StreamReader(url, false);
                if (streamReader != null)
                {
                    saveJson = streamReader.ReadToEnd();
                    streamReader.Close();

                    JsonUtility.FromJsonOverwrite(saveJson, saveObject);

                    saveObject.ReadyLoad();

                    blockList = new List<GameObject>();

                    foreach (BlockStruct b in saveObject.blocks)
                    {
                        if (b != null)
                        {
                            GameObject block = Instantiate(cubePrefab, b.Position(), b.Rotation());
                            blockList.Add(block);
                        }
                    }
                } else {
                    Debug.Log("no file found");
                }
            } else {
                Debug.Log("no prefab assigned");
            }
            Debug.Log("load");
        }
    }

    public void AddBlock(GameObject b)
    {
        if (!blockList.Contains(b)){
            blockList.Add(b);
            Debug.Log("added block");
        }
    }

    public void RemoveBlock(GameObject b)
    {
        if (blockList.Contains(b))
            blockList.Remove(b);
    }

    [Serializable]
    public enum BlockType
    {
        Cube,
        Plane
    }
}
