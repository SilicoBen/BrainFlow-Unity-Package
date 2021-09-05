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
    public Scrollbar xScale;
    public Scrollbar yScale;
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
        yScale.direction = Scrollbar.Direction.TopToBottom;
        yScale.value = 0.5f;
        xScale.value = 0.1f;
        initialized = true;
    }
    
    private void Update()
    {
        if (!initialized) return;
        thicknessText.text = brainFlowSessionProfile.visualizationType + " Thickness";
        brainFlowSessionProfile.thickness = thickness.value;
        titleText.text = "Displaying " + brainFlowSessionProfile.displayData + " Data Streams";
        brainFlowSessionProfile.numberOfDataPoints =Mathf.Max (1, (int) (xScale.value*100f));
        brainFlowSessionProfile.yMaxValue = Mathf.Max (yScale.value*10000f, 0.05f);
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
