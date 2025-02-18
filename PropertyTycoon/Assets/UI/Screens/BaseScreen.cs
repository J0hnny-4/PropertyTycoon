using UI.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Screens
{
    /// <summary>
    /// Generic screen, to be added to the scene as a GameObject (as a child of UIManager).
    /// Part of the same GameObject should be the corresponding UXML document.
    /// </summary>
    public abstract class BaseScreen : MonoBehaviour
    {
        protected VisualElement Root;
        protected BaseUIManager UIManager;

        /// <summary>
        /// Initialises base fields. Serves a similar purpose to <c>Initialise</c>, but having this separate avoid
        /// having to explicitly call the base method in all derivation of this class.
        /// </summary>
        public void BaseSetup(BaseUIManager uiManager)
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            UIManager = uiManager;
        }
        
        /// <summary>
        /// Concrete derivations should use this method to initialise additional fields, register callbacks, etc.
        /// </summary>
        public abstract void Initialise();

        /// <summary>
        /// Used to un-register callbacks to interactive components (such as buttons).
        /// </summary>
        protected abstract void CleanUp();
    
        /// <summary>
        /// Shows the screen.
        /// </summary>
        public void Show()
        {
            Root.style.display = DisplayStyle.Flex;
        }
    
        /// <summary>
        /// Hides the screen.<br/>
        /// NOTE: The screen is simply hidden, it will consume resources in the background. This allows to quickly switch
        /// between screens by avoiding multiple re-initialisation of the same screen.
        /// </summary>
        public void Hide()
        {
            Root.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// Default call to <c>CleanUp</c> to remove event callbacks when object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            CleanUp();
        }
    }
}