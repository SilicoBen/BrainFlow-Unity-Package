using System;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using BrainFlowToolbox.Runtime.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrainFlowToolbox.Runtime.Managers
{
    public class BrainFlowSessionManager : MonoBehaviour
    {
        public  BrainFlowSessionProfile brainFlowSessionProfile;
        public bool streaming;
        public GameObject dataDashboard;
        public GameObject eventSystem;
        public BrainFlowDataType currentDataCanvas;
        
        private void Update()
        {
            if (brainFlowSessionProfile.boardShim == null) return;
            if (currentDataCanvas == brainFlowSessionProfile.displayData) return;
            BrainFlowUtilities.UpdateDataCanvas(brainFlowSessionProfile);
            currentDataCanvas = brainFlowSessionProfile.displayData;

        }
    
        public void StartSession(BrainFlowSessionProfile sessionProfile)
        {
            if (GameObject.Find("BrainFlow Session: " + sessionProfile.name))
            {
                Destroy(GameObject.Find("BrainFlow Session: " + sessionProfile.name));
            }
            
            gameObject.name = "BrainFlow Session: " + sessionProfile.name;
            brainFlowSessionProfile = sessionProfile;
            
            if (FindObjectOfType<EventSystem>() == null)
            {
                eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
            
            
            try
            {
                BoardShim.disable_board_logger();
                BoardShim.set_log_file(brainFlowSessionProfile.boardDataFileName + "_log.txt");
                BoardShim.enable_dev_board_logger();
                BrainFlowUtilities.CreateBoardShim(brainFlowSessionProfile);
                BrainFlowUtilities.StartSession(brainFlowSessionProfile);
                BrainFlowUtilities.UpdateDataCanvas(brainFlowSessionProfile);
                streaming = true;
               
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
                Debug.Log("BrainFlow: Unable to Start Session");
                streaming = false;
            }
        }
        
        private void OnDestroy()
        {
            BrainFlowUtilities.EndSession();
        }
    }
}
