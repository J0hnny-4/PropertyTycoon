using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    /// <summary>
    /// Generic screen, to be added to the scene as a GameObject (as a child of NavigationManager).
    /// As part of the same GameObject should be the corresponding UXML document.
    /// </summary>
    public abstract class BaseScreen : MonoBehaviour
    {
        protected ScreenType Type;
        protected VisualElement Root;
        protected NavigationManager NavManager;

        /// <summary>
        /// Initialises base fields. Serves a similar purpose to <c>Initialise</c>, but having this separate avoid
        /// having to explicitly call the base method in all derivation of this class.
        /// </summary>
        public void BaseSetup(NavigationManager navManager)
        {
            Root = GetComponent<UIDocument>().rootVisualElement;
            NavManager = navManager;
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
        /// Getter method for the screen's type.
        /// </summary>
        /// <returns>The type of the screen.</returns>
        public ScreenType GetScreenType() => Type;

        /// <summary>
        /// Shows the screen.
        /// </summary>
        public void Show() => Root.style.display = DisplayStyle.Flex;

        /// <summary>
        /// Hides the screen.<br/>
        /// NOTE: The screen is simply hidden, it will still consume resources in the background. This allows to quickly switch
        /// between screens by avoiding multiple re-initialisation of the same screen.
        /// </summary>
        public void Hide() => Root.style.display = DisplayStyle.None;

        /// <summary>
        /// Default call to <c>CleanUp</c> to remove event callbacks when object is destroyed.
        /// </summary>
        private void OnDestroy() => CleanUp();
    }
}
