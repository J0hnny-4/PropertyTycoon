namespace UI
{
    public enum MenuScreen {
        MainMenu,
        GameMode,
        PlayerSetup,
        Settings, 
        Dice, 
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