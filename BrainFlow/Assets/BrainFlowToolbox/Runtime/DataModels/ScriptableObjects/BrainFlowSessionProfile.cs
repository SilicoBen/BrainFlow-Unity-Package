using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.DataModels.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BrainFlowSessionProfile", menuName = "BrainFlow/BrainFlowSessionProfile", order = 0)]
    public class BrainFlowSessionProfile : ScriptableObject
    {
        [Header("Session Info")]
        public BoardIds board;
        public string sessionName;
        public string boardDataFileName;
        public int bufferSize = 450000;
        
        [Header("BrainFlow Input Params")]
        public int serialPortNumber;
        public string deviceSerialNumber;
        public string playbackFilePath;
        public string ipAddress;
        public string ipPort;
        public string macAddress;
        public string otherInfo;
        public int deviceDiscoveryTimeout;

        [Header("Data Visualization Options")] 
        public bool createDataDashboard;
        public BrainFlowDataType displayData;
        public Color graphBackgroundColor;
        public Color graphBarColor;
        public Color graphLineColor;
        public Color graphPointColor;
        // Created at Runtime
        public GameObject dataDashboard;
        public int numberOfDataPoints;
        public float yMaxValue;
        public RectTransform dataContainer;
        public VisualizationType visualizationType;
        public float thickness;
        
        // Created @ Runtime
        public double samplingRate;
        public int numberOfSeconds;
        public BoardShim boardShim;
        public BrainFlowInputParams brainFlowInputParams;
        public Dictionary<BrainFlowDataType, BrainFlowDataTypeManager> dataManagers;
        public double[,] boardData;

    }
}