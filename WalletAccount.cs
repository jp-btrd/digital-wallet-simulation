using System.Collections.Generic;

namespace DigitalWallet
{
    // A fictional wallet account.
    // ENCAPSULATION: the balance and the history are PRIVATE.
    // The only way to change the balance is Credit() or Debit(),
    // and both of them check the amount first.
    public class WalletAccount
    {
        public string AccountId { get; private set; }
        public string DisplayName { get; private set; }

        private decimal balance;                                   // hidden
        private List<WalletTransaction> history = new List<WalletTransaction>();  // hidden

        public WalletAccount(string accountId, string displayName, decimal startingBalance)
        {
            AccountId = accountId;
            DisplayName = displayName;
            balance = startingBalance;
        }

        // Read-only way to see the balance.
        public decimal GetBalance()
        {
            return balance;
        }

        // Adds money. Returns false (and changes nothing) if the amount is not positive.
        public bool Credit(decimal amount)
        {
            if (amount <= 0)
            {
                return false;
            }
            balance = balance + amount;
            return true;
        }

        // Removes money. Returns false (and changes nothing) if the amount is
        // not positive or is more than the current balance.
        public bool Debit(decimal amount)
        {
            if (amount <= 0 || amount > balance)
            {
                return false;
            }
            balance = balance - amount;
            return true;
        }

        public void AddHistory(WalletTransaction transaction)
        {
            history.Add(transaction);
        }

        // Gives a COPY of the history, so outside code cannot change the real list.
        public List<WalletTransaction> GetHistory()
        {
            return new List<WalletTransaction>(history);
        }
    }
}
