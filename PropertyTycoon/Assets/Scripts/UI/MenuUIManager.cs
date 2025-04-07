namespace UI
{
    /// <summary>
    /// Enum used to identify screens belonging to the menu scene.
    /// Useful for testing.
    /// </summary>
    public enum MenuScreen {
        MainMenu,
        GameMode,
        PlayerSetup,
        Settings,
    }

    /// <summary>
    /// Implementation of BaseUIManager used in the menu.
    /// </summary>
    public class MenuUIManager : BaseUIManager<MenuScreen>
    {
        protected override void Awake()
        {
            base.Awake();
            NavigateTo(defaultScreen);
        }
    }
}