using System;
using System.Collections;
using SgkLessons.Behaviours;
using SgkLessons.Data;
using SgkLessons.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace SgkLessons.Presenters
{
    public class TimetablePresenter : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Text dateText;
        [SerializeField] private ListPrinter lessonPrinter;
        [SerializeField] private GameObject lessonPrefab;
        [SerializeField] private RectTransform timetableRectTransform;
        [SerializeField] private RectTransform lessonsRectTransform;
        [SerializeField] private GameObject activeMarker;
        [SerializeField] private Animator animator;
        [SerializeField] private AppManager app;
        [Space]
        [SerializeField] private bool activated = false;
        
        private static readonly int Active = Animator.StringToHash("Active");

        private void OnEnable()
        {
            if (activated) this.animator.SetTrigger(Active);
        }

        public void MarkTimetableForActive()
        {
            this.activated = true;
        }
        
        public void DrawTimetable(Timetable timetable)
        {
            this.app = FindObjectOfType<AppManager>();

            this.dateText.text = timetable.date;

            foreach (Lesson lesson in timetable.lessons)
            {
                GameObject lessonGm = this.lessonPrinter.Add(this.lessonPrefab);
                LessonPresenter presenter = lessonGm.GetComponent<LessonPresenter>();
                presenter.DrawLesson(lesson);
            }

            this.lessonPrinter.UpdateHeight();
            float containerHeight = this.timetableRectTransform.rect.height;
            float timetableHeight = this.lessonsRectTransform.rect.height;
            this.timetableRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, containerHeight + timetableHeight);
        }
    }
}
