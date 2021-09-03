using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime
{
    public class BrainFlowDataDashboard : MonoBehaviour
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
        
        public void Initialize(BrainFlowSessionProfile sessionProfile)
        {
            brainFlowSessionProfile = sessionProfile;
            brainFlowSessionProfile.brainFlowSessionProfile = this;
            streaming = true;
            SetupDataCanvas();
            AddDataContainers();
        }

        private void SetupDataCanvas()
        {
            dataCanvas = gameObject.AddComponent<Canvas>();
            canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            gameObject.AddComponent<GraphicRaycaster>();
            canvasRect = gameObject.GetComponent<RectTransform>();
            gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayoutGroup.constraintCount = brainFlowSessionProfile.ChannelDictionary.Count;
            gridLayoutGroup.cellSize = new Vector2(75,25);
            dataCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            dataCanvas.worldCamera = Camera.main;
        }

        private void AddDataContainers()
        {

            foreach (var c in brainFlowSessionProfile.ChannelDictionary)
            {
                if (c.Value.Length == 0) continue;
                var newChannelContainer = new GameObject(c.Key);
                newChannelContainer.name = c.Key;
                newChannelContainer.transform.SetParent(transform);
                var gridLayout = newChannelContainer.AddComponent<GridLayoutGroup>();
                gridLayout.cellSize = new Vector2(500,25);
                foreach (var i in c.Value)
                {
                    var newGO = TMP_DefaultControls.CreateText(uiResources);
                    newGO.transform.SetParent(newChannelContainer.transform);

                    var objectComponent = newGO.AddComponent<BrainFlowDisplayTextData>();
                    objectComponent.Initialize(brainFlowSessionProfile, c.Key, i);
                }
            }
            
        }
        
        void Update()
        {
            // data = board_shim.get_current_board_data(number_of_data_points);
            // // check https://brainflow.readthedocs.io/en/stable/index.html for api ref and more code samples
            // Debug.Log("Num elements: " + data.GetLength(1));
        }
    }
}