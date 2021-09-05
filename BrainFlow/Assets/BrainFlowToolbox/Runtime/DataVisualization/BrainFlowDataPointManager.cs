using System;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization.ChannelDataStreaming
{
    public class BrainFlowDataPointManager : MonoBehaviour
    {
        [FormerlySerializedAs("brainFlowSingleChannelDataStreamVisualizer")] public BrainFlowChannelVisualizer brainFlowChannelVisualizer;
        public RectTransform graphRect;
        public int dataId;
        private float width;
        public float dataValue;
        private Image barImage;
        private RectTransform barRect;
        private BrainFlowChannelVisualizer channelVisualizer;
        public BrainFlowDataTypeManager dataManager;
        private bool initialized;
       
        
        public void CreateBar(BrainFlowChannelVisualizer graph, int dataIndex)
        {
            channelVisualizer = graph;
            dataManager = graph.dataManager;
            brainFlowChannelVisualizer = graph;
            graphRect = graph.graphRect;
            dataId = dataIndex;
            
            barImage = gameObject.GetComponent<Image>();
            barRect = gameObject.GetComponent<RectTransform>();
            
            barRect.anchorMin = new Vector2(0, 0.5f);
            barRect.anchorMax = new Vector2(0, 0.5f);
            barRect.pivot = new Vector2(0.5f, 0.5f);
            
            initialized = true;
        }

        private void Update()
        {
            if (!initialized) return;
            if (dataManager.dataRange <  dataId || dataId > channelVisualizer.graphData.Count - 1)
            {
                barImage.enabled = false;
                return;
            }
            barImage.enabled = true;
            var xInterval = dataManager.xInterval;

            var yScaling = channelVisualizer.graphHeight / dataManager.sessionProfile.yMaxValue  ;
            dataValue = (float) brainFlowChannelVisualizer.graphData[dataId];

            
            width = (dataId+1)*xInterval;
    
            
            barRect.sizeDelta = new Vector2(xInterval*0.8f, Math.Abs(dataValue) * yScaling);
            barRect.anchoredPosition = new Vector2(width, (dataValue * yScaling)/2);
            
            
            barImage.color = dataManager.sessionProfile.graphBarColor;
            
            //labelRect.anchoredPosition = new Vector2(0, brainFlowChannelVisualizer.xLabelOffset);
        }
        
        
        
    }
}