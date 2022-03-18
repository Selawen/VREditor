using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockStruct : ScriptableObject
{
    Vector3 position;
    Quaternion rotation;
    SaveScript.BlockType type;

    public BlockStruct(Vector3 pos, Quaternion rot, SaveScript.BlockType t)
    {
        position = pos;
        rotation = rot;
        type = t;
    }
    public void SetStruct(Vector3 pos, Quaternion rot, SaveScript.BlockType t)
    {
        position = pos;
        rotation = rot;
        type = t;
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
