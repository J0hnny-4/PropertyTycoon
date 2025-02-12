using System;
using UI.Screens;
using UnityEngine;

namespace UI.Managers
{
    /// <summary>
    /// Implementation of BaseUIManager used in the menu.
    /// </summary>
    public class MenuUIManager : BaseUIManager
    {
        /// <summary>
        /// Navigate to a new screen: current screen is hidden, new screen is shown.
        /// </summary>
        /// <param name="screenType">Type (class name) of the screen to navigate to.</param>
        public void NavigateTo(Type screenType)
        {
            CurrentScreen?.Hide();
            Screens.TryGetValue(screenType, out BaseScreen screen);
            Debug.Assert(screen != null, nameof(screen) + " != null");
            screen.Show();
            CurrentScreen = screen;
        }
    }
}