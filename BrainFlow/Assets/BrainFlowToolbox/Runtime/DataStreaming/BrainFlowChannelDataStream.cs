using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using BrainFlowToolbox.Runtime.DataVisualization.ChannelDataStreaming;
using TMPro;
using UnityEngine;

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
            dataManager.dataStreamers[channelIndex] = this;
            transform.SetParent(dataManager.dataStreamersContainer.transform);
            streaming = true;
        }

        private void Update()
        {
            if(streaming == false) return;
            
            channelData.Add(dataManager.boardShim.get_current_board_data(1)[channelID, 0]);
            
            if (channelData.Count <= dataManager.bufferSize) return;
            
            channelData = channelData.GetRange(1, dataManager.bufferSize);
            dataManager.channelData[channelID] = channelData;
        }
        
    }
}