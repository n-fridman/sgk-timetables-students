using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace GoToApps.Debug.Editor
{
    [CustomEditor(typeof(DebugManager))]
    public class DebugManagerEditor : UnityEditor.Editor
    {
        private DebugManager model;
        private SerializedProperty _writeLogsInFile;
        private SerializedProperty _logsDirName;
        private SerializedProperty _logsFileName;
        private SerializedProperty _splitLogFilesByLayers;
        
        private void OnEnable()
        {
            this.model = (DebugManager) this.target;

            this._writeLogsInFile = this.serializedObject.FindProperty(nameof(this.model.writeLogInFile));
            this._splitLogFilesByLayers = this.serializedObject.FindProperty(nameof(this.model.splitLogFilesByLayers));
            this._logsDirName = this.serializedObject.FindProperty(nameof(this.model.logsDirName));
            this._logsFileName = this.serializedObject.FindProperty(nameof(this.model.logsFilename));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(this._writeLogsInFile);
            if (this._writeLogsInFile.boolValue)
            {
                EditorGUILayout.PropertyField(this._splitLogFilesByLayers);
                EditorGUILayout.PropertyField(this._logsDirName);
                EditorGUILayout.PropertyField(this._logsFileName);
            }
            
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed) OnChanged(this.model.gameObject);
        }
        
        private static void OnChanged(GameObject obj)
        {
            if (Application.isPlaying == false)
            {
                EditorUtility.SetDirty(obj);
                EditorSceneManager.MarkSceneDirty(obj.scene);
            }
        }
    }
}