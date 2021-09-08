using System;
using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using BrainFlowToolbox.Runtime.Managers;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.DataModels.Classes
{
    [Serializable]
    public class BrainFlowChannelData
    {
        public BrainFlowChannelType channelType;
        public BrainFlowSessionProfile sessionProfile;
        public BrainFlowChannelTypeData channelTypeData;
        public int channelBoardID;
        public int channelTypeIndex;
        public List<double> channelData = new List<double>();
        public float yAxisScaler;
        public GameObject dataStreamer;
        public BrainFlowChannelTypeDataCanvas channelTypeDataCanvas;
        public float xInterval;
    }
}