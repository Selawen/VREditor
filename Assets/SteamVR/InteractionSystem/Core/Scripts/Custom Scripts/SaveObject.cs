using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// object to contain block information for saving
/// </summary>
[System.Serializable]
public class SaveObject:ScriptableObject
{
    public List<BlockStruct> blocks = new List<BlockStruct>();

    //list of blockstructs converted to json
    public List<string> blockStrings = new List<string>();

    /// <summary>
    /// convert every block to json string and put in a list
    /// </summary>
    /// <returns>json of this object</returns>
    public string ToSaveString()
    {
        foreach(BlockStruct b in blocks)
        {
            blockStrings.Add(b.ToJson());
        }

        string saveString = JsonUtility.ToJson(this);
        return saveString;
    }

    /// <summary>
    /// revert blockstrings to blockstructs
    /// </summary>
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
