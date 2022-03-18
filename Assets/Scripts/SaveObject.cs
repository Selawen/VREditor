using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject:ScriptableObject
{
    //[System.NonSerialized]
    public List<BlockStruct> blocks = new List<BlockStruct>();

    public List<string> blockStrings = new List<string>();

    public string ToSaveString()
    {
        foreach(BlockStruct b in blocks)
        {
            blockStrings.Add(b.ToJson());
        }

        string saveString = JsonUtility.ToJson(this);
        return saveString;
    }

    public void ReadyLoad()
    {
        foreach(string s in blockStrings)
        {
            BlockStruct bs = new BlockStruct(Vector3.zero, new Quaternion(), SaveScript.BlockType.Cube);
            JsonUtility.FromJsonOverwrite(s, bs);
            blocks.Add(bs);
        }
    }
}
