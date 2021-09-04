﻿using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using BrainFlowToolbox.Runtime.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization
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
        public GameObject graphContainer;

        public void Initialize(BrainFlowSessionProfile sessionProfile)
        {
            brainFlowSessionProfile = sessionProfile;
            brainFlowSessionProfile.brainFlowDataDashboard = this;
            streaming = true;
            SetupDataCanvas();
            CreateDataStreamers();
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
            gridLayoutGroup.constraintCount = brainFlowSessionProfile.ChannelTypeChannelIds.Count;
            gridLayoutGroup.cellSize = new Vector2(75,25);
            dataCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            dataCanvas.worldCamera = Camera.main;
        }
        
        private void CreateDataStreamers()
        {
            foreach (var c in brainFlowSessionProfile.ChannelTypeChannelIds)
            {
                if (c.Value.Length == 0) continue;

                var newDataBoard = new GameObject(c.Key + "Data Flow Board");
                var dataFlowBoard = newDataBoard.AddComponent<BrainFlowChannelTypeDashboard>();
                dataFlowBoard.Create(brainFlowSessionProfile, c.Key);
                brainFlowSessionProfile.DataFlowBoards[c.Key] = dataFlowBoard;
                
                var newChannelContainer = new GameObject(c.Key);
                newChannelContainer.name = c.Key;
                newChannelContainer.transform.SetParent(transform);
                var gridLayout = newChannelContainer.AddComponent<GridLayoutGroup>();
                gridLayout.cellSize = new Vector2(500,25);
                var id = 0;
                foreach (var i in c.Value)
                {
                    var newGO = TMP_DefaultControls.CreateText(uiResources);
                    newGO.transform.SetParent(newChannelContainer.transform);

                    var objectComponent = newGO.AddComponent<BrainFlowSingleChannelDataStream>();
                    objectComponent.Initialize(brainFlowSessionProfile, c.Key, i, id);
                    id++;
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