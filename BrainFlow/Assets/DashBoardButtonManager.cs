using System.Collections;
using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.Enumerators;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


public class DashBoardButtonManager : MonoBehaviour
{

    public Texture2D activeTexture;
    public Texture2D notActiveTexture;
    public RawImage buttonImage;
    public BrainFlowSessionProfile sessionProfile;
    public VisualizationType visualizationType;


    public void Initialize(BrainFlowSessionProfile profile)
    {
        sessionProfile = profile;
    }

    // Update is called once per frame
    void Update()
    {
        if (!sessionProfile) return;

        buttonImage.texture = sessionProfile.visualizationType == visualizationType ? activeTexture : notActiveTexture;
    }
}
