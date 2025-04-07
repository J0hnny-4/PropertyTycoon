using System;
using System.Collections;
using BackEnd;
using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game
{
    /// <summary>
    /// The timer used only in Abridge mode.
    /// This class is responsible for handling the countdown logic and its UI representation.
    /// </summary>
    public class AbridgeTimer : MonoBehaviour
    {
        [SerializeField] private int spinFrequency = 2000;
        private TimeSpan _totalTime = Cons.TimeLimit;
        private VisualElement _hourglassIcon;
        private Label _countdownLabel;
        private Coroutine _countdownCoroutine;
        private bool IsRunning => _countdownCoroutine != null;
        public event Action OnTimeUp;
    
        private void Awake()
        {
            // destroys itself if game mode is not Abridged
            if (GameState.GameMode != GameMode.Abridged)
            {
                Destroy(gameObject);
                return;
            }
            
            // grab references to UI objects
            var root = GetComponent<UIDocument>().rootVisualElement;
            _countdownLabel = root.Q<Label>("countdown-label");
            _hourglassIcon = root.Q<VisualElement>("icon");
            
            // setup timer
            StarTimer();
            GameState.OnGameOver += _ => StopTimer();
        }
        
        /// <summary>
        /// Starts timer and hourglass animation.
        /// </summary>
        private void StarTimer()
        {
            if (IsRunning) { Debug.LogWarning("Timer is already running."); }
            _countdownCoroutine = StartCoroutine(RunTimer());
            _hourglassIcon.schedule.Execute(SpinHourglass).Until(() => !IsRunning).Every(spinFrequency);
        }

        /// <summary>
        /// Stops timer.
        /// </summary>
        private void StopTimer()
        {
            if (!IsRunning)
            {
                Debug.LogWarning("Timer is already stopped.");
                return;
            }
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        /// <summary>
        /// Coroutine for updating countdown. It decrements the timer and updates the countdown label.<br/>
        /// Once the timer reaches zero, the event <c>OnTimeUp</c> is invoked.
        /// </summary>
        private IEnumerator RunTimer()
        {
            while (_totalTime > TimeSpan.Zero)
            {
                _totalTime -= TimeSpan.FromSeconds(Time.deltaTime);
                _countdownLabel.text = _totalTime.ToString(@"hh\:mm\:ss");
                yield return null;
            }
            OnTimeUp?.Invoke();
        }

        /// <summary>
        /// Spins the hourglass 180 degrees.
        /// </summary>
        private void SpinHourglass() => _hourglassIcon.ToggleInClassList("flip");
    }
}
