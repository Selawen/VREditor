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
            saveObject = new SaveObject();
            if (saveObject == null) Debug.Log("saveobj went wrong");

            foreach (GameObject b in blockList)
            {
                if (b != null)
                {
                    BlockType blockType = BlockType.Cube;

                    switch (b.tag)
                    {
                        case ("Cube"):
                            blockType = BlockType.Cube;
                            break;
                        case ("Plane"):
                            blockType = BlockType.Plane;
                            break;
                        case ("Sphere"):
                            blockType = BlockType.Sphere;
                            break;
                        default:
                            break;
                    }

                    BlockStruct bs = new BlockStruct(b.transform.position, b.transform.rotation, blockType);
                    //bs.SetStruct(b.transform.position, b.transform.rotation, BlockType.Cube);
                    if (bs != null)
                    {
                        saveObject.blocks.Add(bs);
                    } else
                    {
                        Debug.Log("something went wrong creating a struct");
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
            string url = Path.Combine(Application.dataPath, path);
            StreamReader streamReader = new StreamReader(url, false);
            if (streamReader != null)
            {
                saveJson = streamReader.ReadToEnd();
                streamReader.Close();

                JsonUtility.FromJsonOverwrite(saveJson, saveObject);

                saveObject.ReadyLoad();

                foreach(GameObject block in blockList)
                {
                    Destroy(block);
                }
                blockList = new List<GameObject>();

                foreach (BlockStruct b in saveObject.blocks)
                {
                    if (b != null)
                    {
                        switch (b.Type())
                        {
                            case (BlockType.Cube):
                            {
                                    blockPrefab = Resources.Load<GameObject>("Blocks/Cube");
                                break;
                            }
                            case (BlockType.Sphere):
                            {
                                    blockPrefab = Resources.Load<GameObject>("Blocks/Sphere");
                                break;
                            }
                            case (BlockType.Plane):
                            {
                                blockPrefab = Resources.Load<GameObject>("Blocks/Plane");
                                break;
                            }
                            default:
                            {
                                blockPrefab = Resources.Load<GameObject>("Blocks/Cube");
                                break;
                            }
                        }
                        if (blockPrefab != null)
                        {
                            GameObject block = Instantiate(blockPrefab, b.Position(), b.Rotation());
                            blockList.Add(block);
                        } else
                        {
                            Debug.Log("prefab block not found");
                        }
                    }
                }
            } else {
                Debug.Log("no file found");
            }
            Debug.Log("load");
        }
    }

    public void AddBlock(GameObject b)
    {
        if (!blockList.Contains(b)){
            blockList.Add(b);
            //Debug.Log("added block");
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
        Plane,
        Sphere
    }
}
