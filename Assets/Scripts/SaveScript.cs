using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveScript : MonoBehaviour
{
     protected SaveObject saveObject;                        //object to encapsulate list for saving using json
    private string saveJson;                                //the json string for saving town layout

    [Serializable]
    public struct SaveObject
    {
        public List<ICommand> commands;
        public int currentCommand;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("save");
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("load");
        }
    }
}
