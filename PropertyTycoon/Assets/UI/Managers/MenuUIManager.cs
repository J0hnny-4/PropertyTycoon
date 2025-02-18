using UI.Screens.Menu;

namespace UI.Managers
{
    /// <summary>
    /// Implementation of BaseUIManager used in the menu.
    /// </summary>
    public class MenuUIManager : BaseUIManager
    {
        protected override void Awake()
        {
            base.Awake();
            NavigateTo<MainMenuScreen>();
        }
    }
}