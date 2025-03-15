using System;
using UI.Game.DialogBoxes;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
    public class SimpleDialogBox : BaseDialogBox
    {
        public void Initialise(string title, 
            string text,
            Texture2D icon,
            string cancelText = "",
            Action cancelCallback = null,
            string confirmText = "",
            Action confirmCallback = null)
        {
            SetTitle(title);
            if (cancelCallback != null) { SetCancelButton(cancelText, cancelCallback); }
            if (confirmCallback != null) { SetConfirmButton(confirmText, confirmCallback); }
            
            LeftPanel.style.backgroundImage = icon;
            RightPanel.Q<Label>("text").text = text;
        }


        protected override void CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}