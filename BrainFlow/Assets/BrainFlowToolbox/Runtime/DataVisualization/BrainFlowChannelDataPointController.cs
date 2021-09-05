using BrainFlowToolbox.Runtime.DataModels.Classes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization.ChannelDataStreaming
{
    public class BrainFlowChannelDataPointController : MonoBehaviour
    {
        [FormerlySerializedAs("brainFlowSingleChannelDataStreamVisualizer")] public BrainFlowChannelVisualizer brainFlowChannelVisualizer;
        public RectTransform graphRect;
        public int dataId;
        private float height;
        private float width;
        private Image barImage;
        private RectTransform barRect;
        private GameObject label;
        private RectTransform labelRect;
        private Text labelText;
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
            
            barRect.anchorMin = new Vector2(0, 0);
            barRect.anchorMax = new Vector2(0, 0);
            barRect.pivot = new Vector2(0.5f, 0.5f);

            label = new GameObject("Label");
            label.transform.SetParent(transform, false);
            labelText = label.AddComponent<Text>();
            labelText.text = dataId.ToString();
            labelText.alignment = TextAnchor.MiddleCenter;
            labelRect = label.GetComponent<RectTransform>();
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

            var xInterval = dataManager.xInterval;
            
            barImage.enabled = true;
            width = (dataId+1)*xInterval;
            height = (float) (brainFlowChannelVisualizer.graphData[dataId]) * 
                dataManager.sessionProfile.yScale / 
                channelVisualizer.graphHeight ;
            
            barRect.sizeDelta = new Vector2(xInterval*0.8f, height);
            barRect.anchoredPosition = new Vector2(width, height/2);
            
            
            barImage.color = dataManager.sessionProfile.graphBarColor;
            
            //labelRect.anchoredPosition = new Vector2(0, brainFlowChannelVisualizer.xLabelOffset);
        }
        
        
        
    }
}