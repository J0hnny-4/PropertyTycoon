using System;
using System.Collections.Generic;
using UI.Screens;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Acts as a link between all screens in the scene.
    /// </summary>
    public abstract class BaseUIManager : MonoBehaviour
    {
        protected BaseScreen CurrentScreen;
        protected Dictionary<Type, BaseScreen> Screens;
    
        /// <summary>
        /// Gets all screens for which this manager is responsible for. They are present in the scene as children of this
        /// GameObject.
        /// </summary>
        public void RegisterScreens()
        {
            Screens = new Dictionary<Type, BaseScreen>();
            foreach (BaseScreen screen in GetComponentsInChildren<BaseScreen>())
            {
                screen.Initialise(this);
                Screens.Add(screen.GetType(), screen);
            }
        }
    }
}