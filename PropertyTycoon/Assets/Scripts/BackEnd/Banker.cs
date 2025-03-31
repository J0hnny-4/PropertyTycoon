using System;
using System.Threading.Tasks;
using UI.Game;

namespace BackEnd
{
    public static class Banker
    {
        public static void PayPlayer(Object itemCausingPayment, int payee, int amount)
        {
            GameState.Players[payee].Money += amount;
        }
        
        public static int ChargePlayer(Object itemCausingPayment, int payer, int amount)
        {
            bool choice = DialogBoxFactory.PaymentDialogBox(itemCausingPayment, amount).AsTask().Result;
            if(GameState.Players[payer].Money < amount)
            {
                // TODO: Implement bankruptcy logic
                if (GameState.Players[payer].Money < amount)
                {
                    //TODO: kill player
                    int returnable = GameState.Players[payer].Money;
                    GameState.Players[payer].Money = 0;
                    return returnable;
                }
            }
            GameState.Players[payer].Money -= amount;
            return amount;
        }
        
        public static int ChargePlayer(Object itemCausingPayment, int payer, int amount, int payee)
        {
            int collected = ChargePlayer(itemCausingPayment, payer, amount);
            PayPlayer(itemCausingPayment, payee, collected);
            return collected;
        }
    }
}