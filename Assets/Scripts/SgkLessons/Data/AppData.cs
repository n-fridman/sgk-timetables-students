using System;
using System.Collections.Generic;
using GoToApps.Debug;
using GoToApps.Debug.Data;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SgkLessons.Data
{
    [System.Serializable]
    public struct AppData
    {
        public bool isSelectedGroup;
        public Group selectedGroup;
        public Date currentDate;
        public List<Timetable> savedTimetables;
        
        public string GetDateForRequest()
        {
            return $"{currentDate.year}-{currentDate.month}-{currentDate.day}";
        }

        public string GetDateForTimetable()
        {
            DateTime dt = DateTime.Now;

            this.currentDate.year = dt.Year.ToString();

            if (dt.Month < 10) this.currentDate.month =  "0" + dt.Month;
            else this.currentDate.month = dt.Month.ToString();
            
            this.currentDate.day = dt.Day.ToString();

            if (dt.Day < 10) this.currentDate.day = "0" + dt.Day;
            else this.currentDate.day = dt.Day.ToString();

            DebugManager debugManager = Object.FindObjectOfType<DebugManager>();
            DebugRecord record = new DebugRecord {
                moduleName = "Data Log",
                className = "AppData",
                methodName = "GetDateForTimetable",
                message = $" Current date: {currentDate.day}.{currentDate.month}.{currentDate.year}",
                    
                type = RecordType.Trace,
                color = DebugColors.Lime
            };
            debugManager.LogRecord(record);
            
            return $"{currentDate.day}.{currentDate.month}.{currentDate.year}";
        }
    }
}
