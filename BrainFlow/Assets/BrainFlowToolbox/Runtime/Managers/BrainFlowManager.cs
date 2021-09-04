using BrainFlowToolbox.Runtime.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace BrainFlowToolbox.Runtime.Managers
{
    public class BrainFlowManager : MonoBehaviour
    {
        public BrainFlowSessionProfile brainFlowSessionProfile; 
        
        public void StartSession(BrainFlowSessionProfile sessionProfile)
        {
            if(GameObject.Find("BrainFlow")) DestroyImmediate(GameObject.Find("BrainFlow"));
            gameObject.name = "BrainFlow";
#if UNITY_EDITOR
            EditorApplication.isPlaying = true;
#endif
            brainFlowSessionProfile = sessionProfile;
        }

        void Start()
        {
            var newSessionGameObject = new GameObject();
            newSessionGameObject.transform.SetParent(transform);
            newSessionGameObject.AddComponent<BrainFlowSessionManager>().StartSession(brainFlowSessionProfile);
        }
    }
}