using System;
using System.Collections.Generic;
using brainflow;
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
        
        private void Update()
        {
            if (brainFlowSessionProfile.boardShim == null) return;
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
                
               streaming = true;
                
                Debug.Log("BrainFlow: Session Started Successfully!");
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
                Debug.Log("BrainFlow: Unable to Start Session");
                streaming = false;
            }
        }


        
        // EndSession calls release_session and ensures that all resources correctly released
        
        

        private void OnDestroy()
        {
            BrainFlowUtilities.EndSession(brainFlowSessionProfile);
        }
    }
}
