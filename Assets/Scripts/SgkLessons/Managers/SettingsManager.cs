using System.Collections.Generic;
using SgkLessons.Behaviours;
using SgkLessons.Data;
using SgkLessons.Presenters;
using UnityEngine;

namespace SgkLessons.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private ListPrinter groupsPrinter;
        [SerializeField] private GameObject quitButton;

        [Header("Settings")]
        [SerializeField] private GameObject groupItemPrefab;
        [SerializeField] private List<Group> groups;
        [SerializeField] private List<GameObject> drawingGroups;
        
        public void ShowButton() => this.quitButton.SetActive(true);

        public void HideButton() => this.quitButton.SetActive(false);

        public void SetGroups(List<Group> groups) => this.groups = groups;

        public void Search(string match)
        {
            ClearList();
        }

        private void ClearList() => this.groupsPrinter.Clear();

        /// <summary>
        /// Draw group list.
        /// </summary>
        /// <param name="groups"></param>
        public void DrawGroups(List<Group> groups)
        {
            foreach (Group group in groups)
            {
                GameObject groupGm = this.groupsPrinter.Add(this.groupItemPrefab);
                GroupPresenter groupPresenter = groupGm.GetComponent<GroupPresenter>();
                groupPresenter.DrawGroup(group);
            }
            this.groupsPrinter.UpdateHeight();
        }
    }
}
