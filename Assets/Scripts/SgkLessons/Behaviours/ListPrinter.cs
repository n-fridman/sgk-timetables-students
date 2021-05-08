using System.Collections.Generic;
using UnityEngine;

namespace SgkLessons.Behaviours
{
    public class ListPrinter : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private Transform parentTransform;
        [SerializeField] private RectTransform parentRectTransform;

        [Header("Settings")] 
        [SerializeField] private List<GameObject> items;
        [SerializeField] private float offset;

        public void Clear()
        {
            foreach (GameObject item in items)
            {
                Destroy(item);
            }
            UpdateHeight();
        }
        
        /// <summary>
        /// Update container height.
        /// </summary>
        public void UpdateHeight()
        {
            float height = 0;
            foreach (GameObject item in items)
            {
                ListItem listItem = item.GetComponent<ListItem>();
                height += listItem.Height() + this.offset;
            }
            this.parentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
        
        /// <summary>
        /// Add element to list.
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public GameObject Add(GameObject prefab)
        {
            GameObject instantiatedListItem = Instantiate(prefab, this.parentTransform);
            this.items.Add(instantiatedListItem);
            return instantiatedListItem;
        }
    }
}
