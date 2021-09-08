using System;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using BrainFlowToolbox.Runtime.Managers;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.DataModels.Classes
{
    [Serializable]
    public class BrainFlowChannelTypeData
    {
        public BrainFlowSessionProfile sessionProfile;
        public BrainFlowChannelType channelType;
        public int[] channelIds;
        public Dictionary<int, BrainFlowChannelData> ChannelData = new Dictionary<int, BrainFlowChannelData>();
        public GameObject channelDataStreamers;
        public GameObject dataCanvas;
        public RectTransform dataCanvasRect;
        public float xInterval;
        public Vector2 dataCanvasSize;
        public Vector2 channelCanvasSize;
      
    }
}