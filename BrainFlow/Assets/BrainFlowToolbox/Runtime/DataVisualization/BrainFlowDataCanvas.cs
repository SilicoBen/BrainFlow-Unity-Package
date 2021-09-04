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
        private List<GameObject> dataStreamVisualizers;
        private Slider slider;
        public RectTransform dataCanvasRect;
        public float yInterval;

        private void Update()
        {
            dataManager.dataRange = (int) slider.value;
            yInterval = dataCanvasRect.sizeDelta.y / (dataManager.numberOfChannels + 1);
        }
        
        public void Initialize(BrainFlowDataTypeManager manager)
        {
            dataManager = manager;
            streaming = true;
            SetupDataCanvas();
        }

        private void SetupDataCanvas()
        {
            dataCanvas = gameObject.AddComponent<Canvas>();
            canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            gameObject.AddComponent<GraphicRaycaster>();
            canvasRect = gameObject.GetComponent<RectTransform>();
            gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = dataManager.channelIds.Length;
            gridLayoutGroup.cellSize = new Vector2(75,25);
            dataCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            dataCanvas.worldCamera = Camera.main;
            var sliderGO = new GameObject("Data Range Slider", typeof(Slider));
            slider = sliderGO.GetComponent<Slider>();
            slider.minValue = 1;
            slider.maxValue = 500;
        }
        
        public void AddDataStreamVisualizer(GameObject dataStreamVisualizer)
        {
            dataStreamVisualizers.Add(dataStreamVisualizer);
        }
        
    }
}