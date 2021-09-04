using System;
using brainflow;
using BrainFlowToolbox.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime
{
    public class BrainFlowDisplayTextData : MonoBehaviour
    {
        public BrainFlowSessionProfile brainFlowSessionProfile;
        public int dataID;
        private TextMeshProUGUI textOptions;
        private bool streaming;
        private RectTransform rect;
        private int dataCycle;
        private string channel;
        
        
        public void Initialize(BrainFlowSessionProfile sessionProfile, string channelName, int dataIndex)
        {
            brainFlowSessionProfile = sessionProfile;
            channel = channelName;
            dataID = dataIndex;
            textOptions = gameObject.GetComponent<TextMeshProUGUI>();
            rect = gameObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.localScale = new Vector3(1, 1, 1);
            //textOptions.autoSizeTextContainer = true;
            textOptions.fontSize = 8;
            streaming = true;
        }

        private void Update()
        {
            dataCycle++;
            if (brainFlowSessionProfile.currentData == null) return;
            
            textOptions.text = channel + dataID + ": " + Math.Round(brainFlowSessionProfile.currentData[dataID, 0], 4);
          
            
        }
        
    }
}