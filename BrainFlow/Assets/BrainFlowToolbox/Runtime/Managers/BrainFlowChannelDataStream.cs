using System.Collections.Generic;
using brainflow.math;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using TMPro;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.Managers
{
    public class BrainFlowChannelDataStream : MonoBehaviour
    {
        public BrainFlowChannelTypeData channelTypeData;
        public BrainFlowChannelData channelData;
        public int channelID;
        public int channelTypeID;
        public int numberOfChannels;
        private TextMeshProUGUI textOptions;
        private bool streaming;
        private RectTransform rect;
        private string channel;
        private BrainFlowChannelDataCanvas streamer;
        private List<double> dataBuffer = new List<double>();

        public void Initialize(BrainFlowChannelData data)
        {
            channelData = data;
            channelTypeData = data.channelTypeData;
            channelID = channelData.channelBoardID;
            transform.SetParent(channelTypeData.channelDataStreamers.transform);
            streaming = true;
        }

        private void Update()
        {
            if(!streaming || channelData.sessionProfile.boardData == null) return;

            
            // First we update the data buffer
            var data = channelData.sessionProfile.boardData;
            dataBuffer.AddRange(data.GetRow(channelID));
            
            if (dataBuffer.Count > channelData.sessionProfile.bufferSize)
            {
                dataBuffer = dataBuffer.GetRange(
                    dataBuffer.Count - 1 - channelData.sessionProfile.bufferSize, 
                    channelData.sessionProfile.bufferSize);
            }

            // next we update the channel data

            if (dataBuffer.Count <= channelData.sessionProfile.numberOfDataPoints)
            {
                channelData.channelData = dataBuffer;
            }
            else
            {
                channelData.channelData = dataBuffer.GetRange(
                    dataBuffer.Count - 1 - channelData.sessionProfile.numberOfDataPoints, 
                    channelData.sessionProfile.numberOfDataPoints);
            }

            
        }
        
    }
}