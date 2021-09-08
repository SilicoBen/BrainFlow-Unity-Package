using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization
{
    public class BrainFlowChannelCanvas : MonoBehaviour
    {
        private BrainFlowDataTypeManager dataManager;
        private bool streaming;
        private double[,] data;
        private Canvas dataCanvas;
        private CanvasScaler canvasScaler;
        private RectTransform canvasRect;
        private GridLayoutGroup gridLayoutGroup;
        private List<GameObject> dataStreamVisualizers = new List<GameObject>();
        public RectTransform dataCanvasRect;
        public float yInterval;
        

        private void Update()
        {
            if (!streaming) return;
            var size = canvasRect.sizeDelta;
            transform.localPosition = Vector3.zero;
            dataManager.yInterval = size.y / (dataManager.numberOfChannels + 1);
            dataManager.xInterval = (size.x*0.8f) / (dataManager.sessionProfile.numberOfDataPoints);
            dataCanvasRect.sizeDelta = dataManager.sessionProfile.dataContainer.sizeDelta;
            dataManager.dataCanvasRect = dataCanvasRect;
            // gridLayoutGroup.constraintCount = dataManager.numberOfChannels;
            // gridLayoutGroup.cellSize = new Vector2(size.x,size.y / dataManager.numberOfChannels);
        }
        
        public void Initialize(BrainFlowDataTypeManager manager)
        {
            dataManager = manager;
            dataManager.channelCanvas = this;
            SetupDataCanvas();
            transform.SetParent(manager.sessionProfile.dataContainer);
            dataCanvasRect = gameObject.GetComponent<RectTransform>();
            dataCanvasRect.anchorMin = new Vector2(0.5f, 0.5f);
            dataCanvasRect.anchorMax = new Vector2(0.5f, 0.5f);
            dataCanvasRect.pivot = new Vector2(0.5f, 0.5f);
            dataManager.yInterval = (dataCanvasRect.sizeDelta.y -10) / (dataManager.numberOfChannels + 1);
            streaming = true;
        }

        private void SetupDataCanvas()
        {
            dataCanvas = gameObject.AddComponent<Canvas>();
            canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            gameObject.AddComponent<GraphicRaycaster>();
            canvasRect = gameObject.GetComponent<RectTransform>();
            // gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            // gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            dataCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            dataCanvas.worldCamera = Camera.main;
        }
        
        public void AddDataStreamVisualizer(GameObject dataStreamVisualizer)
        {
            dataStreamVisualizers.Add(dataStreamVisualizer);
            dataStreamVisualizer.transform.SetParent(transform);
        }
        
    }
}