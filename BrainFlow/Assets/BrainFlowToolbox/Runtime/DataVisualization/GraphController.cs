using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace BrainFlowToolbox.Runtime.DataVisualization
{
    public class GraphController : MonoBehaviour
    {
        
        public VisualizationType visualizationType;
        
        [Header("Graph Settings")]
        public float xLabelOffset = -10;
        public float yLabelOffset = -10;
        public int numberOfYLabels = 10;
        [Range(1,100)]
        public int numberOfDataPoints;
        public float yMaximum;
        
        [Header("Line Graph Settings")]
        public Sprite circleSprite;
        public Color dataPointColor = Color.white;
        public float pointSize = 10;
        public Color lineColor = Color.black;
        public float lineThickness = 3;
        
        [Header("Bar Chart Settings")]
        public Color dataBarColor = Color.white;
        [Range(0,1)]
        public float barWidthMultiplier;
        
        
        [Header("Required GameObjects")]
        public RectTransform graphContainerRect;
        public RectTransform labelTemplateX;
        public RectTransform labelTemplateY;
        public Slider dataPointSlider;
        
        [HideInInspector]public List<int> graphData;
        [HideInInspector]public float xInterval;
        private Vector2 graphSizeDelta;
        private int currentYLabels;
        private int dataObjectTotal;
        private int dataIndex;

        public GameObject yLabelsContainer;

        private void Awake()
        {
            dataObjectTotal = 0;
            dataPointSlider.wholeNumbers = true;
            graphSizeDelta = graphContainerRect.sizeDelta;
            // CreateGraphObjects();
        }

        private void Update()
        {
            numberOfDataPoints = Mathf.RoundToInt(dataPointSlider.value);
            AddData();
            xInterval = graphContainerRect.sizeDelta.x / (numberOfDataPoints+1);
            if(dataObjectTotal < numberOfDataPoints) CreateGraphObjects();
            //if (currentYLabels != numberOfYLabels) CreateYLabels();
        }

        private void AddData()
        {
            if (graphData.Count < numberOfDataPoints)
            {
                graphData.Add(Random.Range(0, 100));
            }
            else
            {
                if (dataIndex > numberOfDataPoints - 2) dataIndex = 0;
                else dataIndex++;
                graphData[dataIndex] = Random.Range(0, 100);
                Debug.Log(dataIndex);
                
            }
        }

        private void CreateGraphObjects()
        {
            while (dataObjectTotal < numberOfDataPoints)
            {
                var newDataBar = new GameObject("Data Bar: " + dataObjectTotal, typeof(Image));
                newDataBar.transform.SetParent(graphContainerRect, false);
                newDataBar.AddComponent<BarController>().CreateBar(this, dataObjectTotal);
                dataObjectTotal++;
            }
        }

        private void CreateYLabels()
        {
            if(yLabelsContainer) Destroy(yLabelsContainer);
            yLabelsContainer = new GameObject("Y Labels");
            
            for (var i = 0; i < numberOfYLabels; i++)
            {
                var yLabel = Instantiate(labelTemplateY,graphContainerRect, yLabelsContainer.transform);
                yLabel.gameObject.SetActive(true);
                var normalizedValue = i * 1f / numberOfYLabels;
                yLabel.anchoredPosition = new Vector2(yLabelOffset, normalizedValue*graphSizeDelta.y);
                yLabel.GetComponent<Text>().text = Mathf.RoundToInt(normalizedValue * yMaximum).ToString();
                
            }

            currentYLabels = numberOfYLabels;
        }
        
        
        private RectTransform CreateDataPoint(Vector2 position)
        {
            var newDataPoint = new GameObject("Data Point", typeof(Image));
            newDataPoint.transform.SetParent(graphContainerRect, false);
            var dataPointImage = newDataPoint.GetComponent<Image>();
            dataPointImage.sprite = circleSprite;
            dataPointImage.color = dataPointColor;
            var dataPointRect = newDataPoint.GetComponent<RectTransform>();
            dataPointRect.anchoredPosition = position;
            dataPointRect.sizeDelta = new Vector2(pointSize, pointSize);
            dataPointRect.anchorMin = Vector2.zero;
            dataPointRect.anchorMax = Vector2.zero;
            //dataPointGameObjects.Add(dataPointRect);
            return dataPointRect;
        }
        
        private RectTransform CreateDataBar(Vector2 position)
        {
            var newDataBar = new GameObject("Data Bar", typeof(Image));
            newDataBar.transform.SetParent(graphContainerRect, false);
            var dataBarImage = newDataBar.GetComponent<Image>();
            dataBarImage.color = dataBarColor;
            var dataBarRect = newDataBar.GetComponent<RectTransform>();
            dataBarRect.anchoredPosition = new Vector2(position.x, 0);
            dataBarRect.sizeDelta = new Vector2(xInterval*0.8f, position.y);
            dataBarRect.anchorMin = Vector2.zero;
            dataBarRect.anchorMax = Vector2.zero;
            dataBarRect.pivot = new Vector2(0.5f, 0);
            //dataPointGameObjects.Add(dataBarRect);
            return dataBarRect;
        }

        private void CreateConnection(Vector2 pointA, Vector2 pointB)
        {
            var newConnection = new GameObject("Connector", typeof(Image));
            var connectionImage = newConnection.GetComponent<Image>();
            connectionImage.color = lineColor;
            newConnection.transform.SetParent(graphContainerRect, false);
            var connectionRect = newConnection.GetComponent<RectTransform>();
            var direction = (pointB - pointA).normalized;
            var distance = Vector2.Distance(pointA, pointB);
            connectionRect.sizeDelta = new Vector2(distance, lineThickness);
            connectionRect.anchorMin = Vector2.zero;
            connectionRect.anchorMax = Vector2.zero;
            connectionRect.anchoredPosition = pointA + direction * distance * 0.5f;
            
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            connectionRect.localEulerAngles = new Vector3(0, 0, angle);
        }
        
        
    }

    public enum VisualizationType
    {
        LineGraph,
        BarChart
    }
}
