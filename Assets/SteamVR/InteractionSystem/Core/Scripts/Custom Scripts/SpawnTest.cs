using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
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

        [SerializeField] private Dropdown blockPicker = null;

        //a bool for disabling resetting of manual block type change
        public bool pickerTest;

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

            //set block to default to cube
            if (spawnBlock == null)
            {
                spawnBlock = Resources.Load<GameObject>("Blocks/Cube");
            }

            //get commandmanager to send spawn command to
            GameObject managerObj = GameObject.Find("CommandManager");
            if (managerObj!= null)
            commandManager = managerObj.GetComponent<CommandManager>();
            else Debug.Log("could not find command manager");


            //-------------------------------------------------
            // Loads block options to unity dropdown
            //-------------------------------------------------
            /*
            //Load in block options for dropdown
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
                blockPicker.ClearOptions();
                blockPicker.AddOptions(optionList);
            */
        }

        // Update is called once per frame
        void Update()
        {
            //spawn block if spawnbutton was pressed on any controller or fallback
            foreach (Hand hand in player.hands)
            {
                if (WasSpawnButtonPressed(hand))
                {
                    SpawnBlock(hand);
                }
            }
        }

        /// <summary>
        /// spawn the selected block in front of the hand that triggered the spawn button
        /// </summary>
        /// <param name="hand">the hand where the promt originated</param>
        private void SpawnBlock(Hand hand)
        {
            //if not in VR, use fallback
            if (hand.noSteamVRFallbackCamera != null)
            {
                //a bool for disabling resetting of manual block type change
                if (!pickerTest)
                {
                    GameObject tempBlock = Resources.Load<GameObject>("Blocks/" + blockPicker.itemText);
                    if (tempBlock != null)
                        spawnBlock = tempBlock;
                    else
                        spawnBlock = Resources.Load<GameObject>("Blocks/Cube");
                } else
                {
                    if (spawnBlock == null) spawnBlock = Resources.Load<GameObject>("Blocks/Cube");
                }
            }

            if (spawnBlock == null)
            {
                Debug.Log("no block selected");
                return;
            }
            
            //send spawn command to command manager
            if (commandManager != null)
            {
                commandManager.Execute(new SpawnCommand(spawnBlock, hand.transform.position + (hand.transform.forward*0.3f)));
            } else {
                Debug.Log("Could not find commandManager");
            }


            //-------------------------------------------------
            // immediately grab the spawned block
            //-------------------------------------------------
            /*
            GameObject block = Instantiate(spawnBlock);

            block.transform.position = hand.transform.position + (hand.transform.forward*0.3f);
            //hand.AttachObject(block, GrabTypes.Pinch);
            
            Debug.Log("spawned block: " + block);
            */
        }

        /// <summary>
        /// Check whether spawn is valid and spawn button was pressed
        /// </summary>
        /// <param name="hand">hand to check input from</param>
        /// <returns>returns whether block should be spawned</returns>
        private bool WasSpawnButtonPressed(Hand hand)
        {
            if (CanSpawn(hand))
            {
                //get input from fallback if active, otherwise get trigger input 
                if (hand.noSteamVRFallbackCamera != null)
                {
                    return Input.GetKeyDown(KeyCode.M);
                }
                else
                {
                    return spawnAction.GetStateDown(hand.handType);
                }
            }

            return false;
        }

        /// <summary>
        /// checks whether there's anything to prevent block spawning
        /// </summary>
        /// <param name="hand">hand to check</param>
        /// <returns>returns whether spawning is valid</returns>
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

            //don't spawn blocks when other block is already in focus
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

                //don't spawn blocks when already holding a block
                if (hand.currentAttachedObject != null)
                {
                    return false;
                }
            }

            return true;
        }
    }


}