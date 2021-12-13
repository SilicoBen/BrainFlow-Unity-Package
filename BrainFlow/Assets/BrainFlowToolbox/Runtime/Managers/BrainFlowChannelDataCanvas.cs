using System;
using System.Collections.Generic;
using System.Linq;
using BrainFlowToolbox.Runtime.DataModels.Classes;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.Managers
{
    public class BrainFlowChannelDataCanvas : MonoBehaviour
    {
        public BrainFlowChannelData channelData;
        private Image canvasImage;
        private bool initialized;
        public RectTransform canvasRect;
        private RectTransform dataCanvasRect;
        public float graphHeight;
        public TextMeshProUGUI canvasLabel;
        private readonly TMP_DefaultControls.Resources uiResources = new TMP_DefaultControls.Resources();
        private Slider yRangeSlider;
        private RectTransform sliderRect;
        private List<GameObject> dataPointGameObjects = new List<GameObject>();
        private BrainFlowSessionProfile brainFlowSessionProfile;

        public void Initialize(BrainFlowChannelData data)
        {
            channelData = data;
            gameObject.AddComponent<Canvas>();
            canvasImage = gameObject.AddComponent<Image>();
            canvasRect = gameObject.GetComponent<RectTransform>();
            gameObject.AddComponent<GraphicRaycaster>();
            brainFlowSessionProfile = channelData.sessionProfile;
            
            // setup size based on the size of the data canvas for the channel type data
            dataCanvasRect = data.channelTypeDataCanvas.dataCanvasRect;
            canvasRect.anchorMin = new Vector2(0.5f, 0);
            canvasRect.anchorMax = new Vector2(0.5f, 0);
            canvasRect.pivot = new Vector2(0.5f, 0.5f);
            transform.localScale = new Vector3(1,1,1);
            canvasRect.sizeDelta = new Vector2(dataCanvasRect.sizeDelta.x*0.8f, 
                channelData.channelTypeData.dataCanvasSize.y/channelData.channelTypeData.channelIds.Length*0.95f);
            
            
            // Create Label 
            var labelTextGO = TMP_DefaultControls.CreateText(uiResources);
            canvasLabel = labelTextGO.GetComponent<TextMeshProUGUI>();
            labelTextGO.transform.SetParent(transform);
            var textRect = labelTextGO.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0,0.5f);
            textRect.anchorMax = new Vector2(0, 0.5f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.anchoredPosition = new Vector2(-20,0);
            canvasLabel.text = "CH" + channelData.channelBoardID;
            
            // Create slider for controling y-axis
            var slider = (GameObject) Instantiate(Resources.Load("Prefabs/Slider"), transform);
            yRangeSlider = slider.GetComponent<Slider>();
            yRangeSlider.value = 1;
            sliderRect = yRangeSlider.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(1,0.5f);
            sliderRect.anchorMax = new Vector2(1, 0.5f);
            sliderRect.pivot = new Vector2(0.5f, 0.5f);
            sliderRect.anchoredPosition = new Vector2(20,0);
            yRangeSlider.transform.localEulerAngles = new Vector3(0,0,90);
            
            initialized = true;
        }

        private void Update()
        {
            if (!initialized) return;
            canvasImage.color = channelData.sessionProfile.graphBackgroundColor;
            var canvasSize = channelData.channelTypeData.channelCanvasSize;
            var dataCanvasSize = channelData.channelTypeData.dataCanvasSize;
            canvasRect.sizeDelta = new Vector2(canvasSize.x, canvasSize.y * 0.95f);
            sliderRect.sizeDelta = new Vector2(canvasSize.y, 20);
            canvasRect.anchoredPosition = new Vector2(0, canvasSize.y * (channelData.channelTypeData.channelIds.Length-channelData.channelTypeIndex-1) + dataCanvasSize.x*0.05f);
            channelData.yAxisScaler = yRangeSlider.value*brainFlowSessionProfile.dataScaler;
            
            channelData.xInterval = canvasSize.x / channelData.channelData.Count;
            CreateGraphObjects();
        }
        
        private void CreateGraphObjects()
        {
            while (dataPointGameObjects.Count < channelData.channelData.Count)
            {
                var newDataPoint = new GameObject("Data Point: " + dataPointGameObjects.Count, typeof(Image));
                newDataPoint.GetComponent<Image>().enabled = false;
                newDataPoint.transform.SetParent(transform, false);
                newDataPoint.AddComponent<BrainFlowDataPointManager>().Initialize(this, dataPointGameObjects.Count);
                dataPointGameObjects.Add(newDataPoint);
            }
        }
    }

}
