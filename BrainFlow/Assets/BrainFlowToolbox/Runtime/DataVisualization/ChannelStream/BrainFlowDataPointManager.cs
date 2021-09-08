using System;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization
{
    public class BrainFlowDataPointManager : MonoBehaviour
    {
        public RectTransform graphRect;
        public int dataId;
        private float xPosition;
        public float dataValue;
        private Image barLineImage;
        private RectTransform barLineRect;
        private BrainFlowChannelVisualizer channelVisualizer;
        public BrainFlowDataTypeManager dataManager;
        private bool initialized;
        
        
        public void Initialize(BrainFlowChannelVisualizer graph, int dataIndex)
        {
            channelVisualizer = graph;
            dataManager = graph.dataManager;
            graphRect = graph.graphRect;
            dataId = dataIndex;
            
            barLineImage = gameObject.GetComponent<Image>();
            barLineRect = gameObject.GetComponent<RectTransform>();

            initialized = true;
        }

        
        
        private void Update()
        {
            if (!initialized) return;
            
            var xInterval = dataManager.xInterval;
            var yScaling = channelVisualizer.graphHeight / dataManager.sessionProfile.yMaxValue;

            switch (dataManager.sessionProfile.visualizationType)
            {
                case DataModels.Enumerators.VisualizationType.Line:
                    if (dataManager.dataRange <  dataId + 1 || dataId + 1 > channelVisualizer.graphData.Count - 1)
                    {
                        barLineImage.enabled = false;
                    }
                    else
                    {
                        barLineImage.enabled = true;
                        var dataPoint = new Vector2((dataId+1)*xInterval, (float) channelVisualizer.graphData[dataId]* yScaling);
                        var nextDataPoint = new Vector2((dataId+2)*xInterval, (float) channelVisualizer.graphData[dataId+1]* yScaling);
                        var direction = (nextDataPoint - dataPoint).normalized;
                        var distance = Vector2.Distance(dataPoint, nextDataPoint);
                        barLineRect.sizeDelta = new Vector2(distance, dataManager.sessionProfile.thickness*3);
                        barLineRect.anchorMin = new Vector2(0, 0.5f);
                        barLineRect.anchorMax = new Vector2(0, 0.5f);
                        barLineRect.anchoredPosition = dataPoint + direction * (distance * 0.5f);
                        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        if (angle < 0) angle += 360;
                        barLineRect.localEulerAngles = new Vector3(0, 0, angle);
                    }
                    break;
                case DataModels.Enumerators.VisualizationType.Bar:
                    if (dataManager.dataRange <  dataId || dataId > channelVisualizer.graphData.Count - 1)
                    {
                        barLineImage.enabled = false;
                    }
                    else
                    {
                        dataValue = (float) channelVisualizer.graphData[dataId];
                        var dataPoint = new Vector2((dataId+1)*xInterval, (float) dataValue* yScaling);
                        barLineImage.enabled = true;

                        barLineRect.anchorMin = new Vector2(0, 0.5f);
                        barLineRect.anchorMax = new Vector2(0, 0.5f);
                        barLineRect.pivot = new Vector2(0.5f, 0.5f);
                        barLineRect.localEulerAngles = new Vector3(0, 0, 0);
                        barLineRect.sizeDelta = new Vector2(xInterval*0.8f, Math.Abs(dataValue) * yScaling);
                        barLineRect.anchoredPosition = new Vector2(dataPoint.x, (dataValue * yScaling)/2);
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
            barLineImage.color = dataManager.sessionProfile.graphBarColor;
            
            //labelRect.anchoredPosition = new Vector2(0, brainFlowChannelVisualizer.xLabelOffset);
        }
        
        
        
    }
}