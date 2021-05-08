using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GoToApps.Debug;
using GoToApps.Debug.Data;
using SgkLessons.Core;
using SgkLessons.Data;
using SgkLessons.Managers;
using UnityEngine;
using UnityEngine.Networking;

namespace SgkLessons
{
    public class DataLoader : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private AppManager app;
        [SerializeField] private DebugManager debugManager;
        
        [Header("Settings")]
        [SerializeField] private string groupsUrl;
        [SerializeField] private string scheduleUrl;

        [System.Serializable]
        private struct GroupsResponse
        {
            public List<Group> groups;
        }

        [System.Serializable]
        private struct DatesResponse
        {
            public List<string> dates;
        }

        /// <summary>
        /// Load group list from server.
        /// </summary>
        /// <param name="onSuccess">Success loaded callback.</param>
        /// <param name="onError">Error callback.</param>
        /// <returns></returns>
        public IEnumerator LoadGroups(Action<List<Group>> onSuccess, Action<UnityWebRequest> onError)
        {
            UnityWebRequest request = UnityWebRequest.Get(groupsUrl);
            
            DebugRecord record = new DebugRecord {
                moduleName = "Network Log",
                className = "DataLoader",
                methodName = "LoadGroups",
                message = "Send request to: " + request.url,
                    
                type = RecordType.Trace,
                color = DebugColors.Aqua
            };
            this.debugManager.LogRecord(record);
            
            yield return request.SendWebRequest();

            if (request.isNetworkError | request.isHttpError)
            {
                DebugRecord errorLogRecord = new DebugRecord {
                    moduleName = "Network Error",
                    className = "DataLoader",
                    methodName = "LoadGroups",
                    message = request.error,
                    
                    type = RecordType.Error,
                    color = DebugColors.Red
                };
                this.debugManager.LogRecord(errorLogRecord);
                
                onError(request);
                yield break;
            }

            string json = "{\"groups\": " + request.downloadHandler.text + " }";
            GroupsResponse response = JsonUtility.FromJson<GroupsResponse>(json);
            onSuccess(response.groups);

            app.onProgressChanged.Invoke(1, 1);
        }

        /// <summary>
        /// Load dates for group.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="date"></param>
        /// <param name="onSuccess">Success loaded callback.</param>
        /// <param name="onError">Error callback.</param>
        /// <returns></returns>
        public IEnumerator LoadDatesForGroup(Group group, Date date, Action<List<string>> onSuccess, Action<UnityWebRequest> onError)
        {
            UnityWebRequest request = UnityWebRequest.Get(this.scheduleUrl + group.index + "/" + "dates");
            
            DebugRecord requestRecord = new DebugRecord {
                moduleName = "Network Log",
                className = "DataLoader",
                methodName = "LoadDatesForGroup",
                message = "Send request to: " + request.url,
                    
                type = RecordType.Trace,
                color = DebugColors.Aqua
            };
            this.debugManager.LogRecord(requestRecord);
            
            yield return request.SendWebRequest();

            if (request.isNetworkError | request.isHttpError)
            {
                DebugRecord errorLogRecord = new DebugRecord {
                    moduleName = "Network Error",
                    className = "DataLoader",
                    methodName = "LoadDatesForGroup",
                    message = request.error,
                    
                    type = RecordType.Error,
                    color = DebugColors.Red
                };
                this.debugManager.LogRecord(errorLogRecord);
                
                onError(request);
                yield break;
            }

            string json = request.downloadHandler.text;
            DatesResponse response = JsonUtility.FromJson<DatesResponse>(json);

            List<string> dates = response.dates;
            string nowDate = $"{date.year}-{date.month}-{date.day}";
            List<string> activeDates = new List<string>();

            int i = 0;
            foreach (string stringDate in dates)
            {
                Date fromResponseDate = new Date{
                  day = stringDate.Split('-')[2],
                  month = stringDate.Split('-')[1],
                  year = stringDate.Split('-')[0]
                };

                if (int.Parse(date.month) > int.Parse(fromResponseDate.month))
                {
                    break;
                }
                
                if (stringDate == nowDate)
                {
                    activeDates.Add(stringDate);
                    break;
                }
                
                if (Int32.Parse(fromResponseDate.month) <= Int32.Parse(date.month)) {
                  if (Int32.Parse(fromResponseDate.day) < Int32.Parse(date.day)) break;
                }

                activeDates.Add(stringDate);
                i++;
                
                if (i > 10) break;
            }

            onSuccess(activeDates);
        }

        public IEnumerator LoadTimetablesForDates(Group group, List<string> dates, Action<List<Timetable>> onSuccess, Action<UnityWebRequest> onError)
        {
            int i = 0;
            List<Timetable> timetables = new List<Timetable>();
            foreach (string date in dates)
            {
                UnityWebRequest request = UnityWebRequest.Get(this.scheduleUrl + group.index + "/" + date);
                
                DebugRecord requestRecord = new DebugRecord {
                    moduleName = "Network Log",
                    className = "DataLoader",
                    methodName = "LoadTimetablesForDates",
                    message = "Send request to: " + request.url,
                    
                    type = RecordType.Trace,
                    color = DebugColors.Aqua
                };
                this.debugManager.LogRecord(requestRecord);
                yield return request.SendWebRequest();
                i++;
                this.app.onProgressChanged.Invoke(i, dates.Count);

                if (request.isNetworkError | request.isHttpError)
                {
                    DebugRecord errorLogRecord = new DebugRecord {
                        moduleName = "Network Error",
                        className = "DataLoader",
                        methodName = "LoadTimetablesForDates",
                        message = request.error,
                    
                        type = RecordType.Error,
                        color = DebugColors.Red
                    };
                    this.debugManager.LogRecord(errorLogRecord);
                    
                    onError(request);
                    yield break;
                }

                string json = request.downloadHandler.text;
                Timetable timetable = JsonUtility.FromJson<Timetable>(json);

                if (timetable.lessons.Count == 0) {
                    DebugRecord errorLogRecord = new DebugRecord {
                        moduleName = "Server Error",
                        className = "DataLoader",
                        methodName = "LoadTimetablesForDates",
                        message = "Null response from: " + request.url,
                    
                        type = RecordType.Error,
                        color = DebugColors.Red
                    };
                    this.debugManager.LogRecord(errorLogRecord);
                    
                    continue;
                }
                
                timetables.Add(timetable);
            }

            onSuccess(timetables);
        }
    }
}
