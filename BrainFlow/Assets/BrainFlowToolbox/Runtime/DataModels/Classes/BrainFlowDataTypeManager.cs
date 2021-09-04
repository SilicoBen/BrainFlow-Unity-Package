using System;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using BrainFlowToolbox.Runtime.DataStreaming;
using BrainFlowToolbox.Runtime.DataVisualization.ChannelDataStreaming;
using BrainFlowToolbox.Runtime.Managers;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.DataModels.Classes
{
    [Serializable]
    public class BrainFlowDataTypeManager
    {
        public BrainFlowSessionProfile sessionProfile;
        public BrainFlowDataType dataType;
        public int[] channelIds;
        public int numberOfChannels;
        public int bufferSize;
        public Dictionary<int, List<double>> channelData = new Dictionary<int, List<double>>();
        public Dictionary<int, BrainFlowChannelDataStream> dataStreamers = new Dictionary<int, BrainFlowChannelDataStream>();
        public BoardShim boardShim;
        public GameObject dataStreamersContainer;
        public int dataRange;
        public BrainFlowDataCanvas dataCanvas;
        public BrainFlowChannelTypeDashboard channelDashboard;
        public List<BrainFlowChannelVisualizer> channelDataStreamVisualizers;
    }
}