using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UIElements;

namespace Valve.VR.InteractionSystem
{
    public class SpawnTest : MonoBehaviour
    {
        public SteamVR_Action_Boolean spawnAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

        public GameObject spawnBlock = null;

        private Hand pointerHand = null;
        private Player player = null;

        private CommandManager commandManager = null;

        public Dropdown blockPicker = null;

        // Start is called before the first frame update
        void Start()
        {
            player = InteractionSystem.Player.instance;

            if (player == null)
            {
                Debug.LogError("<b>[SteamVR Interaction]</b> Teleport: No Player instance found in map.", this);
                Destroy(this.gameObject);
                return;
            }

            if (spawnBlock == null)
            {
                //Debug.Log(Resources.Load<GameObject>("Blocks/Cube").name);
                spawnBlock = Resources.Load<GameObject>("Blocks/Cube");
            }

            GameObject managerObj = GameObject.Find("CommandManager");
            if (managerObj!= null)
            commandManager = managerObj.GetComponent<CommandManager>();
            else Debug.Log("could not find command manager");

            //Load in block options
            if (blockPicker == null) 
            {
                GameObject blockMenu = GameObject.Find("Dropdown");
                if (blockMenu!= null)
                blockPicker = blockMenu.GetComponent<Dropdown>();
                else Debug.Log("could not find dropdown");
            }

            List<string> optionList = new List<string>();

            foreach (GameObject b in Resources.LoadAll<GameObject>("Blocks"))
            {
                optionList.Add(b.name);
                //Dropdown.OptionData item = new Dropdown.OptionData();
                //item.text = b.name;
            }
            
                blockPicker.AddOptions(optionList);
        }

        // Update is called once per frame
        void Update()
        {
            foreach (Hand hand in player.hands)
            {
                //Debug.Log(hand.name);
                if (WasSpawnButtonPressed(hand))
                {
                    SpawnBlock(hand);
                }
            }
        }

        private void SpawnBlock(Hand hand)
        {
            if (spawnBlock == null)
            {
                Debug.Log("no block selected");
                return;
            }
            
            if (commandManager != null)
            {
                //Debug.Log("execute spawn");
                commandManager.Execute(new SpawnCommand(spawnBlock, hand.transform.position + (hand.transform.forward*0.3f)));
            } else {
                Debug.Log("Could not find commandManager");
            }

            /*
            GameObject block = Instantiate(spawnBlock);

            block.transform.position = hand.transform.position + (hand.transform.forward*0.3f);
            //hand.AttachObject(block, GrabTypes.Pinch);
            
            Debug.Log("spawned block: " + block);
            */
        }

        private bool WasSpawnButtonPressed(Hand hand)
        {
            if (CanSpawn(hand))
            {
                if (hand.noSteamVRFallbackCamera != null)
                {
                    return Input.GetKeyDown(KeyCode.M);
                }
                else
                {
                    //Debug.Log("pressed: " + spawnAction.GetStateDown(hand.handType));
                    return spawnAction.GetStateDown(hand.handType);

                    //return hand.controller.GetPressDown( SteamVR_Controller.ButtonMask.Touchpad );
                }
            }

            return false;
        }

        public bool CanSpawn(Hand hand)
        {
            if (hand == null)
            {
                return false;
            }

            if (!hand.gameObject.activeInHierarchy)
            {
                return false;
            }

            if (hand.hoveringInteractable != null)
            {
                return false;
            }

            if (hand.noSteamVRFallbackCamera == null)
            {
                if (hand.isActive == false)
                {
                    return false;
                }

                //Something is attached to the hand
                if (hand.currentAttachedObject != null)
                {
                    return false;
                }
            }

            return true;
        }
    }


}