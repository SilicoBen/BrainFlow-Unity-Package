using System;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataVisualization;
using BrainFlowToolbox.Runtime.Enumerators;
using BrainFlowToolbox.Runtime.ScriptableObjects;
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
            brainFlowSessionProfile.currentData = brainFlowSessionProfile.boardShim.get_current_board_data(1);
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
                CreateBoardShim();
                brainFlowSessionProfile.boardShim.prepare_session();
                brainFlowSessionProfile.boardShim.start_stream(
                    brainFlowSessionProfile.bufferSize,
                    "file://" + brainFlowSessionProfile.boardDataFileName + " .csv:w");
                
                if (brainFlowSessionProfile.ChannelTypeChannelIds.Count != 0 && brainFlowSessionProfile.createDataDashboard) CreateDataDashboard();
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

        private void CreateDataDashboard()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
            dataDashboard = new GameObject("Data Dashboard");
            dataDashboard.transform.SetParent(transform);
            dataDashboard.AddComponent<BrainFlowDataDashboard>().Initialize(brainFlowSessionProfile);
        }
        
        // EndSession calls release_session and ensures that all resources correctly released
        
        
        private void CreateBoardShim()
        {
            
            
            brainFlowSessionProfile.boardShim = new BoardShim((int)brainFlowSessionProfile.board,
                brainFlowSessionProfile.brainFlowInputParams);
        }
        private void OnDestroy()
        {
            EndSession();
        }
    }
}
