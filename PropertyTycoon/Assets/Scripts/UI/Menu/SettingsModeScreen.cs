using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Menu
{
    /// <summary>
    /// Allows the user to change game settings.
    /// </summary>
    public class SettingsScreen : BaseScreen<MenuScreen>
    {
        private Button _backButton;
        private Slider _volumeSlider;
        private AudioSource _mixer;
        
        public override void Initialise()
        {
            _backButton = Root.Q<Button>("BackButton");
            _volumeSlider = Root.Q<Slider>("Volume");
            _mixer = GetComponent<AudioSource>();
            _backButton.RegisterCallback<ClickEvent>(OnBackButtonClicked);
            _volumeSlider.RegisterCallback<ClickEvent>(SetVolume);
        }
        
        protected override void CleanUp()
        {
            _backButton.UnregisterCallback<ClickEvent>(OnBackButtonClicked);
            _volumeSlider.UnregisterCallback<ClickEvent>(SetVolume);
        }
        
        /// <summary>
        /// Method triggered by the "back" button. It takes the user back to the main menu screen.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void OnBackButtonClicked(ClickEvent e) => UIManager.NavigateTo(MenuScreen.MainMenu);
        
        /// <summary>
        /// Method triggered when the 'volume' slider is updated. It updates the volume name accordingly.
        /// </summary>
        /// <param name="e">Click event -- not used.</param>
        private void SetVolume(ClickEvent e) => _mixer.volume = _volumeSlider.value;

    }







}