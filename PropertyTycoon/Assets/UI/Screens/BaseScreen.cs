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
        private BaseUIManager _manager;
        private VisualElement _root;

        /// <summary>
        /// Called by the UIManager when setting up, which passes itself as the argument.
        /// It initialises the Screen attributes. Concrete derivations should use this to initialise additional fields,
        /// register callbacks, etc.
        /// </summary>
        /// <param name="manager">Reference to UIManager, in order to facilitate navigation between screens.</param>
        public void Initialise(BaseUIManager manager)
        {
            _manager = manager;
            _root = GetComponent<UIDocument>().rootVisualElement;
        }
    
        /// <summary>
        /// Shows the screen.
        /// </summary>
        public void Show()
        {
            _root.style.display = DisplayStyle.Flex;
        }
    
        /// <summary>
        /// Hides the screen.
        /// NOTE: The screen is simply hidden, it will consume resources in the background. This allows to quickly switch
        /// between screens by avoiding multiple re-initialisation of the same screen.
        /// </summary>
        public void Hide()
        {
            _root.style.display = DisplayStyle.None;
        }
    
    }
}