using System.Collections.Generic;
using UnityEngine;

namespace SgkLessons.Managers
{
    public class ScreenManager : MonoBehaviour
    {
        [System.Serializable]
        public struct Screen
        {
            public string name;
            public GameObject gameObject;
        }

        [Header("Settings")] 
        [SerializeField] private List<Screen> screens;
        
        /// <summary>
        /// Open screen.
        /// </summary>
        /// <param name="screenName">Screen name.</param>
        public void Open(string screenName)
        {
            Screen findScreen = this.screens.Find(s => s.name == screenName);

            foreach (Screen screen in screens)
            {
                screen.gameObject.SetActive(false);
            }
            
            findScreen.gameObject.SetActive(true);
        }
    }
}
