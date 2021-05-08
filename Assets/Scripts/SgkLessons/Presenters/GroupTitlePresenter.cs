using System.Collections.Generic;
using SgkLessons.Behaviours;
using SgkLessons.Data;
using SgkLessons.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SgkLessons.Presenters
{
    public class GroupTitlePresenter : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Text titleText;

        /// <summary>
        /// Draw timetables list.
        /// </summary>
        public void DrawGroupTitle(Group group)
        {
          this.titleText.text = group.name;
        }
    }
}
