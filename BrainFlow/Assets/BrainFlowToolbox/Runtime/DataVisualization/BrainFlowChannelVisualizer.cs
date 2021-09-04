using System;
using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using BrainFlowToolbox.Runtime.DataStreaming;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization.ChannelDataStreaming
{
    public class BrainFlowChannelVisualizer : MonoBehaviour
    {
        public BrainFlowChannelDataStream channelDataStream;
        public BrainFlowDataTypeManager dataManager;
        private BrainFlowDataCanvas dataCanvas;
        private int channelID;
        private Canvas graphCanvas;
        private Image canvasImage;
        private bool initialized;
        public RectTransform graphRect;
        private RectTransform dataCanvasRect;
        public GameObject yLabelsContainer;
        public float xInterval;
        public int graphIndex;
        private int currentDataTotal;
        public List<double> graphData;
        public float graphHeight;
        

        public void Initialize(BrainFlowDataTypeManager manager, int channel)
        {
            dataManager = manager;
            channelID = channel;
            graphIndex = Array.FindIndex(dataManager.channelIds, c =>  c == channelID);
            graphCanvas = gameObject.AddComponent<Canvas>();
            canvasImage = gameObject.AddComponent<Image>();
            graphRect = gameObject.GetComponent<RectTransform>();
            dataCanvasRect = manager.dataCanvas.dataCanvasRect;
            
            initialized = true;
        }

        private void Update()
        {
            if (!initialized) return;
            var dataCanvasSizeDelta = dataCanvasRect.sizeDelta;
            xInterval = dataCanvasSizeDelta.x / (dataManager.numberOfChannels+1);
            graphRect.anchoredPosition = new Vector2(0, (graphIndex+1)*dataCanvas.yInterval);
            CreateGraphObjects();
            graphHeight = dataCanvasSizeDelta.y;
            var channelData = dataManager.channelData[channelID];

            graphData =  channelData.Count < dataManager.dataRange ? channelData :
                channelData.GetRange(channelData.Count - 1 - dataManager.dataRange, 
                    dataManager.dataRange);
        }
        

        private void CreateGraphObjects()
        {
            while (currentDataTotal < dataManager.dataRange)
            {
                var newDataBar = new GameObject("Data Bar: " + currentDataTotal, typeof(Image));
                newDataBar.transform.SetParent(graphRect, false);
                newDataBar.AddComponent<BrainFlowChannelDataPointController>().CreateBar(this, currentDataTotal);
                currentDataTotal++;
            }
        }

        private void CreateYLabels()
        {
            // if(yLabelsContainer) Destroy(yLabelsContainer);
            // yLabelsContainer = new GameObject("Y Labels");
            //
            // for (var i = 0; i < numberOfYLabels; i++)
            // {
            //     var yLabel = Instantiate(labelTemplateY,graphContainerRect, yLabelsContainer.transform);
            //     yLabel.gameObject.SetActive(true);
            //     var normalizedValue = i * 1f / numberOfYLabels;
            //     yLabel.anchoredPosition = new Vector2(yLabelOffset, normalizedValue*graphSizeDelta.y);
            //     yLabel.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();
            //     
            // }
            //
            // currentYLabels = numberOfYLabels;
        }
        
        
        // private RectTransform CreateDataPoint(Vector2 position)
        // {
        //     var newDataPoint = new GameObject("Data Point", typeof(Image));
        //     newDataPoint.transform.SetParent(graphContainerRect, false);
        //     var dataPointImage = newDataPoint.GetComponent<Image>();
        //     dataPointImage.sprite = circleSprite;
        //     dataPointImage.color = dataPointColor;
        //     var dataPointRect = newDataPoint.GetComponent<RectTransform>();
        //     dataPointRect.anchoredPosition = position;
        //     dataPointRect.sizeDelta = new Vector2(pointSize, pointSize);
        //     dataPointRect.anchorMin = Vector2.zero;
        //     dataPointRect.anchorMax = Vector2.zero;
        //     //dataPointGameObjects.Add(dataPointRect);
        //     return dataPointRect;
        // }
        
        // private RectTransform CreateDataBar(Vector2 position)
        // {
        //     var newDataBar = new GameObject("Data Bar", typeof(Image));
        //     newDataBar.transform.SetParent(graphContainerRect, false);
        //     var dataBarImage = newDataBar.GetComponent<Image>();
        //     dataBarImage.color = dataBarColor;
        //     var dataBarRect = newDataBar.GetComponent<RectTransform>();
        //     dataBarRect.anchoredPosition = new Vector2(position.x, 0);
        //     dataBarRect.sizeDelta = new Vector2(xInterval*0.8f, position.y);
        //     dataBarRect.anchorMin = Vector2.zero;
        //     dataBarRect.anchorMax = Vector2.zero;
        //     dataBarRect.pivot = new Vector2(0.5f, 0);
        //     //dataPointGameObjects.Add(dataBarRect);
        //     return dataBarRect;
        // }

        // private void CreateConnection(Vector2 pointA, Vector2 pointB)
        // {
        //     var newConnection = new GameObject("Connector", typeof(Image));
        //     var connectionImage = newConnection.GetComponent<Image>();
        //     connectionImage.color = lineColor;
        //     newConnection.transform.SetParent(graphContainerRect, false);
        //     var connectionRect = newConnection.GetComponent<RectTransform>();
        //     var direction = (pointB - pointA).normalized;
        //     var distance = Vector2.Distance(pointA, pointB);
        //     connectionRect.sizeDelta = new Vector2(distance, lineThickness);
        //     connectionRect.anchorMin = Vector2.zero;
        //     connectionRect.anchorMax = Vector2.zero;
        //     connectionRect.anchoredPosition = pointA + direction * distance * 0.5f;
        //     
        //     var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //     if (angle < 0) angle += 360;
        //
        //     connectionRect.localEulerAngles = new Vector3(0, 0, angle);
        // }
        
        
    }

    public enum VisualizationType
    {
        LineGraph,
        BarChart
    }
}
