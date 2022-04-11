using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCommand : MonoBehaviour, ICommand
{
    private GameObject block = null;
    private GameObject blockPrefab = null;
    private Vector3 blockPosition;
    private SaveScript saveScript;

    void Start()
    {
        if (blockPrefab == null)
        {
            Debug.Log(Resources.Load<GameObject>("Blocks/Cube").name);
        }
        saveScript = GameObject.Find("Save").GetComponent<SaveScript>();
    }

    /// <summary>
    /// Command to retain the spawning of a block
    /// </summary>
    /// <param name="prefab">the prefab to use as a base for the block</param>
    /// <param name="position">the position to spawn at</param>
    public SpawnCommand(GameObject prefab, Vector3 position)
    {
        blockPrefab = prefab;
        blockPosition = position;
    }
    public void Execute()
    {
        block = Instantiate(blockPrefab);

        block.transform.position = blockPosition;
        block.tag = blockPrefab.name;

        saveScript.AddBlock(block);

        //Debug.Log("spawned block: " + block);
    }

    public void Undo()
    {
        if (block != null)
        {
            saveScript.RemoveBlock(block);
            block.SetActive(false);
            //Debug.Log("undo spawn");
        } else {
            Debug.Log("nothing to undo");
        }
        
    }

    public void Redo()
    {
        if (block != null)
        {
            block.SetActive(true);

            saveScript.AddBlock(block);
            //Debug.Log("redo spawn");
        } else {
            Debug.Log("nothing to redo");
        }
    }
}
