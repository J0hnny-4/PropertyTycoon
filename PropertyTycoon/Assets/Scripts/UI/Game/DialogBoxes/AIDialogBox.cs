using System.Threading.Tasks;
using Data;
using UnityEngine.UIElements;

namespace UI.Game.DialogBoxes
{
    public class AIDialogBox : BaseDialogBox<bool>
    {
        /// <summary>
        /// Initialise an AI dialog box. This is a simple message box, closing itself after a short amount of time.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text shown in the right panel.</param>
        /// 
        public void Initialise(string title, string text)
        {
            base.Initialise();
            SetTitle(title);
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

        public override async Task<bool> AsTask()
        {
            await Task.Delay(Cons.AIDialogBoxDelay);
            Close();
            return true;
        }
    }
}