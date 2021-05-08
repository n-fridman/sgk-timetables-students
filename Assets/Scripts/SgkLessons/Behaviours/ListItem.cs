using UnityEngine;

namespace SgkLessons.Behaviours
{
    public class ListItem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform rectTransform;
        
        /// <summary>
        /// Return element height.
        /// </summary>
        /// <returns>Height (float)</returns>
        public float Height() => this.rectTransform.rect.height;
    }
}