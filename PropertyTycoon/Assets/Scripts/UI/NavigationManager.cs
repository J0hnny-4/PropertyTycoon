using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Enum used to identify screens.
    /// </summary>
    public enum ScreenType {
        MainMenu,
        GameMode,
        PlayerSetup,
        Settings,
        MainGame,
        GameOver
    }
    
    /// <summary>
    /// Acts as a link between all screens in the scene.
    /// </summary>
    public class NavigationManager : MonoBehaviour
    {
        [SerializeField] private ScreenType defaultScreen;
        private ScreenType _currentScreen;
        private Dictionary<ScreenType, BaseScreen> _screens;

        /// <summary>
        /// On awake, screens are initialised and registered.
        /// </summary>
        protected void Awake()
        {
            RegisterScreens();
            NavigateTo(defaultScreen);
        }

        /// <summary>
        /// Gets all screens for which this manager is responsible for and initialises them.<br/>
        /// NOTE: screens should be present in the scene as children of this GameObject.
        /// </summary>
        private void RegisterScreens()
        {
            _screens = new Dictionary<ScreenType, BaseScreen>();
            foreach (var screen in GetComponentsInChildren<BaseScreen>())
            {
                screen.BaseSetup(this);
                screen.Initialise();
                _screens.Add(screen.GetScreenType(), screen);
                screen.Hide();
            }
        }

        /// <summary>
        /// Navigate to a new screen: current screen is hidden, new screen is shown.
        /// </summary>
        /// <param name="newScreenType">The type of the screen to navigate to.</param>`
        public void NavigateTo(ScreenType newScreenType)
        {
            // hides current screen
            _screens.TryGetValue(_currentScreen, out var currentScreen);
            currentScreen?.Hide();
            
            // shows new screen
            _screens.TryGetValue(newScreenType, out var newScreen);
            Debug.Assert(newScreen != null, $"{newScreenType} not found."); // debug
            newScreen.Show();
            _currentScreen = newScreenType;
        }
    }
}