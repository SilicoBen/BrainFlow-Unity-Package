using System.Collections.Generic;
using BrainFlowToolbox.Runtime.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization
{
    public class BrainFlowChannelTypeDashboard : MonoBehaviour
    {
        private BrainFlowSessionProfile brainFlowSessionProfile;
        private bool streaming;
        private double[,] data;
        private Canvas dataCanvas;
        private CanvasScaler canvasScaler;
        private RectTransform canvasRect;
        private GridLayoutGroup gridLayoutGroup;
        private readonly TMP_DefaultControls.Resources uiResources = new TMP_DefaultControls.Resources();
        private List<GameObject> boardChannelContainers;
        public GameObject graphContainer;
        public int numberOfChannels;
        public BrainFlowSessionProfile sessionProfile;
        public List<GameObject> channelGraphs = new List<GameObject>();
        public string channelType;
        [Range(3,500)]
        public float xRange = 10;
        public float rowHeight;
        public float colWidth;
        public RectTransform rect;
        
        public void Update()
        {
            numberOfChannels = channelGraphs.Count;
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = numberOfChannels;
            var size = canvasRect.sizeDelta;
            rowHeight = size.y / numberOfChannels;
            colWidth = size.x;
            gridLayoutGroup.cellSize = new Vector2(colWidth,rowHeight);
        }
        
        public void Create(BrainFlowSessionProfile profile, string channelName)
        {
            channelType = channelName;
            sessionProfile = profile;
            gameObject.name = channelType + " Data Flow Visualizer";
            dataCanvas = gameObject.AddComponent<Canvas>();
            canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            gameObject.AddComponent<GraphicRaycaster>();
            canvasRect = gameObject.GetComponent<RectTransform>();
            gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            dataCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            dataCanvas.worldCamera = Camera.main;
            
        }

        public void AddChannelDataFlow(GameObject channelDataVisualizer)
        {
            if (channelDataVisualizer == null) Debug.Log("channel Game Object Is Null");
            channelGraphs.Add(channelDataVisualizer);
            channelDataVisualizer.transform.SetParent(transform);
        }
        
        
        
    }
}