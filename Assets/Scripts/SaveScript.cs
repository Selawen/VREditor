using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveScript : MonoBehaviour
{
    [SerializeField] private string path = "levelSave";
    public List<GameObject> blockList = new List<GameObject>();
    protected SaveObject saveObject;                        //object to encapsulate list for saving using json
    private string saveJson;                                //the json string for saving level layout

    //ToDo: implemet more blck types
    [SerializeField] private GameObject cubePrefab;

    [Serializable]
    public struct SaveObject
    {
        public List<BlockStruct> blocks;
    }


    // Start is called before the first frame update
    void Start()
    {
        saveObject = new SaveObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (GameObject b in blockList)
            {
                BlockStruct bs = b.GetComponent<BlockStruct>();
                bs.SetStruct(b.transform.position, b.transform.rotation, BlockType.Cube);
                saveObject.blocks.Add(bs);
            }

            saveJson = JsonUtility.ToJson(saveObject);
            
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

                    foreach (BlockStruct b in saveObject.blocks)
                    {
                        Instantiate(cubePrefab, b.Position(), b.Rotation());
                    }
                }
                else
                {
                    Debug.Log("no file found");
                }
            } else
            {
                Debug.Log("no prefab assigned");
            }
            Debug.Log("load");
        }
    }

    public void AddBlock(GameObject b)
    {
        if (!blockList.Contains(b))
        {
            blockList.Add(b);
            Debug.Log("added block");
        }
    }

    public void RemoveBlock(GameObject b)
    {
        if (blockList.Contains(b))
            blockList.Remove(b);
    }

    public enum BlockType
    {
        Cube,
        Plane
    }
}
