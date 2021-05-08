using System;
using System.Collections.Generic;
using System.IO;
using GoToApps.Debug.Data;
using UnityEngine;

namespace GoToApps.Debug
{
    [AddComponentMenu("GoTo-Apps/Debug/Debug Manager")]
    public class DebugManager : MonoBehaviour
    {
        private readonly Queue<DebugRecord> _recordsQueue = new Queue<DebugRecord>();
        
        [Header("Settings")] 
        public bool writeLogInFile;
        public string logsDirName;
        public string logsFilename;
        public bool splitLogFilesByLayers;
        
        private string _logsPath;
        private string _logsDirPath;

        private void Awake()
        {
#if UNITY_EDITOR
            this._logsDirPath = Path.Combine(Application.dataPath, this.logsDirName);
            this._logsPath = Path.Combine(Application.dataPath, this.logsDirName, this.logsFilename);
#elif UNITY_ANDROID
            this._logsDirPath = Path.Combine(Application.persistentDataPath, this.logsDirName);
            this._logsPath = Path.Combine(Application.persistentDataPath, this.logsDirName, this.logsFilename);
#endif
            
            this.LogRecord(new DebugRecord
            {
                moduleName = "Self System",
                className = "DebugManager",
                methodName = "Awake",
                message = "App Started :---------------------------------------------------------------------------------------------",
                
                type = RecordType.System,
                color = DebugColors.Lime
            });
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            this.LogRecord(new DebugRecord
            {
                moduleName = "Self System",
                className = "DebugManager",
                methodName = "OnApplicationPause",
                message = "App Pause :----------------------------------------------------------------------------------",
                
                type = RecordType.System,
                color = DebugColors.Lime
            });
        }

        private void OnApplicationQuit()
        {
            this.LogRecord(new DebugRecord
            {
                moduleName = "Self System",
                className = "DebugManager",
                methodName = "OnApplicationQuit",
                message = "App Closed :----------------------------------------------------------------------------------",
                
                type = RecordType.System,
                color = DebugColors.Lime
            });
        }

        /// <summary>
        /// Return color for log type.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <returns></returns>
        private string GetColorForLogLayer(RecordType type)
        {
            switch (type)
            {
                case RecordType.Trace: return "green";
                case RecordType.Debug: return "yellow";
                case RecordType.Info: return "blue";
                case RecordType.Warning: return "orange";
                case RecordType.Error: return "red";
                case RecordType.System: return "lime";
                default: return "gray";
            }
        }
        
        /// <summary>
        /// Return name for log type.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <returns></returns>
        private string GetNameForLogLayer(RecordType type)
        {
            switch (type)
            {
                case RecordType.Trace: return "TRACE";
                case RecordType.Debug: return "DEBUG";
                case RecordType.Info: return "INFO";
                case RecordType.Warning: return "WARNING";
                case RecordType.Error: return "ERROR";
                case RecordType.System: return "SYSTEM";
                default: return "NONE";
            }
        }
        
        /// <summary>
        /// Return log string for Unity editor.
        /// </summary>
        /// <param name="record">Log record.</param>
        /// <returns></returns>
        private string BuildLogStringForUnity(DebugRecord record) => $"[<color={GetColorForLogLayer(record.type)}><b>{GetNameForLogLayer(record.type)}</b></color>] {{<color={record.color}>{record.moduleName}</color>}} => [{record.className}] - (<color=yellow>{record.methodName}</color>) -> {record.message}";
        
        /// <summary>
        /// Return log string for file.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private string BuildLogStringToFile(DebugRecord record)
        {
            DateTime dt = DateTime.Now;
            TimeSpan ts = dt.TimeOfDay;
            string time = $"{dt.Year}.{dt.Month}.{dt.Day} {ts.Hours}:{ts.Minutes}:{ts.Seconds}.{ts.Milliseconds}";
            return $"[{time}] [{GetNameForLogLayer(record.type)}] {{{record.moduleName}}} => [{record.className}] - ({record.methodName}) -> {record.message}";
        }
        
        /// <summary>
        /// Write log string to file.
        /// </summary>
        /// <param name="record">Log record.</param>
        private void WriteToFile(DebugRecord record)
        {
            string logFileName = this.logsFilename;
            if (this.splitLogFilesByLayers) logFileName = $"{this.logsFilename}.{GetNameForLogLayer(record.type).ToLower()}.log";
            string path = Path.Combine(this._logsDirPath, logFileName);
            
            if (_recordsQueue.Count != 0)
            {
                foreach (DebugRecord recordFromQueue in _recordsQueue)
                {
                    File.AppendAllText(path, $" {BuildLogStringToFile(recordFromQueue)} \n");
                }
            }
            File.AppendAllText(path,  $"{BuildLogStringToFile(record)} \n");
        }        
        
        /// <summary>
        /// Log record.
        /// </summary>
        /// <param name="record"></param>
        public void LogRecord(DebugRecord record)
        {
            string logStrForUnity = BuildLogStringForUnity(record);
            UnityEngine.Debug.Log(logStrForUnity);
            
            if (this.writeLogInFile)
            {
                if (this._logsPath == null | this._logsDirPath == null)
                {
                    this._recordsQueue.Enqueue(record);
                    return;
                }
                
                if (Directory.Exists(this._logsDirPath) == false)
                {
                    Directory.CreateDirectory(this._logsDirPath);
                }
                
                WriteToFile(record);
            }
        }
    }
}