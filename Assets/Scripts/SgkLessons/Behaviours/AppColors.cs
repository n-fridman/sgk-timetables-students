using UnityEngine;
using System.Collections.Generic;

namespace SgkLessons.Behaviours
{
    public class AppColors : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private List<Color> colors;

        public Color GetRandomColor()
        {
            int index = Random.Range(0, this.colors.Count);
            return this.colors[index];
        }
    }
}
