using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization
{
    public class SecondLabelManager : MonoBehaviour
    {
        public Image labelImage;
        public TextMeshProUGUI labelText;
        public int second;
        public BrainFlowSessionProfile sessionProfile;
        private bool initialized;
        private RectTransform container;
        private RectTransform labelRect;
    
    
        public void Initialize(RectTransform dataContainer, BrainFlowSessionProfile profile, int s)
        {
            container = dataContainer;
            sessionProfile = profile;
            second = s;
            labelRect = gameObject.GetComponent<RectTransform>();
            initialized = true;
            labelText.text = s > 0 ? "-"+ s*0.5f +"sec" : "0sec";
        }

        // Update is called once per frame
        void Update()
        {
            if (!initialized || sessionProfile.numberOfSeconds < second)
            {
                labelImage.enabled = false;
                labelText.enabled = false;
                return;
            }

            var containerSize = container.sizeDelta;
            labelRect.anchoredPosition = new Vector2(-(float)(containerSize.x*0.8f / (sessionProfile.numberOfDataPoints / sessionProfile.samplingRate))*second -containerSize.x*0.1f, 0);
        
            labelImage.enabled = true;
            labelText.enabled = true;
        
        
        
        }
    }
}
