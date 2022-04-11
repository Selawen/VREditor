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
    public string typeString = null;
    public Vector3 scale;

    public BlockStruct()
    {

    }

    public BlockStruct(Vector3 pos, Quaternion rot, string t)
    {
        position = pos;
        rotation = rot;
        typeString = t;
    }
    public BlockStruct(Vector3 pos, Quaternion rot, string t, Vector3 s)
    {
        position = pos;
        rotation = rot;
        typeString = t;
        scale = s;
    }

    public void SetStruct(Vector3 pos, Quaternion rot, string t)
    {
        position = pos;
        rotation = rot;
        typeString = t;
    }
    public void SetStruct(Vector3 pos, Quaternion rot, string t, Vector3 s)
    {
        position = pos;
        rotation = rot;
        typeString = t;
        scale = s;
    }

    public Vector3 Position()
    {
        return position;
    }
    public Quaternion Rotation()
    {
        return rotation;
    }

    public Vector3 Scale()
    {
        return scale;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
