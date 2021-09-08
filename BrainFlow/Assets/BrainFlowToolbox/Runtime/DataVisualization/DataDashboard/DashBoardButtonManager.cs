using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace BrainFlowToolbox.Runtime.DataVisualization.DataDashboard
{
    public class DashBoardButtonManager : MonoBehaviour
    {

        public Texture2D activeTexture;
        public Texture2D notActiveTexture;
        public RawImage buttonImage;
        public BrainFlowSessionProfile sessionProfile;
        public DataModels.Enumerators.VisualizationType visualizationType;


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
}
