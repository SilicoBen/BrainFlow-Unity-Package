using System;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using BrainFlowToolbox.Runtime.DataStreaming;
using BrainFlowToolbox.Runtime.DataVisualization.ChannelDataStreaming;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.Utilities
{
    public static class BrainFlowUtilities
    {
        public static int[] GetChannelIds(BrainFlowDataType dataType, BoardIds board)
        {
            try
            {
                return dataType switch
                {
                    BrainFlowDataType.EEG => BoardShim.get_eeg_channels((int) board),
                    BrainFlowDataType.EXG => BoardShim.get_exg_channels((int) board),
                    BrainFlowDataType.EMG => BoardShim.get_emg_channels((int) board),
                    BrainFlowDataType.ECG => BoardShim.get_ecg_channels((int) board),
                    BrainFlowDataType.EOG => BoardShim.get_eog_channels((int) board),
                    BrainFlowDataType.EDA => BoardShim.get_eda_channels((int) board),
                    BrainFlowDataType.PPG => BoardShim.get_ppg_channels((int) board),
                    BrainFlowDataType.Accel => BoardShim.get_accel_channels((int) board),
                    BrainFlowDataType.Analog => BoardShim.get_analog_channels((int) board),
                    BrainFlowDataType.Gyro => BoardShim.get_gyro_channels((int) board),
                    BrainFlowDataType.Temperature => BoardShim.get_temperature_channels((int) board),
                    BrainFlowDataType.Resistance => BoardShim.get_resistance_channels((int) board),
                    BrainFlowDataType.Other => BoardShim.get_other_channels((int) board),
                    _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
                };
            }
            catch(BrainFlowException e)
            {
                Debug.Log(e);
                return new int[0];
            }
        }
        public static BoardShim CreateBoardShim(BrainFlowSessionProfile brainFlowSessionProfile)
        {
            // In order to create a new boardShim we need the BoardID and we need a BrainFlowInputParams object
            // First we get the boardID using the select board
            
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

            brainFlowSessionProfile.boardShim = new BoardShim((int)brainFlowSessionProfile.board,
                brainFlowSessionProfile.brainFlowInputParams);
            
            return brainFlowSessionProfile.boardShim;
        }
        public static void StartSession(BrainFlowSessionProfile brainFlowSessionProfile)
        {
            if (brainFlowSessionProfile.boardShim == null)
            {
                Debug.Log(
                    "BrainFlow: Cannot Start Session - " +
                    "No Board Shim Found. Use BrainFlowUtilities.CreateBoardShim() " +
                    "to create the BoardShim before calling Start Session");
                return;
            }
            
            try
            {
                BoardShim.disable_board_logger();
                BoardShim.set_log_file(brainFlowSessionProfile.boardDataFileName + "_log.txt");
                BoardShim.enable_dev_board_logger();
                brainFlowSessionProfile.boardShim.prepare_session();
                brainFlowSessionProfile.boardShim.start_stream(
                    brainFlowSessionProfile.bufferSize,
                    "file://" + brainFlowSessionProfile.boardDataFileName + " .csv:w");
                CreateDataTypeManagers(brainFlowSessionProfile);
                
                Debug.Log("BrainFlow: Session Started Successfully!");
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
                Debug.Log("BrainFlow: Unable to Start Session");
            }
        }
        public static Dictionary<BrainFlowDataType, BrainFlowDataTypeManager> CreateDataTypeManagers(BrainFlowSessionProfile brainFlowSessionProfile)
        {
            brainFlowSessionProfile.dataManagers = new Dictionary<BrainFlowDataType, BrainFlowDataTypeManager>();

            foreach (BrainFlowDataType i in Enum.GetValues(typeof(BrainFlowDataType)))
            {
                var channels = GetChannelIds(i, brainFlowSessionProfile.board);
                if (channels.Length == 0) continue;

                brainFlowSessionProfile.dataManagers[i] = new BrainFlowDataTypeManager
                {
                    dataType = i,
                    channelIds = channels,
                    bufferSize = brainFlowSessionProfile.bufferSize,
                    boardShim = brainFlowSessionProfile.boardShim,
                    numberOfChannels = channels.Length,
                    sessionProfile = brainFlowSessionProfile
                };
                
                CreateChannelStreamers(brainFlowSessionProfile.dataManagers[i]);
                CreateDataCanvas(brainFlowSessionProfile.dataManagers[i]);
            }
            
            return brainFlowSessionProfile.dataManagers;
        }
        public static void CreateChannelStreamers(BrainFlowDataTypeManager dataManager)
        {
            if (!dataManager.dataStreamersContainer)
            {
                dataManager.dataStreamersContainer =
                    new GameObject(dataManager.dataType + " Data Streamers");
            }
            
            foreach (var i in dataManager.channelIds)
            {
                var newDataStreamer =  new GameObject(dataManager.dataType + " CH" + i);
                var streamComponent = newDataStreamer.AddComponent<BrainFlowChannelDataStream>();
                streamComponent.Initialize(dataManager, i);
            }
        }
        public static void CreateDataCanvas(BrainFlowDataTypeManager dataManager)
        {
            if (!dataManager.dataCanvas)
            {
                var newDataCanvas = new GameObject(dataManager.dataType + " Data Canvas");
                dataManager.dataCanvas = newDataCanvas.AddComponent<BrainFlowDataCanvas>();
                dataManager.dataCanvas.Initialize(dataManager);
            }
            
            foreach (var i in dataManager.channelIds)
            {
                var newDataVisualizer =  new GameObject(dataManager.dataType + " CH" + i + " Visualizer");
                var streamComponent = newDataVisualizer.AddComponent<BrainFlowChannelDataStream>();
                streamComponent.Initialize(dataManager, i);
                dataManager.dataCanvas.AddDataStreamVisualizer(newDataVisualizer);
            }
        }
        public static void EndSession(BrainFlowSessionProfile brainFlowSessionProfile)
        {
            
            BoardShim.disable_board_logger();
            
            if (brainFlowSessionProfile == null || brainFlowSessionProfile.boardShim == null)
            {
                Debug.Log("BrainFlow: Tried to end Session, but no Session was found!");
                return;
            }
            try
            {
                brainFlowSessionProfile.boardShim.stop_stream();
                brainFlowSessionProfile.boardShim.release_session();
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