using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// object to store block information
/// </summary>
[System.Serializable]
public class BlockStruct : ScriptableObject
{
    public Vector3 position;
    public Quaternion rotation;
    public SaveScript.BlockType type;
    public string typeString = null;

    public BlockStruct(Vector3 pos, Quaternion rot, SaveScript.BlockType t)
    {
        position = pos;
        rotation = rot;
        type = t;
    }
    public BlockStruct(Vector3 pos, Quaternion rot, string t)
    {
        position = pos;
        rotation = rot;
        typeString = t;
    }
    public void SetStruct(Vector3 pos, Quaternion rot, SaveScript.BlockType t)
    {
        position = pos;
        rotation = rot;
        type = t;
    }
    public void SetStruct(Vector3 pos, Quaternion rot, string t)
    {
        position = pos;
        rotation = rot;
        typeString = t;
    }

    public Vector3 Position()
    {
        return position;
    }
    public Quaternion Rotation()
    {
        return rotation;
    }

    public SaveScript.BlockType Type()
    {
        return type;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
