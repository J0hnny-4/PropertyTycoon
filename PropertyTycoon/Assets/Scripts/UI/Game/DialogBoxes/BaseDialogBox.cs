using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
    public abstract class BaseDialogBox : MonoBehaviour
    {
        protected VisualElement Background;
        protected Label TitleLabel;
        protected VisualElement LeftPanel;
        protected VisualElement RightPanel;
        protected Button CancelBtn;
        protected Button ConfirmBtn;
        protected Button CloseBtn;

        /// <summary>
        /// Grabs related UI document, and gets reference to all base elements.
        /// </summary>
        public void Awake()
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
            
            CloseBtn.clicked += Close;
        }


        protected abstract void CleanUp();
        
        protected void Close()
        {
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
        /// <param name="closable">Visibility value of the button.</param>
        protected void SetClosable(bool closable) => CloseBtn.visible = closable;

        /// <summary>
        /// Sets button text and action, and in the process un-hides it.
        /// </summary>
        /// <param name="button">The button to initialise.</param>
        /// <param name="text">Button text.</param>
        /// <param name="callback">Button on-click action.</param>
        protected void SetButton(Button button, string text, Action callback)
        {
            CancelBtn.text = text;
            CancelBtn.clicked += callback;
            CancelBtn.visible = true;
        }

        protected void SetCancelButton(string text, Action callback) => SetButton(CancelBtn, text, callback);
        protected void SetConfirmButton(string text, Action callback) => SetButton(ConfirmBtn, text, callback);
    }
}