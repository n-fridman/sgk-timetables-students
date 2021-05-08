using SgkLessons.Behaviours;
using SgkLessons.Data;
using UnityEngine;
using UnityEngine.UI;

namespace SgkLessons.Presenters
{
    public class LessonPresenter : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Text teacherNameText;
        [SerializeField] private Text lessonNumText;
        [SerializeField] private Text lessonNameText;
        [SerializeField] private Text lessonRoomText;
        [SerializeField] private Image icon;
        [Space] 
        [SerializeField] private AppColors colorsManager;
        
        /// <summary>
        /// Draw lesson fields.
        /// </summary>
        /// <param name="lesson">Lesson data to draw.</param>
        public void DrawLesson(Lesson lesson)
        {
            this.teacherNameText.text = lesson.teachername;
            this.lessonNumText.text = lesson.num;
            this.lessonNameText.text = lesson.title;
            this.lessonRoomText.text = lesson.cab;

            this.colorsManager = FindObjectOfType<AppColors>();
            this.icon.color = this.colorsManager.GetRandomColor();
        }
    }
}
