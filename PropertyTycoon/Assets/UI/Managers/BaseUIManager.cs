using System;
using System.Collections.Generic;
using UI.Screens;
using UnityEngine;

namespace UI.Managers
{
    /// <summary>
    /// Acts as a link between all screens in the scene.
    /// </summary>
    public abstract class BaseUIManager : MonoBehaviour
    {
        protected BaseScreen CurrentScreen;
        protected Dictionary<Type, BaseScreen> Screens;
        
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
            Screens = new Dictionary<Type, BaseScreen>();
            foreach (BaseScreen screen in GetComponentsInChildren<BaseScreen>())
            {
                screen.Initialise();
                Screens.Add(screen.GetType(), screen);
            }
        }
        
        /// <summary>
        /// Navigate to a new screen: current screen is hidden, new screen is shown.
        /// </summary>
        /// <typeparam name="T">The type of the screen to navigate to.</typeparam>
        public void NavigateTo<T>() where T : BaseScreen
        {
            CurrentScreen?.Hide();
            Screens.TryGetValue(typeof(T), out BaseScreen screen);
            Debug.Assert(screen != null, nameof(screen) + " != null");
            screen.Show();
            CurrentScreen = screen;
        }
    }
}