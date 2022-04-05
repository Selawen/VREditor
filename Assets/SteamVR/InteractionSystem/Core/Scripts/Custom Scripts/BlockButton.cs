using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockButton : MonoBehaviour
{
    private GameObject block;
    private Valve.VR.InteractionSystem.SpawnTest spawn;

    public BlockButton(GameObject _block)
    {
        block = _block;
        spawn = FindObjectOfType<Valve.VR.InteractionSystem.SpawnTest>();
    }
    /// <summary>
    /// set block to reference to
    /// </summary>
    /// <param name="_block">the block to reference</param>
    public void SetBlock(GameObject _block)
    {
        block = _block;
        spawn = FindObjectOfType<Valve.VR.InteractionSystem.SpawnTest>();
    }

    /// <summary>
    /// set referenced block as spawnblock
    /// </summary>
    public void PickBlock()
    {
        if (spawn == null)
        {
            Debug.Log("no spawner found");
            return;
        }
        spawn.spawnBlock = block;
    }
}
