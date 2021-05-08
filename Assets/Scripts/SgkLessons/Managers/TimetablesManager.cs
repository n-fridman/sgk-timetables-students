using System.Collections.Generic;
using SgkLessons.Behaviours;
using SgkLessons.Data;
using SgkLessons.Presenters;
using SgkLessons.Core;
using UnityEngine;

namespace SgkLessons.Managers
{
    public class TimetablesManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ListPrinter timetablesPrinter;
        [SerializeField] private GameObject timetablePrefab;
        [SerializeField] private GameObject groupHeaderPrefab;
        [SerializeField] private GameObject noInternetPrefab;
        [SerializeField] private AppCore core;

        public void DrawLocalLoadMarker()
        {
            this.timetablesPrinter.Add(noInternetPrefab);
        }
        
        /// <summary>
        /// Draw timetables list.
        /// </summary>
        public void DrawTimetables(List<Timetable> timetables)
        {

            GameObject groupHeaderGm = this.timetablesPrinter.Add(this.groupHeaderPrefab);
            GroupTitlePresenter groupTitlePresenter = groupHeaderGm.GetComponent<GroupTitlePresenter>();
            Group selectedGroup = this.core.GetGroup();
            groupTitlePresenter.DrawGroupTitle(selectedGroup);

            foreach (Timetable timetable in timetables)
            {
                GameObject timetableGm = this.timetablesPrinter.Add(this.timetablePrefab);
                TimetablePresenter presenter = timetableGm.GetComponent<TimetablePresenter>();
                presenter.DrawTimetable(timetable);
                AppData data = FindObjectOfType<AppManager>().GetAppData();
                if (timetable.date == data.GetDateForTimetable())
                {
                    presenter.MarkTimetableForActive();
                }
            }

            this.timetablesPrinter.UpdateHeight();
        }
    }
}
