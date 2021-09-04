using System;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization
{
    public class BarController : MonoBehaviour
    {
        public GraphController graphController;
        public RectTransform graphRect;
        public int barId;
        private float yPosition;
        private float xPosition;
        private Image barImage;
        private RectTransform barRect;
        private GameObject label;
        private RectTransform labelRect;
        private Text labelText;
        
        public void CreateBar(GraphController graph, int barIndex)
        {
            graphController = graph;
            graphRect = graphController.graphContainerRect;
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
            if (graphController.graphData.Count - 1 <  barId || barId > graphController.numberOfDataPoints - 1)
            {
                barImage.enabled = false;
                return;
            }

            var xInterval = graphController.xInterval;
            
            barImage.enabled = true;
            xPosition = (barId+1)*xInterval;
            yPosition = (graphController.graphData[barId] / graphController.yMaximum) * graphRect.sizeDelta.y;
            
            barRect.sizeDelta = new Vector2(xInterval*0.8f, yPosition);
            barRect.anchoredPosition = new Vector2(xPosition, 0);
            
            
            barImage.color = graphController.dataBarColor;
            
            labelRect.anchoredPosition = new Vector2(0, graphController.xLabelOffset);
        }
        
        
        
    }
}