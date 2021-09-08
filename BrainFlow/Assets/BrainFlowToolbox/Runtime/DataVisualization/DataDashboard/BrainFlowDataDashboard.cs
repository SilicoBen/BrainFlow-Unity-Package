using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization.DataDashboard
{
    public class BrainFlowDataDashboard : MonoBehaviour
    {
        public Slider xScale;
        public TextMeshProUGUI titleText;
        public BrainFlowSessionProfile brainFlowSessionProfile;
        public bool initialized;
        public RectTransform dataContainer;
        public Slider thickness;
        public TextMeshProUGUI thicknessText;
        public BrainFlowDataContainer brainFlowDataContainer;
        public List<DashBoardButtonManager> buttons;


        public void Initialize(BrainFlowSessionProfile brainFlowSession)
        {
            brainFlowSessionProfile = brainFlowSession;
            brainFlowSessionProfile.dataContainer = dataContainer;
            brainFlowDataContainer.Initialize(brainFlowSession);
            foreach (var b in buttons)
            {
                b.Initialize(brainFlowSessionProfile);
            }
            initialized = true;
        }
    
        private void Update()
        {
            if (!initialized) return;
            thicknessText.text = brainFlowSessionProfile.visualizationType + " Thickness";
            brainFlowSessionProfile.thickness = thickness.value;
            titleText.text = "Displaying " + brainFlowSessionProfile.dataCanvas + " Data Streams";
            brainFlowSessionProfile.numberOfDataPoints =  Mathf.RoundToInt(xScale.value);
        }
        
        public void ChangeGraphType(string type)
        {
            brainFlowSessionProfile.visualizationType = type switch
            {
                "Bar" => DataModels.Enumerators.VisualizationType.Bar,
                "Line" => DataModels.Enumerators.VisualizationType.Line,
                _ => brainFlowSessionProfile.visualizationType
            };
        
        }
    }
}
