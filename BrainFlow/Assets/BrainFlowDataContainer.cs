using System.Collections;
using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using UnityEngine;

public class BrainFlowDataContainer : MonoBehaviour
{

    public GameObject labelModel;
    public List<GameObject> xLabels;
    private BrainFlowSessionProfile brainFlowSessionProfile;
    private int numberOfSeconds;
    private bool initialized;
    private RectTransform rect;

    public void Initialize(BrainFlowSessionProfile sessionProfile)
    {
        brainFlowSessionProfile = sessionProfile;
        rect = gameObject.GetComponent<RectTransform>();
        initialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) return;
        brainFlowSessionProfile.numberOfSeconds = (int) (brainFlowSessionProfile.numberOfDataPoints / brainFlowSessionProfile.samplingRate);

        Debug.Log("Sampling Rate = " + brainFlowSessionProfile.samplingRate +
                  " Number Of Data Points = " +  brainFlowSessionProfile.numberOfDataPoints + 
                  " Number Of Seconds = " +  brainFlowSessionProfile.numberOfSeconds);
        
        
        while (xLabels.Count < brainFlowSessionProfile.numberOfSeconds * 2)
        {
            var newLabel = Instantiate(labelModel, transform);
            newLabel.name = xLabels.Count + " Second Label";
            newLabel.GetComponent<SecondLabelManager>().Initialize(rect, brainFlowSessionProfile, xLabels.Count);
            newLabel.SetActive(true);
            xLabels.Add(newLabel);
        }
        
    }
}
