using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCommand : MonoBehaviour, ICommand
{
    private GameObject block = null;
    private GameObject blockPrefab = null;
    private Vector3 blockPosition;

    public SpawnCommand(GameObject prefab, Vector3 position)
    {
        blockPrefab = prefab;
        blockPosition = position;
    }
    public void Execute()
    {
        block = Instantiate(blockPrefab);

            block.transform.position = blockPosition;
            //hand.AttachObject(block, GrabTypes.Pinch);
            
            Debug.Log("spawned block: " + block);
        Debug.Log("execute spawn");
    }

    public void Undo()
    {
        if (block != null)
        {
            Destroy(block);
            Debug.Log("undo spawn");
        } else {
            Debug.Log("nothing to undo");
        }
        
    }

    public void Redo()
    {
        if (block == null)
        {
            block = Instantiate(blockPrefab);
            block.transform.position = blockPosition;
            //hand.AttachObject(block, GrabTypes.Pinch);
            Debug.Log("redo spawn");
        } else {
            Debug.Log("nothing to redo");
        }
    }

}
