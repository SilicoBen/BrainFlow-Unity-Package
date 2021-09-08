using System;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using BrainFlowToolbox.Runtime.DataVisualization.DataDashboard;
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
        public BrainFlowChannelType currentDataCanvas;
        private BoardShim boardShim;
        
        private void Update()
        {
            if (boardShim == null) return;
            brainFlowSessionProfile.boardData = brainFlowSessionProfile.boardShim.get_board_data();

            if (brainFlowSessionProfile.showData)
            {
                brainFlowSessionProfile.dataDashboard.SetActive(true);
                if (currentDataCanvas == brainFlowSessionProfile.dataCanvas) return;
                BrainFlowUtilities.UpdateDataCanvas();
                currentDataCanvas = brainFlowSessionProfile.dataCanvas;
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
                BrainFlowUtilities.CreateBoardShim(brainFlowSessionProfile);

                if (!BrainFlowUtilities.StartSession(brainFlowSessionProfile))
                {
                    Debug.Log("Unable to Start BrainFlow Session");
                    return;
                };
                boardShim = brainFlowSessionProfile.boardShim;
                BrainFlowUtilities.CreateDataContainers(brainFlowSessionProfile);
                SetupDataDashBoard();
                BrainFlowUtilities.CreateDataCanvases();
                BrainFlowUtilities.UpdateDataCanvas();
                brainFlowSessionProfile.numberOfSeconds = 0;
                streaming = true;
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
                Debug.Log("BrainFlow: Unable to Start Session");
                streaming = false;
            }
        }

        private void SetupDataDashBoard()
        {
            brainFlowSessionProfile.dataDashboard = (GameObject) Instantiate(Resources.Load("Prefabs/DataDashboard"), transform);
            brainFlowSessionProfile.dataDashboard.name = "Data Dashboard";
            brainFlowSessionProfile.dataDashboard.GetComponent<BrainFlowDataDashboard>().Initialize(brainFlowSessionProfile);
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

        private void OnDisable()
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
        }
    }
}
