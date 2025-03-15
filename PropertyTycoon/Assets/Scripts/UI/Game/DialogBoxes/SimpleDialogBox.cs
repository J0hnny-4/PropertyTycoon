using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
    public class SimpleDialogBox : BaseDialogBox<bool>
    {
        /// <summary>
        /// Initialise the dialog box by setting values and callbacks.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text shown in the left panel.</param>
        /// <param name="icon">Icon shown in the right panel.</param>
        /// <param name="cancelText">Text of cancel button.</param>
        /// <param name="confirmText">Text of confirm button.</param>
        /// 
        public void Initialise(string title, 
            string text,
            Texture2D icon,
            string cancelText = null,
            string confirmText = null,
            bool closable = false
            )
        {
            base.Initialise();
            SetTitle(title);
            if (cancelText != null) { SetCancelButton(cancelText); }
            if (confirmText != null) { SetConfirmButton(confirmText); }
            if (closable) { AllowClosing(false); }
            
            
            LeftPanel.style.backgroundImage = icon;
            RightPanel.Q<Label>("text").text = text;
        }

        protected override void HandleCancelClicked()
        {
            RaiseOnChoiceMade(false);
            Close();
        }

        protected override void HandleConfirmClicked()
        {
            RaiseOnChoiceMade(true);
            Close();
        }
    }
}