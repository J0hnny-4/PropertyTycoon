using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace UI.Game
{
    public class NewMonoBehaviourScript : MonoBehaviour
    {
        [SerializeField] private float totalTime = 3600;
        [SerializeField] private int spinFrequency = 2000;
        private VisualElement _hourglassIcon;
        private Label _countdownLabel;
        private Coroutine _countdownCoroutine;
        private bool IsRunning => _countdownCoroutine != null;
        public event Action OnTimeUp;
    
        private void Awake()
        {
            // grab references to UI objects
            var root = GetComponent<UIDocument>().rootVisualElement;
            _countdownLabel = root.Q<Label>("countdown-label");
            _hourglassIcon = root.Q<VisualElement>("icon");
            StarTimer();
        }
        
        /// <summary>
        /// Starts timer and hourglass animation.
        /// </summary>
        private void StarTimer()
        {
            Debug.Log("Timer started");
            if (IsRunning) { Debug.LogWarning("Timer is already running."); }
            _countdownCoroutine = StartCoroutine(RunTimer());
            _hourglassIcon.schedule.Execute(SpinHourglass).Until(() => !IsRunning).Every(spinFrequency);
        }

        /// <summary>
        /// Stops timer.
        /// </summary>
        private void StopTimer()
        {
            if (!IsRunning) { Debug.LogWarning("Timer is already stopped."); }
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        /// <summary>
        /// Coroutine for updating countdown. It decrements the timer and updates the countdown label.<br/>
        ///  Once the timer reaches zero, the event <c>OnTimeUp</c> is invoked.
        /// </summary>
        private IEnumerator RunTimer()
        {
            while (totalTime > 0f)
            {
                totalTime -= Time.deltaTime;
                _countdownLabel.text = TimeSpan.FromSeconds(totalTime).ToString(@"hh\:mm\:ss");
                yield return null;
            }
            OnTimeUp?.Invoke();
        }

        /// <summary>
        /// Spins the hourglass 180 degrees.
        /// </summary>
        private void SpinHourglass()
        {
            _hourglassIcon.ToggleInClassList("flip");
        }
    }
}
