using System;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime;
using BrainFlowToolbox.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BrainFlowToolbox.Utilities
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
            
            
            try
            {
                BoardShim.disable_board_logger();
                BoardShim.set_log_file(brainFlowSessionProfile.boardDataFileName + "_log.txt");
                BoardShim.enable_dev_board_logger();
                brainFlowSessionProfile.samplingRate = BoardShim.get_sampling_rate(brainFlowSessionProfile.boardId);
                CreateBoardShim();
                GetChannels();
                brainFlowSessionProfile.boardShim.prepare_session();
                brainFlowSessionProfile.boardShim.start_stream(
                    brainFlowSessionProfile.bufferSize,
                    "file://" + brainFlowSessionProfile.boardDataFileName + " .csv:w");
                
                if (brainFlowSessionProfile.ChannelDictionary.Count != 0 && brainFlowSessionProfile.createDataDashboard) CreateDataDashboard();
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
        public void EndSession()
        {
            
            if (brainFlowSessionProfile == null || brainFlowSessionProfile.boardShim == null)
            {
                Debug.Log("BrainFlow: Tried to end Session, but no Session was found!");
                return;
            }
            try
            {
                brainFlowSessionProfile.boardShim.stop_stream();
                brainFlowSessionProfile.boardShim.release_session();
                BoardShim.disable_board_logger();
                Debug.Log("BrainFlow: Session has Ended");
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
                Debug.Log("BrainFlow: Could Not Release Session");
            }
        }

        private void GetChannels()
        {
            brainFlowSessionProfile.ChannelDictionary = new Dictionary<string, int[]>();
            
            brainFlowSessionProfile.boardId = (int) brainFlowSessionProfile.board;
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["EEG"] =
                    BoardShim.get_eeg_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["EXG"] = 
                    BoardShim.get_exg_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["EMG"] = 
                    BoardShim.get_emg_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["ECG"] = 
                    BoardShim.get_ecg_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["EOG"] = 
                    BoardShim.get_eog_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["PPG"] = 
                    BoardShim.get_ppg_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["Accel"] = 
                    BoardShim.get_accel_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["Analog"] = 
                    BoardShim.get_analog_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["Gyro"] = 
                    BoardShim.get_gyro_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["Other"] = 
                    BoardShim.get_other_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["Temp"] = 
                    BoardShim.get_temperature_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }
            
            try
            {
                brainFlowSessionProfile.ChannelDictionary["Resist"] = BoardShim.get_resistance_channels(brainFlowSessionProfile.boardId);
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
            }

            


        }

        private void CreateBoardShim()
        {
            // In order to create a new boardShim we need the BoardID and we need a BrainFlowInputParams object
            // First we get the boardID using the select board
            brainFlowSessionProfile.boardId = (int) brainFlowSessionProfile.board;
            
            // Next we create the BrainFlowInputParams object based on the board that was chosen.
            brainFlowSessionProfile.brainFlowInputParams = new BrainFlowInputParams();
            brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                brainFlowSessionProfile.brainFlowInputParams.timeout : 
                brainFlowSessionProfile.deviceDiscoveryTimeout;
            
            // Different boards require different parameters to create the BoardShim 
            // Here we get the parameters from our ScriptableObject BrainFlowSessionProfile depending on 
            // what board was chosen. 
            switch(brainFlowSessionProfile.board)
            {
                case BoardIds.PLAYBACK_FILE_BOARD:
                    break;
                case BoardIds.STREAMING_BOARD:
                    break;
                case BoardIds.SYNTHETIC_BOARD:
                    break;
                case BoardIds.CYTON_BOARD:
                    break;
                case BoardIds.GANGLION_BOARD:
                    break;
                case BoardIds.CYTON_DAISY_BOARD:
                    break;
                case BoardIds.GALEA_BOARD:
                    break;
                case BoardIds.GANGLION_WIFI_BOARD:
                    break;
                case BoardIds.CYTON_WIFI_BOARD:
                    break;
                case BoardIds.CYTON_DAISY_WIFI_BOARD:
                    break;
                case BoardIds.BRAINBIT_BOARD:
                    break;
                case BoardIds.UNICORN_BOARD:
                    break;
                case BoardIds.CALLIBRI_EEG_BOARD:
                    break;
                case BoardIds.CALLIBRI_EMG_BOARD:
                    break;
                case BoardIds.CALLIBRI_ECG_BOARD:
                    break;
                case BoardIds.FASCIA_BOARD:
                    break;
                case BoardIds.NOTION_1_BOARD:
                    break;
                case BoardIds.NOTION_2_BOARD:
                    break;
                case BoardIds.IRONBCI_BOARD:
                    break;
                case BoardIds.GFORCE_PRO_BOARD:
                    break;
                case BoardIds.FREEEEG32_BOARD:
                    break;
                case BoardIds.BRAINBIT_BLED_BOARD:
                    break;
                case BoardIds.GFORCE_DUAL_BOARD:
                    break;
                case BoardIds.GALEA_SERIAL_BOARD:
                    break;
                case BoardIds.MUSE_S_BLED_BOARD:
                    break;
                case BoardIds.MUSE_2_BLED_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_port =
                        "COM" + brainFlowSessionProfile.serialPortNumber;
                    brainFlowSessionProfile.brainFlowInputParams.serial_number =
                        brainFlowSessionProfile.deviceSerialNumber == ""
                            ? brainFlowSessionProfile.brainFlowInputParams.serial_number
                            : brainFlowSessionProfile.deviceSerialNumber;
                    brainFlowSessionProfile.brainFlowInputParams.timeout = 
                        brainFlowSessionProfile.deviceDiscoveryTimeout; 
                    break;
                case BoardIds.CROWN_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_410_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_411_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_430_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_211_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_212_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_213_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_214_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_215_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_221_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_222_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_223_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_224_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_225_BOARD:
                    break;
                case BoardIds.ENOPHONE_BOARD:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            brainFlowSessionProfile.boardShim = new BoardShim(brainFlowSessionProfile.boardId,
                brainFlowSessionProfile.brainFlowInputParams);
        }
        
        private void OnDestroy()
        {
            EndSession();
        }
    }
}
