using System.Collections.Generic;
using BrainFlowToolbox.Runtime.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.DataVisualization
{
    public class BrainFlowSingleChannelDataStreamer : MonoBehaviour
    {
        public BrainFlowSessionProfile brainFlowSessionProfile;
        public int channelID;
        public int channelTypeID;
        public int numberOfChannels;
        private TextMeshProUGUI textOptions;
        private bool streaming;
        private RectTransform rect;
        private string channel;
        private BrainFlowSingleChannelDataVisualizer dataStreamer;
        public List<double> channelData = new List<double>();
        
        
        public void Initialize(BrainFlowSessionProfile sessionProfile, string channelName, int index, int dataIndex)
        {
            brainFlowSessionProfile = sessionProfile;
            channel = channelName;
            channelID = index;
            channelTypeID = dataIndex;
            textOptions = gameObject.GetComponent<TextMeshProUGUI>();
            rect = gameObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.localScale = new Vector3(1, 1, 1);
            //textOptions.autoSizeTextContainer = true;
            textOptions.fontSize = 8;
            if(channel != "EEG") return;
            var newDataVisualizer = (GameObject) Instantiate(Resources.Load("Prefabs/SingleChannelDataStreamer"));
            dataStreamer = newDataVisualizer.GetComponentInChildren<BrainFlowSingleChannelDataVisualizer>();
            dataStreamer.Initialize(this);
            streaming = true;
        }

        private void Update()
        {
            if(streaming == false || channel != "EEG") return;
            
            if (brainFlowSessionProfile.currentData == null) return;
            
            channelData.Add(brainFlowSessionProfile.currentData[channelID, 0]);
            
            if (channelData.Count <= brainFlowSessionProfile.bufferSize) return;
            channelData = channelData.GetRange(1, brainFlowSessionProfile.bufferSize);

            //textOptions.text = channel + channelID + ": " + Math.Round(brainFlowSessionProfile.currentData[channelID, 0], 4);
          
            
        }
        
    }
}