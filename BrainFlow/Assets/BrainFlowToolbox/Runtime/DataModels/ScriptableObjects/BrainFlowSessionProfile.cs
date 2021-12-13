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
        public BrainFlowChannelType dataCanvas;
        public Color graphBackgroundColor = Color.black;
        public Color graphBarColor = Color.cyan;
        public Color graphLineColor = Color.green;
        public Color graphPointColor = Color.blue;
        // Created at Runtime
        public GameObject dataDashboard;
        public int numberOfDataPoints;
        public float dataXInterval;
        public float yMaxValue;
        public RectTransform dataContainer;
        public VisualizationType visualizationType;
        public float thickness;
        public GameObject sessionContainer;
        public GameObject dataStreamersContainer;
        public GameObject dataCanvases;
        public Dictionary<BrainFlowChannelType, GameObject> dataCanvasGameObjects = new Dictionary<BrainFlowChannelType, GameObject>();
        public Dictionary<BrainFlowChannelType, GameObject> dataStreamGameObjects = new Dictionary<BrainFlowChannelType, GameObject>();
        public bool showData;
        public double samplingRate;
        public int numberOfSeconds;
        public BoardShim boardShim;
        public BrainFlowInputParams brainFlowInputParams;
        public Dictionary<BrainFlowChannelType, BrainFlowChannelTypeData> channelTypeData = new Dictionary<BrainFlowChannelType, BrainFlowChannelTypeData>();
        public double[,] boardData;

        [Range(0,10)]
        public float dataScaler;

    }
}