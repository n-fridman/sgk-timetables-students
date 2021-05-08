using System;
using UnityEngine;
using UnityEngine.UI;

namespace SgkLessons.Managers
{
    public class SplashScreenManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Image progressBar;
        [SerializeField] private Text loadingText;
        [SerializeField] private Text authorText;

        private void Awake()
        {
            this.authorText.text = "n.fridman | " + Application.version;
        }

        /// <summary>
        /// Draw progress bar value.
        /// </summary>
        /// <param name="value"></param>
        public void DrawProgressBar(float value) => this.progressBar.fillAmount = value;

        /// <summary>
        /// Draw error message.
        /// </summary>
        /// <param name="error"></param>
        public void DrawError(string error) => this.loadingText.text = error;
    }
}
