using System;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogBoxFactory : MonoBehaviour
{
    [SerializeField] private static VisualTreeAsset _simpleDialogBoxTemplate;

    public static VisualElement MakeSimpleDialogBox(
        Texture2D icon, 
        string title,
        string text,
        string cancelButtonText = "",
        string confirmButtonText = "",
        Action cancelCallback = null,
        Action confirmCallback = null
        )
    {
        // initialises template
        var root = new VisualElement();
        _simpleDialogBoxTemplate.CloneTree(root);
        
        // set ui elements
        root.Q<Label>("title").text = title;
        root.Q<Label>("text").text = text;
        root.Q<Button>("left-panel").style.backgroundImage = icon;
        
        // handles buttons
        var cancelButton = root.Q<Button>("cancel-button");
        var confirmButton = root.Q<Button>("confirm-button");
        cancelButton.text = cancelButtonText;
        confirmButton.text = confirmButtonText;

        if (cancelCallback != null)
        {
            cancelButton.clicked += cancelCallback;
        }
        else
        {
            cancelButton.visible = false;
        }

        if (confirmCallback != null)
        {
            confirmButton.clicked += confirmCallback;
        }
        else
        {
            confirmButton.visible = false;
        }
        
        return root;
    }
}
