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
        public int barId;
        private float yPosition;
        private float xPosition;
        private Image barImage;
        private RectTransform barRect;
        private GameObject label;
        private RectTransform labelRect;
        private Text labelText;
        private BrainFlowChannelVisualizer streamVisualizer;
        public BrainFlowDataTypeManager dataManager;
        
        public void CreateBar(BrainFlowChannelVisualizer graph, int barIndex)
        {
            streamVisualizer = graph;
            dataManager = graph.dataManager;
            brainFlowChannelVisualizer = graph;
            graphRect = graph.graphRect;
            barId = barIndex;
            
            barImage = gameObject.GetComponent<Image>();
            barRect = gameObject.GetComponent<RectTransform>();
            
            barRect.anchorMin = Vector2.zero;
            barRect.anchorMax = Vector2.zero;
            barRect.pivot = new Vector2(0.5f, 0);

            label = new GameObject("Label");
            label.transform.SetParent(transform, false);
            labelText = label.AddComponent<Text>();
            labelText.text = barId.ToString();
            labelText.alignment = TextAnchor.MiddleCenter;
            labelRect = label.GetComponent<RectTransform>();
            
        }

        private void Update()
        {
            if (dataManager.dataRange <  barId || barId > streamVisualizer.graphData.Count - 1)
            {
                barImage.enabled = false;
                return;
            }

            var xInterval = streamVisualizer.xInterval;
            
            barImage.enabled = true;
            xPosition = (barId+1)*xInterval;
            yPosition = (float) (brainFlowChannelVisualizer.graphData[barId])*10 / streamVisualizer.graphHeight;
            
            barRect.sizeDelta = new Vector2(xInterval*0.8f, yPosition);
            barRect.anchoredPosition = new Vector2(xPosition, 0);
            
            
            barImage.color = dataManager.sessionProfile.graphBarColor;
            
            //labelRect.anchoredPosition = new Vector2(0, brainFlowChannelVisualizer.xLabelOffset);
        }
        
        
        
    }
}