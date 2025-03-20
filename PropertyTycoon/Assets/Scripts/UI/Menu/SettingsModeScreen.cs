using System.Collections.Generic;
using UI.Controllers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

namespace UI.Menu
{
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
            _backButton.RegisterCallback<ClickEvent>(OnBackButtonClick);
            _volumeSlider.RegisterCallback<ClickEvent>(SetVolume);
        }
        protected override void CleanUp()
        {
            _backButton.UnregisterCallback<ClickEvent>(OnBackButtonClick);
            _volumeSlider.UnregisterCallback<ClickEvent>(SetVolume);
        }

        private void OnBackButtonClick(ClickEvent evt)
        {
            UIManager.NavigateTo(MenuScreen.MainMenu);
        }

        private void SetVolume(ClickEvent e)
        {
            _mixer.volume = _volumeSlider.value;
        }

    }







}