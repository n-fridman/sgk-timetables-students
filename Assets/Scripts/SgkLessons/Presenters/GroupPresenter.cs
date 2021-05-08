using System;
using SgkLessons.Behaviours;
using SgkLessons.Core;
using SgkLessons.Data;
using UnityEngine;
using UnityEngine.UI;

namespace SgkLessons.Presenters
{
    public class GroupPresenter : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private Text groupName;
        [SerializeField] private Button actionButton;
        [SerializeField] private AppCore core;
        [SerializeField] private AppColors colorsManager;
        [SerializeField] private Image icon;
        
        private void Awake()
        {
            this.core = FindObjectOfType<AppCore>();
        }

        /// <summary>
        /// Draw group item fields.
        /// </summary>
        public void DrawGroup(Group group)
        {
            this.groupName.text = group.name;
            this.colorsManager = FindObjectOfType<AppColors>();
            this.icon.color = this.colorsManager.GetRandomColor();
            
            this.actionButton.onClick.AddListener(() =>
            {
                this.core.SetGroup(group);
            });
        }
    }
}