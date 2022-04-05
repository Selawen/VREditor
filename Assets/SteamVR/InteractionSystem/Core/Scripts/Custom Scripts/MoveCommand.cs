using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Command to save how a block was moved
/// </summary>
public class MoveCommand : MonoBehaviour, ICommand
{
    private Vector3 newPosition, oldPosition;
    private Quaternion newRotation, oldRotation;
    private GameObject block;

    /// <summary>
    /// Command to retain how a block was moved
    /// </summary>
    public MoveCommand(Vector3 newPos, Quaternion newRot, Vector3 oldPos, Quaternion oldRot, GameObject movedBlock)
    {
        newPosition = newPos;
        newRotation = newRot;
        oldPosition = oldPos;
        oldRotation = oldRot;
        block = movedBlock;
    }
    
    public void Execute()
    {
        //Debug.Log("block moved from: " + oldPosition + " to: " + newPosition);
    }

    public void Redo()
    {
        if (block == null) 
        {
            Debug.Log("no block found");
            return; 
        }

        block.transform.position = newPosition;
        block.transform.rotation = newRotation;
        //Debug.Log("block set to: " + newPosition + " again");

    }

    public void Undo()
    {
        if (block == null)
        {
            Debug.Log("no block found");
            return;
        }

        block.transform.position = oldPosition;
        block.transform.rotation = oldRotation;
        //Debug.Log("block reset to: " + oldPosition);

    }
}
