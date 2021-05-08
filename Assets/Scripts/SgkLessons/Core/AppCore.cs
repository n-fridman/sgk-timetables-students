using System;
using SgkLessons.Data;
using SgkLessons.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using GoToApps.Debug;
using GoToApps.Debug.Data;

namespace SgkLessons.Core
{
    public class AppCore : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private AppManager appManager;
        [SerializeField] private DebugManager debugManager;
        
        [Header("Settings")]
        [SerializeField] private string dataSaveKey;
        [SerializeField] private AppData data;

        private void Awake()
        {
            if (PlayerPrefs.HasKey(this.dataSaveKey))
            {
                string json = PlayerPrefs.GetString(this.dataSaveKey);
                AppData dataFromJson = JsonUtility.FromJson<AppData>(json);
                this.data = dataFromJson;

                DebugRecord record = new DebugRecord
                {
                    moduleName = "App Log",
                    className = "AppCore",
                    methodName = "Awake",
                    message = "Loaded group: " + this.data.selectedGroup.name,
                    
                    type = RecordType.Info,
                    color = DebugColors.Magenta
                };
                
                this.debugManager.LogRecord(record);
            }else {
                
                DebugRecord record = new DebugRecord
                {
                    moduleName = "App Log",
                    className = "AppCore",
                    methodName = "Awake",
                    message = "Saved group not loaded",
                    
                    type = RecordType.Info,
                    color = DebugColors.Magenta
                };
                
                this.debugManager.LogRecord(record);
            }
        }

        private void Start()
        {
            this.appManager.LoadApp(this.data);
        }

        /// <summary>
        /// Set active group data.
        /// </summary>
        /// <param name="group">Selected group)</param>
        public void SetGroup(Group group)
        {
            this.data.selectedGroup = group;
            this.data.isSelectedGroup = true;

            string json = JsonUtility.ToJson(this.data);
            PlayerPrefs.SetString(this.dataSaveKey, json);
            
            DebugRecord record = new DebugRecord
            {
                moduleName = "App Log",
                className = "AppCore",
                methodName = "SetGroup",
                message = "Selected group: " + this.data.selectedGroup.name,
                    
                type = RecordType.Info,
                color = DebugColors.Magenta
            };
            
            this.debugManager.LogRecord(record);
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnApplicationQuit()
        {
            string json = JsonUtility.ToJson(this.data);
            PlayerPrefs.SetString(this.dataSaveKey, json);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            string json = JsonUtility.ToJson(this.data);
            PlayerPrefs.SetString(this.dataSaveKey, json);
        }

        public Group GetGroup() => this.data.selectedGroup;
        
        public void SaveTimetables(List<Timetable> timetables)
        {
            this.data.savedTimetables = timetables;
        }
    }
}
