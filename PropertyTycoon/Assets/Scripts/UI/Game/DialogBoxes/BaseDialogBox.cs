using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
    /// <summary>
    /// The base from which all dialog boxes derive from. It contains a wide variety of components used (buttons, title,
    /// panels for text and images, etc.) by its derivations. If a derived class does not use a certain component, it is
    /// simply hidden/disabled.
    /// This class offers a way to get input from the player and return it as value, to be processed by the back-end.
    /// </summary>
    /// <typeparam name="T">The type of value processed and returnes, dependant by the dialog box.</typeparam>
    public abstract class BaseDialogBox<T> : MonoBehaviour
    {
        protected VisualElement Background;
        protected Label TitleLabel;
        protected VisualElement LeftPanel;
        protected VisualElement RightPanel;
        protected Button CancelBtn;
        protected Button ConfirmBtn;
        protected Button CloseBtn;
        private T ReturnValueOnClose;
        public event Action<T> OnChoiceMade;

        /// <summary>
        /// Grabs related UI document, and gets reference to all base elements.
        /// </summary>
        public virtual void Initialise()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // gets reference to UI elements
            Background = root.Q<VisualElement>("background");
            TitleLabel = root.Q<Label>("title");
            LeftPanel = root.Q<VisualElement>("left-panel");
            RightPanel = root.Q<VisualElement>("right-panel");
            CancelBtn = root.Q<Button>("cancel-button");
            ConfirmBtn = root.Q<Button>("confirm-button");
            CloseBtn = root.Q<Button>("close-button");
            
            // by default, hides all buttons.
            CloseBtn.visible = false;
            ConfirmBtn.visible = false;
            CancelBtn.visible = false;

            // registers buttons callbacks
            CloseBtn.clicked += HandleCloseClicked;
            CancelBtn.clicked += HandleCancelClicked;
            ConfirmBtn.clicked += HandleConfirmClicked;
        }

        /// <summary>
        /// Used to un-register listeners.
        /// </summary>
        protected virtual void CleanUp()
        {
            CloseBtn.clicked -= Close;
            ConfirmBtn.clicked -= HandleConfirmClicked;
            CancelBtn.clicked -= HandleCancelClicked;
        }
        
        /// <summary>
        /// Return the default/current value of Choice before destroying itself. 
        /// </summary>
        protected void Close()
        {
            CleanUp();
            Destroy(gameObject);
        }

        /// <summary>
        /// Sets title of dialog box.
        /// </summary>
        /// <param name="title">New title.</param>
        protected void SetTitle(string title) => TitleLabel.text = title;

        /// <summary>
        /// Hides/shows the close button, effectively setting the ability to close the dialog box.
        /// </summary>
        /// <param name="toReturn">Value to 'return' (via event) in case the close button is pressed.</param>
        /// 
        protected void AllowClosing(T toReturn)
        {
            CloseBtn.visible = true;
            ReturnValueOnClose = toReturn;
        }

        /// <summary>
        /// Sets button text and action, and in the process un-hides it.
        /// </summary>
        /// <param name="button">The button to initialise.</param>
        /// <param name="text">Button text.</param>
        protected void SetButton(Button button, string text)
        {
            button.text = text;
            button.visible = true;
        }
        
        /// <summary>
        /// Helper method, calls SetButton targeting the 'cancel' button.
        /// </summary>
        /// <param name="text">Text displayed on the button.</param>
        protected void SetCancelButton(string text) => SetButton(CancelBtn, text);
        
        /// <summary>
        /// Helper method, calls SetButton targeting the 'confirm' button.
        /// </summary>
        /// <param name="text">Text displayed on the button.</param>
        protected void SetConfirmButton(string text) => SetButton(ConfirmBtn, text);
        
        /// <summary>
        /// Method triggered by the 'cancel' button.
        /// </summary>
        protected abstract void HandleCancelClicked();
        
        /// <summary>
        /// Method triggered by the 'confirm' button.
        /// </summary>
        protected abstract void HandleConfirmClicked();

        /// <summary>
        /// Method triggered by the 'close' button.
        /// </summary>
        protected virtual void HandleCloseClicked()
        {
            RaiseOnChoiceMade(ReturnValueOnClose);
            Close();
        }
        
        /// <summary>
        /// Helper method to raise the OnChoiceMade event.
        /// </summary>
        /// <param name="value">Value corresponding to the choice made by the player.</param>
        protected void RaiseOnChoiceMade(T value) => OnChoiceMade?.Invoke(value);

        /// <summary>
        /// Returns a task that completes when the player makes a choice in the dialog box. The result of the task
        /// represents the player's selection, with the type determined by the specific dialog box implementation.
        /// </summary>
        /// <returns>A value reflecting the choice made by the player.</returns>
        public virtual Task<T> AsTask()
        {
            var tcs = new TaskCompletionSource<T>();
            OnChoiceMade += (t) => tcs.SetResult(t);
            return tcs.Task;
        }
    }
}