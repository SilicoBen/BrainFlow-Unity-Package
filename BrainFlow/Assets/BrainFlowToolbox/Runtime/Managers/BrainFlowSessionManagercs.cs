using System;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using BrainFlowToolbox.Runtime.DataVisualization;
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
        private BoardShim boardShim;
        
        private void Update()
        {
            if (brainFlowSessionProfile.boardShim == null) return;
            brainFlowSessionProfile.boardData = brainFlowSessionProfile.boardShim.get_board_data();
            if (brainFlowSessionProfile.showData)
            {
                brainFlowSessionProfile.dataDashboard.SetActive(true);
                if (currentDataCanvas == brainFlowSessionProfile.displayData) return;
                BrainFlowUtilities.UpdateDataCanvas(brainFlowSessionProfile);
                currentDataCanvas = brainFlowSessionProfile.displayData;
            }
            else
            {
                brainFlowSessionProfile.dataDashboard.SetActive(false);
            }
            
            
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
                brainFlowSessionProfile.dataDashboard = GameObject.Find("DataDashboard") ?? (GameObject) Instantiate(Resources.Load("Prefabs/DataDashboard"), brainFlowSessionProfile.sessionGameObject.transform);
                brainFlowSessionProfile.dataDashboard.name = "Data Dashboard";
                brainFlowSessionProfile.dataDashboard.GetComponent<BrainFlowDataDashboard>().Initialize(brainFlowSessionProfile);
                BrainFlowUtilities.CreateBoardShim(brainFlowSessionProfile);
                BrainFlowUtilities.StartSession(brainFlowSessionProfile);
                BrainFlowUtilities.UpdateDataCanvas(brainFlowSessionProfile);
                boardShim = brainFlowSessionProfile.boardShim;
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
            BoardShim.disable_board_logger();
            
            if (boardShim == null)
            {
                Debug.Log("BrainFlow: Tried to end Session, but no Session was found!");
                return;
            }
            try
            {
                if (boardShim != null)
                {
                    boardShim.stop_stream();
                    boardShim.release_session();
                }
                
                Debug.Log("BrainFlow: Session has Ended");
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
                Debug.Log("BrainFlow: Could Not Release Session");
            }
            BrainFlowUtilities.EndSession();
        }
    }
}
