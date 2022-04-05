using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCommand : MonoBehaviour, ICommand
{
    GameObject block;
    Vector3 oldScale, newScale;

    public ScaleCommand(Vector3 scale, float scaleMultiplier, GameObject scaledObject)
    {
        block = scaledObject;
        oldScale = scale;
        newScale = scale*scaleMultiplier;
    }

    public void Execute()
    {
        block.transform.localScale = newScale;
    }

    public void Redo()
    {
        if (block != null)
        {
            block.transform.localScale = newScale;
        }
    }

    public void Undo()
    {
        if (block != null)
        {
            block.transform.localScale = oldScale;
        }
    }
}
