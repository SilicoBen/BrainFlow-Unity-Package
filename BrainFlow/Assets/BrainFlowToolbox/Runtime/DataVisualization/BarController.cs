using System;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization
{
    public class BarController : MonoBehaviour
    {
        public BrainFlowSingleChannelDataVisualizer brainFlowSingleChannelDataVisualizer;
        public RectTransform graphRect;
        public int barId;
        private float yPosition;
        private float xPosition;
        private Image barImage;
        private RectTransform barRect;
        private GameObject label;
        private RectTransform labelRect;
        private Text labelText;
        
        public void CreateBar(BrainFlowSingleChannelDataVisualizer graph, int barIndex)
        {
            brainFlowSingleChannelDataVisualizer = graph;
            graphRect = brainFlowSingleChannelDataVisualizer.graphContainerRect;
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
            if (brainFlowSingleChannelDataVisualizer.graphData.Count - 1 <  barId || barId > brainFlowSingleChannelDataVisualizer.numberOfDataPoints - 1)
            {
                barImage.enabled = false;
                return;
            }

            var xInterval = brainFlowSingleChannelDataVisualizer.xInterval;
            
            barImage.enabled = true;
            xPosition = (barId+1)*xInterval;
            yPosition = (float) (brainFlowSingleChannelDataVisualizer.graphData[barId] / brainFlowSingleChannelDataVisualizer.yMaximum) * graphRect.sizeDelta.y;
            
            barRect.sizeDelta = new Vector2(xInterval*0.8f, yPosition);
            barRect.anchoredPosition = new Vector2(xPosition, 0);
            
            
            barImage.color = brainFlowSingleChannelDataVisualizer.dataBarColor;
            
            labelRect.anchoredPosition = new Vector2(0, brainFlowSingleChannelDataVisualizer.xLabelOffset);
        }
        
        
        
    }
}