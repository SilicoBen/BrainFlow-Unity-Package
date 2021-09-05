using System;
using System.Collections;
using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrainFlowDataDashboard : MonoBehaviour
{
    public Scrollbar xScale;
    public Scrollbar yScale;
    public TextMeshProUGUI text;
    public BrainFlowSessionProfile brainFlowSessionProfile;
    public bool initialized;
    public RectTransform dataContainer;

    public void Initialize(BrainFlowSessionProfile brainFlowSession)
    {
        brainFlowSessionProfile = brainFlowSession;
        brainFlowSessionProfile.dataContainer = dataContainer;
        yScale.direction = Scrollbar.Direction.TopToBottom;
        yScale.value = 0.5f;
        xScale.value = 0.1f;
        initialized = true;
    }
    
    private void Update()
    {
        if (!initialized) return;
        text.text = "Displaying " + brainFlowSessionProfile.displayData + " Data Streams";
        brainFlowSessionProfile.numberOfDataPoints =Mathf.Max (1, (int) (xScale.value*100f));
        brainFlowSessionProfile.yMaxValue = Mathf.Max (yScale.value*10000f, 0.05f);
    }
}
