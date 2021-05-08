using System;
using System.Collections;
using System.Collections.Generic;
using GoToApps.Debug;
using GoToApps.Debug.Data;
using SgkLessons.Core;
using SgkLessons.Data;
using UnityEngine;
using UnityEngine.Events;

namespace SgkLessons.Managers
{
    public class AppManager : MonoBehaviour
    {
        [Header("Core")] 
        [SerializeField] private AppCore core;
        
        [Header("Managers")]
        [SerializeField] private ScreenManager screenManager;
        [SerializeField] private SettingsManager settingsManager;
        [SerializeField] private TimetablesManager timetablesManager;
        [SerializeField] private SplashScreenManager splashScreenManager;
        [SerializeField] private DebugManager debugManager;
        
        [Header("Behaviours")]
        [SerializeField] private DataLoader dataLoader;

        [HideInInspector] public UnityEvent<float, float> onProgressChanged;
        [HideInInspector] public UnityEvent<List<Group>> onGroupsLoaded;
        [HideInInspector] public UnityEvent<string> onRequestError;
        [HideInInspector] public UnityEvent<List<Timetable>> onTimetablesLoaded;

        [Header("Settings")]
        [SerializeField] private AppData data;

        private void Awake()
        {
            onRequestError.AddListener(error =>
                this.splashScreenManager.DrawError(error));

            onProgressChanged.AddListener((value, maxValue) =>
                this.splashScreenManager.DrawProgressBar(value / maxValue));

            onGroupsLoaded.AddListener(groups =>
            {
                this.settingsManager.DrawGroups(groups);
                this.settingsManager.SetGroups(groups);
            });

            onTimetablesLoaded.AddListener(timetables => {
                this.core.SaveTimetables(timetables);
                this.timetablesManager.DrawTimetables(timetables);
                this.screenManager.Open("Timetables");
            });
        }

        /// <summary>
        /// Draw request error screen.
        /// </summary>
        private void DrawError()
        {
            this.screenManager.Open("Splashscreen");
            this.onRequestError.Invoke("Во время загрузки данных произошла ошибка...");
            this.onProgressChanged.Invoke(0, 1);
        }

        public AppData GetAppData() => this.data;

        /// <summary>
        /// Load application.
        /// </summary>
        /// <param name="data"></param>
        public void LoadApp(AppData data)
        {
            this.data = data;

            DateTime dateTimeNow = DateTime.Now;

            if (data.isSelectedGroup == false){
                IEnumerator groupsLoader = this.dataLoader.LoadGroups(
                    groups => {
                      this.onGroupsLoaded.Invoke(groups);
                      this.settingsManager.HideButton();
                      this.screenManager.Open("Settings");
                      },
                    errorRequest => DrawError());

                StartCoroutine(groupsLoader);
            } else {
                
                this.settingsManager.ShowButton();
                
                IEnumerator groupsLoader = this.dataLoader.LoadGroups(
                  groups => {
                    this.onGroupsLoaded.Invoke(groups);
                    },
                  errorRequest =>
                  {
                      if (data.isSelectedGroup & data.savedTimetables.Count != 0)
                      {
                          this.timetablesManager.DrawLocalLoadMarker();
                          this.onTimetablesLoaded.Invoke(data.savedTimetables);
                          
                          DebugRecord record = new DebugRecord
                          {
                              moduleName = "App Log",
                              className = "AppManager",
                              methodName = "LoadApp",
                              message = "Timetables successfully loaded from local drive.",
                    
                              type = RecordType.Info,
                              color = DebugColors.Orange
                          };
                          
                          this.debugManager.LogRecord(record);
                      }
                  });

              StartCoroutine(groupsLoader);

                int month = dateTimeNow.Month;
                string monthForRequest = "";
                if (month < 10)
                {
                    monthForRequest = "0" + month;
                }

                Date date = new Date {
                    year = dateTimeNow.Year.ToString(),
                    month = monthForRequest,
                    day = dateTimeNow.Day.ToString()
                };

                IEnumerator datesLoader = this.dataLoader.LoadDatesForGroup(data.selectedGroup, date, dates => {
                    this.onProgressChanged.Invoke(0.3f, 1);

                    IEnumerator timetableLoader = this.dataLoader.LoadTimetablesForDates(data.selectedGroup, dates, timetables =>
                        onTimetablesLoaded.Invoke(timetables),
                        errorRequest => {
                            if (data.isSelectedGroup == false & data.savedTimetables.Count == 0)
                            {
                                DebugRecord record = new DebugRecord
                                {
                                    moduleName = "App Log",
                                    className = "AppManager",
                                    methodName = "LoadApp",
                                    message = "Error drawing.",
                    
                                    type = RecordType.Info,
                                    color = DebugColors.Red
                                };
                          
                                this.debugManager.LogRecord(record);
                                DrawError();
                            }
                        });

                    StartCoroutine(timetableLoader);

                }, errorRequest =>
                {
                    if (data.isSelectedGroup == false & data.savedTimetables.Count == 0)
                    {
                        DrawError();
                    }
                });

                StartCoroutine(datesLoader);
            }
        }
    }
}
