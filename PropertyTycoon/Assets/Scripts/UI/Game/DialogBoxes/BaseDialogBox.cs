using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
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

            CloseBtn.clicked += HandleCloseClicked;
            CancelBtn.clicked += HandleCancelClicked;
            ConfirmBtn.clicked += HandleConfirmClicked;
        }

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
        
        protected void SetCancelButton(string text) => SetButton(CancelBtn, text);
        protected void SetConfirmButton(string text) => SetButton(ConfirmBtn, text);
        
        protected abstract void HandleCancelClicked();
        protected abstract void HandleConfirmClicked();

        protected virtual void HandleCloseClicked()
        {
            RaiseOnChoiceMade(ReturnValueOnClose);
            Close();
        }
        
        protected void RaiseOnChoiceMade(T value) => OnChoiceMade?.Invoke(value);

        public virtual Task<T> AsTask()
        {
            var tcs = new TaskCompletionSource<T>();
            OnChoiceMade += (t) => tcs.SetResult(t);
            return tcs.Task;
        }
    }
}