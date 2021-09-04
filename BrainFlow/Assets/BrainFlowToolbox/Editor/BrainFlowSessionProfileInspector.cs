using System;
using brainflow;
using BrainFlowToolbox.Runtime.DataModels.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace BrainFlowToolbox.Editor
{
    [CustomEditor(typeof(BrainFlowSessionProfile))]
    public class BrainFlowSessionProfileInspector : UnityEditor.Editor
    {

        private BrainFlowSessionProfile sessionProfile;
        private float slh;
        
        private void OnEnable()
        {
            slh = EditorGUIUtility.singleLineHeight;
            if (target == null) return;
            sessionProfile = (BrainFlowSessionProfile) target;
            sessionProfile.sessionName = sessionProfile.name;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
        
            var scale = EditorGUIUtility.currentViewWidth ;

            EditorGUILayout.LabelField(sessionProfile.sessionName,
                new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.UpperCenter,
                    wordWrap = true,
                    fontStyle = FontStyle.Bold,
                    fontSize = 30
                });
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(EditorApplication.isPlaying ? Resources.Load<Texture2D>("Icons/EndSession") : Resources.Load<Texture2D>("Icons/StartSession")),
                new GUIStyle {alignment = TextAnchor.MiddleCenter, padding = new RectOffset(2, 2, 2, 2)},
                GUILayout.Width(80), GUILayout.Height(80)))
            {
                if (EditorApplication.isPlaying)
                {
                    EditorApplication.isPlaying = false;
                }
                else
                {
                    BrainFlowEditorUtilities.StartBrainFlowSession(sessionProfile);
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(scale * 0.95f));
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sessionName"), new GUIContent("Name"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("board"));
            
            
            // Because each board requires different parameters to create the BoardShim we only expose
            // the required parameters in the GUI to make it easier to know which ones are required.
            switch (sessionProfile.board)
            {
                case BoardIds.PLAYBACK_FILE_BOARD:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("playbackFilePath"), 
                        new GUIContent("Playback File Path (.file)"));
                    break;
                case BoardIds.STREAMING_BOARD:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ipAddress"), 
                        new GUIContent("Multicast IP (.ip_address)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("ipPort"), 
                        new GUIContent("Multicast IP (.ip_port)"));
                    break;
                case BoardIds.SYNTHETIC_BOARD:
                    break;
                case BoardIds.CYTON_BOARD:
                    break;
                case BoardIds.GANGLION_BOARD:
                    break;
                case BoardIds.CYTON_DAISY_BOARD:
                    break;
                case BoardIds.GALEA_BOARD:
                    break;
                case BoardIds.GANGLION_WIFI_BOARD:
                    break;
                case BoardIds.CYTON_WIFI_BOARD:
                    break;
                case BoardIds.CYTON_DAISY_WIFI_BOARD:
                    break;
                case BoardIds.BRAINBIT_BOARD:
                    break;
                case BoardIds.UNICORN_BOARD:
                    break;
                case BoardIds.CALLIBRI_EEG_BOARD:
                    break;
                case BoardIds.CALLIBRI_EMG_BOARD:
                    break;
                case BoardIds.CALLIBRI_ECG_BOARD:
                    break;
                case BoardIds.FASCIA_BOARD:
                    break;
                case BoardIds.NOTION_1_BOARD:
                    break;
                case BoardIds.NOTION_2_BOARD:
                    break;
                case BoardIds.IRONBCI_BOARD:
                    break;
                case BoardIds.GFORCE_PRO_BOARD:
                    break;
                case BoardIds.FREEEEG32_BOARD:
                    break;
                case BoardIds.BRAINBIT_BLED_BOARD:
                    break;
                case BoardIds.GFORCE_DUAL_BOARD:
                    break;
                case BoardIds.GALEA_SERIAL_BOARD:
                    break;
                case BoardIds.MUSE_S_BLED_BOARD:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serialPortNumber"),
                        new GUIContent("Dongle Port # (.serial_port)", 
                            "Enter the number of the serial port only. If using COM3 simply enter 3"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("deviceSerialNumber"), 
                        new GUIContent("Device Name (.serial_number)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("deviceDiscoveryTimeout"));
                    break;
                case BoardIds.MUSE_2_BLED_BOARD:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("serialPortNumber"),
                        new GUIContent("DonglePort # (.serial_port)", 
                            "Enter the number of the serial port only. If using COM3 simply enter 3"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("deviceSerialNumber"), 
                        new GUIContent("Device Name (.serial_number)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("deviceDiscoveryTimeout"));
                    break;
                case BoardIds.CROWN_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_410_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_411_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_430_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_211_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_212_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_213_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_214_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_215_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_221_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_222_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_223_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_224_BOARD:
                    break;
                case BoardIds.ANT_NEURO_EE_225_BOARD:
                    break;
                case BoardIds.ENOPHONE_BOARD:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bufferSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("boardDataFileName"));
            
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("createDataDashboard"));
            if (sessionProfile.createDataDashboard)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("graphBackgroundColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("graphBarColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("graphLineColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("graphPointColor"));
            }
            
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

        }
    }
}