using System;
using System.Collections.Generic;
using System.Linq;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using BrainFlowToolbox.Runtime.Managers;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.Utilities
{
    public static class BrainFlowUtilities
    {
        private static BrainFlowSessionProfile sessionProfile;
        private static BoardShim boardShim;
        private static GameObject activeDataCanvas;
        private static GameObject activeDataStreamer;
        private static GameObject dataDashboard;
        private static BrainFlowChannelType currentCanvas;

        // Session Initialization functions
        public static void CreateBoardShim(BrainFlowSessionProfile brainFlowSessionProfile)
        {
            // In order to create a new boardShim we need the BoardID and we need a BrainFlowInputParams object
            // First we get the boardID using the select board
            sessionProfile = brainFlowSessionProfile;
            
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
                    brainFlowSessionProfile.brainFlowInputParams.other_info = brainFlowSessionProfile.otherInfo;
                    brainFlowSessionProfile.brainFlowInputParams.file = brainFlowSessionProfile.playbackFilePath;
                    break;
                case BoardIds.STREAMING_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.ip_address = brainFlowSessionProfile.ipAddress;
                    try
                    {
                        brainFlowSessionProfile.brainFlowInputParams.ip_port = Int32.Parse(brainFlowSessionProfile.ipPort);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log("Cannot parse string ipPort into an int");
                    }
                    brainFlowSessionProfile.brainFlowInputParams.other_info = brainFlowSessionProfile.otherInfo;
                    break;
                case BoardIds.SYNTHETIC_BOARD:
                    break;
                case BoardIds.CYTON_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_port =
                        "COM" + brainFlowSessionProfile.serialPortNumber;
                    break;
                case BoardIds.GANGLION_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_port =
                        "COM" + brainFlowSessionProfile.serialPortNumber;
                    brainFlowSessionProfile.brainFlowInputParams.mac_address = brainFlowSessionProfile.macAddress == "" ? 
                        brainFlowSessionProfile.brainFlowInputParams.mac_address : brainFlowSessionProfile.macAddress;
                    brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                        15 : brainFlowSessionProfile.deviceDiscoveryTimeout;
                    break;
                case BoardIds.CYTON_DAISY_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_port =
                        "COM" + brainFlowSessionProfile.serialPortNumber;
                    break;
                case BoardIds.GALEA_BOARD:
                    break;
                case BoardIds.GANGLION_WIFI_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.ip_address = brainFlowSessionProfile.ipAddress == "" ? 
                        "192.168.4.1" : brainFlowSessionProfile.ipAddress;
                    try
                    {
                        brainFlowSessionProfile.brainFlowInputParams.ip_port = Int32.Parse(brainFlowSessionProfile.ipPort);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log("Cannot parse string ipPort into an int");
                    }
                    brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                        10 : brainFlowSessionProfile.deviceDiscoveryTimeout;
                    break;
                case BoardIds.CYTON_WIFI_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.ip_address = brainFlowSessionProfile.ipAddress == "" ? 
                        "192.168.4.1" : brainFlowSessionProfile.ipAddress;
                    try
                    {
                        brainFlowSessionProfile.brainFlowInputParams.ip_port = Int32.Parse(brainFlowSessionProfile.ipPort);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log("Cannot parse string ipPort into an int");
                    }
                    brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                        10 : brainFlowSessionProfile.deviceDiscoveryTimeout;
                    break;
                case BoardIds.CYTON_DAISY_WIFI_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.ip_address = brainFlowSessionProfile.ipAddress == "" ? 
                        "192.168.4.1" : brainFlowSessionProfile.ipAddress;
                    try
                    {
                        brainFlowSessionProfile.brainFlowInputParams.ip_port = Int32.Parse(brainFlowSessionProfile.ipPort);
                    }
                    catch (FormatException e)
                    {
                        Debug.Log("Cannot parse string ipPort into an int");
                    }
                    brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                        10 : brainFlowSessionProfile.deviceDiscoveryTimeout;
                    break;
                case BoardIds.BRAINBIT_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                        15 : brainFlowSessionProfile.deviceDiscoveryTimeout;
                    brainFlowSessionProfile.brainFlowInputParams.serial_number =
                        brainFlowSessionProfile.deviceSerialNumber == ""
                            ? brainFlowSessionProfile.brainFlowInputParams.serial_number
                            : brainFlowSessionProfile.deviceSerialNumber;
                    break;
                case BoardIds.UNICORN_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_number =
                        brainFlowSessionProfile.deviceSerialNumber == ""
                            ? brainFlowSessionProfile.brainFlowInputParams.serial_number
                            : brainFlowSessionProfile.deviceSerialNumber;
                    break;
                case BoardIds.CALLIBRI_EEG_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.other_info = brainFlowSessionProfile.otherInfo == "" ? 
                        brainFlowSessionProfile.brainFlowInputParams.other_info : brainFlowSessionProfile.otherInfo;
                    brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                        15 : brainFlowSessionProfile.deviceDiscoveryTimeout;
                    break;
                case BoardIds.CALLIBRI_EMG_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.other_info = brainFlowSessionProfile.otherInfo == "" ? 
                        brainFlowSessionProfile.brainFlowInputParams.other_info : brainFlowSessionProfile.otherInfo;
                    brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                        15 : brainFlowSessionProfile.deviceDiscoveryTimeout;
                    break;
                case BoardIds.CALLIBRI_ECG_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.other_info = brainFlowSessionProfile.otherInfo == "" ? 
                        brainFlowSessionProfile.brainFlowInputParams.other_info : brainFlowSessionProfile.otherInfo;
                    brainFlowSessionProfile.brainFlowInputParams.timeout = brainFlowSessionProfile.deviceDiscoveryTimeout == 0 ? 
                        15 : brainFlowSessionProfile.deviceDiscoveryTimeout;
                    break;
                case BoardIds.FASCIA_BOARD:
                    break;
                case BoardIds.NOTION_1_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_number =
                        brainFlowSessionProfile.deviceSerialNumber == ""
                            ? brainFlowSessionProfile.brainFlowInputParams.serial_number
                            : brainFlowSessionProfile.deviceSerialNumber;
                    break;
                case BoardIds.NOTION_2_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_number =
                        brainFlowSessionProfile.deviceSerialNumber == ""
                            ? brainFlowSessionProfile.brainFlowInputParams.serial_number
                            : brainFlowSessionProfile.deviceSerialNumber;
                    break;
                case BoardIds.IRONBCI_BOARD:
                    break;
                case BoardIds.GFORCE_PRO_BOARD:
                    break;
                case BoardIds.FREEEEG32_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_port =
                        "COM" + brainFlowSessionProfile.serialPortNumber;
                    break;
                case BoardIds.BRAINBIT_BLED_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_port =
                        "COM" + brainFlowSessionProfile.serialPortNumber;
                    brainFlowSessionProfile.brainFlowInputParams.mac_address = brainFlowSessionProfile.macAddress == "" ? 
                        brainFlowSessionProfile.brainFlowInputParams.mac_address : brainFlowSessionProfile.macAddress;
                    break;
                case BoardIds.GFORCE_DUAL_BOARD:
                    break;
                case BoardIds.GALEA_SERIAL_BOARD:
                    break;
                case BoardIds.MUSE_S_BLED_BOARD:
                    brainFlowSessionProfile.brainFlowInputParams.serial_port =
                        "COM" + brainFlowSessionProfile.serialPortNumber;
                    brainFlowSessionProfile.brainFlowInputParams.serial_number =
                        brainFlowSessionProfile.deviceSerialNumber == ""
                            ? brainFlowSessionProfile.brainFlowInputParams.serial_number
                            : brainFlowSessionProfile.deviceSerialNumber;
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
                    brainFlowSessionProfile.brainFlowInputParams.serial_number =
                        brainFlowSessionProfile.deviceSerialNumber == ""
                            ? brainFlowSessionProfile.brainFlowInputParams.serial_number
                            : brainFlowSessionProfile.deviceSerialNumber;
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
                    brainFlowSessionProfile.brainFlowInputParams.mac_address = brainFlowSessionProfile.macAddress == "" ? 
                        brainFlowSessionProfile.brainFlowInputParams.mac_address : brainFlowSessionProfile.macAddress;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            brainFlowSessionProfile.boardShim = new BoardShim((int)brainFlowSessionProfile.board,
                brainFlowSessionProfile.brainFlowInputParams);
            
            brainFlowSessionProfile.samplingRate = BoardShim.get_sampling_rate((int)brainFlowSessionProfile.board);
        }
        public static bool StartSession(BrainFlowSessionProfile brainFlowSessionProfile)
        {
            if (brainFlowSessionProfile.boardShim == null) CreateBoardShim(brainFlowSessionProfile);
            
            try
            {
                BoardShim.disable_board_logger();
                BoardShim.set_log_file(brainFlowSessionProfile.boardDataFileName + "_log.txt");
                BoardShim.enable_dev_board_logger();
                brainFlowSessionProfile.boardShim?.prepare_session();
                brainFlowSessionProfile.boardShim?.start_stream(
                    brainFlowSessionProfile.bufferSize,
                    "file://" + brainFlowSessionProfile.boardDataFileName + " .csv:w");
                boardShim = brainFlowSessionProfile.boardShim;
                Debug.Log("BrainFlow: Session Started Successfully!");
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
                Debug.Log("BrainFlow: Unable to Start Session");
                return false;
            }
            
            return true;
        }
        public static void EndSession()
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
        
        // Methods for organizing data streams
        public static void CreateDataContainers(BrainFlowSessionProfile brainFlowSessionProfile)
        {
            brainFlowSessionProfile.channelTypeData = new Dictionary<BrainFlowChannelType, BrainFlowChannelTypeData>();

            if (!brainFlowSessionProfile.dataStreamersContainer)
            {
                brainFlowSessionProfile.dataStreamersContainer = new GameObject("Data Streams");
            }
            
            brainFlowSessionProfile.dataStreamersContainer.transform.SetParent(brainFlowSessionProfile.sessionContainer.transform);
            
            foreach (BrainFlowChannelType i in Enum.GetValues(typeof(BrainFlowChannelType)))
            {
                // Here we create a data data for each channel type
                var channels = GetChannelIds(i, brainFlowSessionProfile.board);
                if (channels.Length == 0) continue;
                
                brainFlowSessionProfile.channelTypeData[i] = new BrainFlowChannelTypeData
                {
                    channelType = i,
                    channelIds = channels,
                    sessionProfile = brainFlowSessionProfile,
                };
                
                // once the type data is created we create a separate data for each channel.
                CreateChannelDataStreamers(brainFlowSessionProfile.channelTypeData[i]);
            }
        }
        public static void CreateChannelDataStreamers(BrainFlowChannelTypeData channelTypeData)
        {
            
            sessionProfile.dataStreamGameObjects[channelTypeData.channelType] = new GameObject(channelTypeData.channelType + " Data Streams");

            sessionProfile.dataStreamGameObjects[channelTypeData.channelType].transform
                .SetParent(sessionProfile.dataStreamersContainer.transform);

            channelTypeData.channelDataStreamers = sessionProfile.dataStreamGameObjects[channelTypeData.channelType];
            channelTypeData.channelDataStreamers.SetActive(false);
            
            var index = 0;
            
            foreach (var c in channelTypeData.channelIds)
            {
                var dataStreamer = new GameObject("Channel " + c + " Data Streamer");
                
                channelTypeData.ChannelData[c] = new BrainFlowChannelData()
                {
                    sessionProfile = channelTypeData.sessionProfile,
                    channelType = channelTypeData.channelType,
                    channelTypeData = channelTypeData,
                    channelBoardID = c,
                    channelTypeIndex = index,
                    dataStreamer = dataStreamer
                };
                
                channelTypeData.ChannelData[c].dataStreamer.AddComponent<BrainFlowChannelDataStream>().Initialize(channelTypeData.ChannelData[c]);
                channelTypeData.ChannelData[c].dataStreamer.transform.SetParent(
                    sessionProfile.dataStreamGameObjects[channelTypeData.channelType].transform);
                index++;
            }
            
            
        }
        
        // Methods for creating Data Visualization
        public static void CreateDataCanvases()
        {
            sessionProfile.dataCanvases = new GameObject("Data Canvases");
            sessionProfile.dataCanvases.transform.SetParent(sessionProfile.sessionContainer.transform);
            
            foreach (var channelData in sessionProfile.channelTypeData.Select(channelTypeData => channelTypeData.Value))
            {
                channelData.dataCanvas = new GameObject(channelData.channelType + " Data Canvas");
                channelData.dataCanvas.transform.SetParent(channelData.sessionProfile.dataCanvases.transform);
                channelData.sessionProfile.dataCanvasGameObjects[channelData.channelType] = channelData.dataCanvas;
                channelData.dataCanvas.AddComponent<BrainFlowChannelTypeDataCanvas>().Initialize(channelData);
                channelData.dataCanvas.SetActive(false);
            }
        }
        
        // Runtime Update methods
        public static void UpdateDataCanvas()
        {
            if(!sessionProfile.dataCanvasGameObjects.ContainsKey(sessionProfile.dataCanvas))
            {
                Debug.Log("Brain Flow: Session board does not have " + sessionProfile.dataCanvas);
                sessionProfile.dataCanvas = currentCanvas;
                return;
            }
            
            if(activeDataCanvas != null) activeDataCanvas.SetActive(false);
            if(activeDataStreamer != null) activeDataStreamer.SetActive(false);
            activeDataCanvas = sessionProfile.dataCanvasGameObjects[sessionProfile.dataCanvas];
            activeDataStreamer = sessionProfile.dataStreamGameObjects[sessionProfile.dataCanvas];
            activeDataCanvas.SetActive(true);
            activeDataStreamer.SetActive(true);
        }
        
        // Helper methods
        public static int[] GetChannelIds(BrainFlowChannelType channelType, BoardIds board)
        {
            try
            {
                return channelType switch
                {
                    BrainFlowChannelType.EEG => BoardShim.get_eeg_channels((int) board),
                    BrainFlowChannelType.EXG => BoardShim.get_exg_channels((int) board),
                    BrainFlowChannelType.EMG => BoardShim.get_emg_channels((int) board),
                    BrainFlowChannelType.ECG => BoardShim.get_ecg_channels((int) board),
                    BrainFlowChannelType.EOG => BoardShim.get_eog_channels((int) board),
                    BrainFlowChannelType.EDA => BoardShim.get_eda_channels((int) board),
                    BrainFlowChannelType.PPG => BoardShim.get_ppg_channels((int) board),
                    BrainFlowChannelType.Accel => BoardShim.get_accel_channels((int) board),
                    BrainFlowChannelType.Analog => BoardShim.get_analog_channels((int) board),
                    BrainFlowChannelType.Gyro => BoardShim.get_gyro_channels((int) board),
                    BrainFlowChannelType.Temperature => BoardShim.get_temperature_channels((int) board),
                    BrainFlowChannelType.Resistance => BoardShim.get_resistance_channels((int) board),
                    BrainFlowChannelType.Other => BoardShim.get_other_channels((int) board),
                    _ => throw new ArgumentOutOfRangeException(nameof(channelType), channelType, null)
                };
            }
            catch(BrainFlowException e)
            {
                //Debug.Log(e);
                return new int[0];
            }
        }
    }
}