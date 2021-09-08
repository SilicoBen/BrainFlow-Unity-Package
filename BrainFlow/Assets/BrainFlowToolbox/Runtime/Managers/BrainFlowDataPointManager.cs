using System;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.Managers
{
    public class BrainFlowDataPointManager : MonoBehaviour
    {
        public int dataId;
        private float xPosition;
        public float dataValue;
        private Image barLineImage;
        private RectTransform barLineRect;
        private BrainFlowChannelDataCanvas channelDataCanvas;
        public BrainFlowChannelData channelData;
        private bool initialized;
        
        
        public void Initialize(BrainFlowChannelDataCanvas graph, int dataIndex)
        {
            channelDataCanvas = graph;
            channelData = graph.channelData;
            dataId = dataIndex;
            barLineImage = gameObject.GetComponent<Image>();
            barLineRect = gameObject.GetComponent<RectTransform>();
            initialized = true;
        }
        
        private void Update()
        {
            if (!initialized) return;
            
            var xInterval = channelData.channelTypeData.dataCanvasSize.x*0.8f / channelData.channelData.Count;

            switch (channelData.sessionProfile.visualizationType)
            {
                case DataModels.Enumerators.VisualizationType.Line:
                    if (channelData.channelData.Count <  dataId + 1 || dataId + 1 > channelData.channelData.Count-1)
                    {
                        barLineImage.enabled = false;
                    }
                    else
                    {
                        barLineImage.enabled = true;
                        var dataPoint = new Vector2((dataId+1)*xInterval, (float) channelData.channelData[dataId]* channelData.yAxisScaler);
                        var nextDataPoint = new Vector2((dataId+2)*xInterval, (float) channelData.channelData[dataId+1]* channelData.yAxisScaler);
                        var direction = (nextDataPoint - dataPoint).normalized;
                        var distance = Vector2.Distance(dataPoint, nextDataPoint);
                        barLineRect.sizeDelta = new Vector2(distance, channelData.sessionProfile.thickness*3);
                        barLineRect.anchorMin = new Vector2(0, 0.5f);
                        barLineRect.anchorMax = new Vector2(0, 0.5f);
                        barLineRect.anchoredPosition = dataPoint + direction * (distance * 0.5f);
                        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        if (angle < 0) angle += 360;
                        barLineRect.localEulerAngles = new Vector3(0, 0, angle);
                    }
                    break;
                case DataModels.Enumerators.VisualizationType.Bar:
                    if (channelData.channelData.Count <  dataId || dataId > channelData.channelData.Count-1)
                    {
                        barLineImage.enabled = false;
                    }
                    else
                    {
                        dataValue = (float) channelData.channelData[dataId];
                        var dataPoint = new Vector2((dataId+1)*xInterval, (float) dataValue* channelData.yAxisScaler);
                        barLineImage.enabled = true;

                        barLineRect.anchorMin = new Vector2(0, 0.5f);
                        barLineRect.anchorMax = new Vector2(0, 0.5f);
                        barLineRect.pivot = new Vector2(0.5f, 0.5f);
                        barLineRect.localEulerAngles = new Vector3(0, 0, 0);
                        barLineRect.sizeDelta = new Vector2(xInterval*0.8f, Math.Abs(dataValue) * channelData.yAxisScaler);
                        barLineRect.anchoredPosition = new Vector2(dataPoint.x, (dataValue * channelData.yAxisScaler)/2);
                    }
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
            barLineImage.color = channelData.sessionProfile.graphBarColor;
            
            //labelRect.anchoredPosition = new Vector2(0, brainFlowChannelVisualizer.xLabelOffset);
        }
        
        
        
    }
}