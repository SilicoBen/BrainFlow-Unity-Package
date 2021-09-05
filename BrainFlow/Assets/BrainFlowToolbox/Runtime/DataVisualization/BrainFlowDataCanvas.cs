using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization.ChannelDataStreaming
{
    public class BrainFlowDataCanvas : MonoBehaviour
    {
        private BrainFlowDataTypeManager dataManager;
        private bool streaming;
        private double[,] data;
        private Canvas dataCanvas;
        private CanvasScaler canvasScaler;
        private RectTransform canvasRect;
        private GridLayoutGroup gridLayoutGroup;
        private readonly TMP_DefaultControls.Resources uiResources = new TMP_DefaultControls.Resources();
        private List<GameObject> dataStreamVisualizers = new List<GameObject>();
        private Slider slider;
        public RectTransform dataCanvasRect;
        public float yInterval;
        

        private void Update()
        {
            if (!streaming) return;
            var size = canvasRect.sizeDelta;
            dataManager.dataRange = (int) slider.value;
            dataManager.yInterval = size.y / (dataManager.numberOfChannels + 1);
            dataManager.xInterval = (size.x-10) / (dataManager.dataRange+1);
            dataManager.dataCanvasRect = dataCanvasRect;
            // gridLayoutGroup.constraintCount = dataManager.numberOfChannels;
            // gridLayoutGroup.cellSize = new Vector2(size.x,size.y / dataManager.numberOfChannels);
        }
        
        public void Initialize(BrainFlowDataTypeManager manager)
        {
            dataManager = manager;
            dataManager.dataCanvas = this;
            SetupDataCanvas();
            dataCanvasRect = gameObject.GetComponent<RectTransform>();
            dataCanvasRect.anchoredPosition = Vector2.zero;
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
            var sliderGO = (GameObject) Instantiate(Resources.Load("Prefabs/Slider"),transform, false);
            var sliderRect = sliderGO.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.5f, 0);
            sliderRect.anchorMax = new Vector2(0.5f, 0);
            sliderRect.pivot = new Vector2(0.5f, 0.5f);
            sliderRect.anchoredPosition = new Vector2(0, 10);
            slider = sliderGO.GetComponent<Slider>();
            slider.minValue = 1;
            slider.maxValue = 500;
        }
        
        public void AddDataStreamVisualizer(GameObject dataStreamVisualizer)
        {
            dataStreamVisualizers.Add(dataStreamVisualizer);
            dataStreamVisualizer.transform.SetParent(transform);
        }
        
    }
}