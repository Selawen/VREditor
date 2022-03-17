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
        block.AddComponent<BlockStruct>();

        StartCoroutine(SaveStruct());
        //hand.AttachObject(block, GrabTypes.Pinch);
            
        Debug.Log("spawned block: " + block);
    }

    public void Undo()
    {
        if (block != null)
        {
            GameObject.Find("Save").GetComponent<SaveScript>().RemoveBlock(block);
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

            block.AddComponent<BlockStruct>();
            StartCoroutine(SaveStruct());

            //hand.AttachObject(block, GrabTypes.Pinch);
            Debug.Log("redo spawn");
        } else {
            Debug.Log("nothing to redo");
        }
    }

    IEnumerator SaveStruct()
    {
        yield return new WaitForEndOfFrame();
        //ToDo: implement more block types
        GameObject.Find("Save").GetComponent<SaveScript>().AddBlock(block);
        block.GetComponent<BlockStruct>().SetStruct(blockPosition, transform.rotation, SaveScript.BlockType.Cube);
    }

}
