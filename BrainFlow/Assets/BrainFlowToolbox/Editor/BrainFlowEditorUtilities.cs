using BrainFlowToolbox.Runtime;
using BrainFlowToolbox.ScriptableObjects;
using BrainFlowToolbox.Utilities;
using UnityEditor;
using UnityEngine;

namespace BrainFlowToolbox.Editor
{
    public static class BrainFlowEditorUtilities
    {
        private static GameObject brainFlowManagerGameObject;
        private static BrainFlowManager brainFlowManager;
        public static void StartBrainFlowSession(BrainFlowSessionProfile sessionProfile)
        {
            SaveSessionChanges(sessionProfile);
            brainFlowManagerGameObject = new GameObject();
            brainFlowManager = brainFlowManagerGameObject.AddComponent<BrainFlowManager>();
            brainFlowManager.StartSession(sessionProfile);
        }
        
        public static void SaveSessionChanges(BrainFlowSessionProfile sessionProfile)
        {
            if (sessionProfile == null ) return;
            Debug.Log(sessionProfile.sessionName + " Changes Saved");

            EditorUtility.SetDirty(sessionProfile);
            sessionProfile.name = sessionProfile.sessionName;
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(sessionProfile), sessionProfile.sessionName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        public static void DeleteSession(BrainFlowSessionProfile sessionProfile)
        {
            if (!EditorUtility.DisplayDialog("Warning!",
                "Are you sure you want to delete " + sessionProfile.name + "?", "Yes", "No")) return;

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(sessionProfile));
        }

    }
}