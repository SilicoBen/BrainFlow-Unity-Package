using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime;
using BrainFlowToolbox.Utilities;
using UnityEngine;

namespace BrainFlowToolbox.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BrainFlowSessionProfile", menuName = "BrainFlow/BrainFlowSessionProfile", order = 0)]
    public class BrainFlowSessionProfile : ScriptableObject
    {
        [Header("Session Info")]
        public string sessionName;
        [TextArea(2, 5)] public string description = "Session Description";
        
        [Header("Board Data")]
        public BoardIds board;
        public string boardDataFileName;
        public int bufferSize = 450000;
        public int serialPortNumber;
        public string deviceSerialNumber;
        public string playbackFilePath;
        public string ipAddress;
        public string ipPort;
        public string macAddress;
        public int deviceDiscoveryTimeout;
        
        

        [Header("Data Dashboard")] 
        public bool createDataDashboard;
        
        // Runtime Data (Not shown in editor)
        public int samplingRate;
        public int numberOfDataPoints;
        public GameObject dataStreamingGameObject;
        public BoardShim boardShim;
        public double[,] currentData;
        public BrainFlowInputParams brainFlowInputParams;
        public int boardId;
        public GameObject brainFlowDashboard;
        public BrainFlowDataDashboard brainFlowSessionProfile;
        public BrainFlowSessionManager brainFlowSessionDataStreamer;
        public Dictionary<string, int[]> ChannelDictionary; 
        
        public int[] eegChannels;
        public int[] exgChannels;
        public int[] emgChannels;
        public int[] ecgChannels;
        public int[] eogChannels;
        public int[] edaChannels;
        public int[] ppgChannels;
        public int[] accelChannels;
        public int[] analogChannels;
        public int[] gyroChannels;
        public int[] otherChannels;
        public int[] temperatureChannels;
        public int[] resistanceChannels;
        
        
    }
}