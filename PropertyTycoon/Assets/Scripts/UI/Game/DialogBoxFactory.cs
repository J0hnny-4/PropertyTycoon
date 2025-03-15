using UI.Game.DialogBoxes;
using UnityEngine;

public class DialogBoxFactory : MonoBehaviour
{
    private static GameObject simpleDialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/SimpleDialogBox");

    private static SimpleDialogBox MakeSimpleDialogBox()
    {
        var dialogObject = Instantiate(simpleDialogBoxPrefab);
        var simpleDialogBox = dialogObject.GetComponent<SimpleDialogBox>();
        return simpleDialogBox;
    }

    /// <summary>
    /// Creates a 'Payment Due' dialog box. 
    /// </summary>
    /// <param name="item">The item prompting the payment (Property, Card, etc.)</param>
    /// <param name="amount">The amount due.</param>
    /// <typeparam name="T">The type of the payment (Property, Card, etc.)</typeparam>
    /// <returns>A simple dialog box, showing only the 'pay' (true) button.</returns>
    public static SimpleDialogBox MakePaymentDialogBox<T>(T item, int amount)
    {
        var dialogBox = MakeSimpleDialogBox();
        
        // todo temporary variables -- swap with proper image & text
        Texture2D icon = Resources.Load<Texture2D>("Images/Icons/fee");
        string text = $"You have been charged ${amount} after landing on [SQUARE NAME].";
        
        dialogBox.Initialise("Payment Due", text, icon, confirmText: "Pay");
        return dialogBox;
    }

    /// <summary>
    /// Creates a 'Jail' dialog box.
    /// </summary>
    /// <returns>A simple dialog box, showing the 'pay' (true) and 'cancel' (false) buttons.</returns>
    public static SimpleDialogBox MakeJailLandingDialogBox()
    {
        var dialogBox = MakeSimpleDialogBox();
        dialogBox.Initialise(
            "Jail", 
            "You've been sent to prison.\n\nYour bail is set to $50. Would you like to pay it?",
            Resources.Load<Texture2D>("Images/Icons/jail-1"), 
            confirmText: "Pay", 
            cancelText: "Cancel");
        return dialogBox;
    }
}
