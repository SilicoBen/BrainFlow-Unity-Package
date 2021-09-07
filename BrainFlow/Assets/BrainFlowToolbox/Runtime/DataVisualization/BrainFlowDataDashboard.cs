using System;
using System.Collections;
using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrainFlowDataDashboard : MonoBehaviour
{
    public Slider xScale;
    public Slider yScale;
    public TextMeshProUGUI titleText;
    public BrainFlowSessionProfile brainFlowSessionProfile;
    public bool initialized;
    public RectTransform dataContainer;
    public Slider thickness;
    public TextMeshProUGUI thicknessText;

    public void Initialize(BrainFlowSessionProfile brainFlowSession)
    {
        brainFlowSessionProfile = brainFlowSession;
        brainFlowSessionProfile.dataContainer = dataContainer;
        initialized = true;
    }
    
    private void Update()
    {
        if (!initialized) return;
        thicknessText.text = brainFlowSessionProfile.visualizationType + " Thickness";
        brainFlowSessionProfile.thickness = thickness.value;
        titleText.text = "Displaying " + brainFlowSessionProfile.displayData + " Data Streams";
        brainFlowSessionProfile.numberOfDataPoints = (int) xScale.value;
        brainFlowSessionProfile.yMaxValue = yScale.value * 1000;
    }

    public void ChangeGraphType(string type)
    {
        brainFlowSessionProfile.visualizationType = type switch
        {
            "Bar" => VisualizationType.Bar,
            "Line" => VisualizationType.Line,
            _ => brainFlowSessionProfile.visualizationType
        };
        
    }
}
