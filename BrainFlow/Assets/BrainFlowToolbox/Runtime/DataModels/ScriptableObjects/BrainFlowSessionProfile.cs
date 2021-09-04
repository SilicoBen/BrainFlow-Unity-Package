using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using BrainFlowToolbox.Runtime.DataVisualization;
using BrainFlowToolbox.Runtime.Enumerators;
using BrainFlowToolbox.Runtime.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace BrainFlowToolbox.Runtime.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BrainFlowSessionProfile", menuName = "BrainFlow/BrainFlowSessionProfile", order = 0)]
    public class BrainFlowSessionProfile : ScriptableObject
    {
        [Header("Session Info")]
        public BoardIds board;
        public string sessionName;
        public string boardDataFileName;
        

        [Header("BrainFlow Input Params")]
        public int bufferSize = 450000;
        public int serialPortNumber;
        public string deviceSerialNumber;
        public string playbackFilePath;
        public string ipAddress;
        public string ipPort;
        public string macAddress;
        public int deviceDiscoveryTimeout;

        // Created at Runtime
        public BoardShim boardShim;
        public BrainFlowInputParams brainFlowInputParams;
        public Dictionary<BrainFlowDataType, BrainFlowDataTypeManager> dataManagers;

    }
}