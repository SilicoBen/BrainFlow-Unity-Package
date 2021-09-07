using System.Collections.Generic;
using brainflow.math;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using TMPro;
using UnityEngine;
using System.Linq;
using BrainFlowToolbox.Runtime.DataVisualization;

namespace BrainFlowToolbox.Runtime.DataStreaming
{
    public class BrainFlowChannelDataStream : MonoBehaviour
    {
        public BrainFlowDataTypeManager dataManager;
        public int channelID;
        public int channelTypeID;
        public int numberOfChannels;
        private TextMeshProUGUI textOptions;
        private bool streaming;
        private RectTransform rect;
        private string channel;
        private BrainFlowChannelVisualizer streamer;
        public List<double> channelData = new List<double>();
        
        public void Initialize(BrainFlowDataTypeManager dataTypeManager, int channelIndex)
        {
            dataManager = dataTypeManager;
            channelID = channelIndex;
            dataManager.DataStreamers[channelIndex] = this;
            transform.SetParent(dataManager.dataStreamersContainer.transform);
            dataManager.ChannelData[channelID] = channelData;
            streaming = true;
        }

        private void Update()
        {
            if(!streaming) return;

            if (dataManager.sessionProfile.boardData == null) return;
            var data = dataManager.sessionProfile.boardData;
            channelData.AddRange(data.GetRow(channelID));
            

            if (channelData.Count <= dataManager.sessionProfile.numberOfDataPoints)
            {
                dataManager.ChannelData[channelID] = channelData;
                return;
            }
            
            channelData = channelData.GetRange(channelData.Count - 1 - dataManager.sessionProfile.numberOfDataPoints, dataManager.sessionProfile.numberOfDataPoints);
            dataManager.ChannelData[channelID] = channelData;
        }
        
    }
}