using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Acts as a link between all screens in the scene.
    /// </summary>
    public abstract class BaseUIManager<T> : MonoBehaviour
    {
        [SerializeField] protected T defaultScreen;
        protected BaseScreen<T> CurrentScreen;
        protected Dictionary<T, BaseScreen<T>> Screens;

        /// <summary>
        /// Base class registers screens on awake. Additional functionality to be implemented by concrete derivation. 
        /// </summary>
        protected virtual void Awake()
        {
            RegisterScreens();
        }

        /// <summary>
        /// Gets all screens for which this manager is responsible for and initialises them.<br/>
        /// NOTE: screens should be present in the scene as children of this GameObject.
        /// </summary>
        private void RegisterScreens()
        {
            Screens = new Dictionary<T, BaseScreen<T>>();
            foreach (var screen in GetComponentsInChildren<BaseScreen<T>>())
            {
                screen.BaseSetup(this);
                screen.Initialise();
                Screens.Add(screen.GetScreenType(), screen);
                screen.Hide();
            }
        }

        /// <summary>
        /// Navigate to a new screen: current screen is hidden, new screen is shown.
        /// </summary>
        /// <param name="screenType">The type of the screen to navigate to.</param>`
        public void NavigateTo(T screenType)
        {
            CurrentScreen?.Hide();
            Screens.TryGetValue(screenType, out var screen);
            Debug.Assert(screen != null, $"{screenType} not found."); // debug
            screen.Show();
            CurrentScreen = screen;
        }
    }
}