using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.Managers
{
    public class BrainFlowChannelTypeDataCanvas : MonoBehaviour
    {
        private BrainFlowChannelTypeData channelTypeData;
        private BrainFlowSessionProfile sessionProfile;
        private bool streaming;
        private Canvas dataCanvas;
        private CanvasScaler canvasScaler;
        private GridLayoutGroup gridLayoutGroup;
        public RectTransform dataCanvasRect;
        public Transform t;
        public RectTransform rect;
        
        public void Initialize(BrainFlowChannelTypeData data)
        {
            channelTypeData = data;
            sessionProfile = data.sessionProfile;
            dataCanvasRect = sessionProfile.dataContainer;
            SetupDataCanvas();
            transform.SetParent(sessionProfile.dataContainer);
            rect = gameObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            CreateChannelCanvases();
            streaming = true;
        }

        private void Update()
        {
            transform.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.sizeDelta = sessionProfile.dataContainer.sizeDelta;
            var canvasSize = rect.sizeDelta;
            channelTypeData.dataCanvasSize = canvasSize;
            channelTypeData.channelCanvasSize = new Vector2(canvasSize.x * 0.8f,
                canvasSize.y * 0.9f / channelTypeData.channelIds.Length);

        }

        private void SetupDataCanvas()
        {
            dataCanvas = gameObject.AddComponent<Canvas>();
            canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            gameObject.AddComponent<GraphicRaycaster>();
            dataCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            dataCanvas.worldCamera = Camera.main;
        }

        private void CreateChannelCanvases()
        {
            foreach (var i in channelTypeData.channelIds)
            {
                var newDataVisualizer =  new GameObject(channelTypeData.channelType + " CH" + i + " Visualizer");
                newDataVisualizer.transform.SetParent(transform);
                var visualizerComponent = newDataVisualizer.AddComponent<BrainFlowChannelDataCanvas>();
                channelTypeData.ChannelData[i].channelTypeDataCanvas = this;
                visualizerComponent.Initialize(channelTypeData.ChannelData[i]);
            }
        }
        
    }
}