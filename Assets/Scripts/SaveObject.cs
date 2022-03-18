using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject:ScriptableObject
{
    public List<BlockStruct> blocks = new List<BlockStruct>();
    public List<string> blockstrings = new List<string>();

    public string ToSaveString()
    {
        foreach(BlockStruct b in blocks)
        {
            blockstrings.Add(b.ToJson());
        }

        string saveString = JsonUtility.ToJson(this);
        return saveString;
    }
}
