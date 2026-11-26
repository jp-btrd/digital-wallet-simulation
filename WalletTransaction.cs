using System;

namespace DigitalWallet
{
    // ABSTRACTION: this is the shared "blueprint" for every transaction type.
    // It holds the data all transactions have (reference, amount, time, status)
    // and forces every subtype to write its own Process() and GetSummary().
    public abstract class WalletTransaction
    {
        public string Reference { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Status { get; protected set; }    // Pending, Successful or Rejected
        public string Message { get; protected set; }   // reason shown to the user

        protected WalletTransaction(decimal amount)
        {
            Amount = amount;
            Timestamp = DateTime.Now;
            Status = "Pending";
            Message = "";
        }

        // Called by WalletSystem only after the transaction succeeded.
        public void SetReference(string reference)
        {
            Reference = reference;
        }

        // Each subtype must provide these (the abstract "contract").
        public abstract string TypeName { get; }
        public abstract bool Process();
        public abstract string GetSummary();

        // Shared helper used by all subtypes so the summary looks the same.
        protected string BaseSummary()
        {
            return Reference + " | " + TypeName + " | PHP " + Amount.ToString("N2")
                   + " | " + Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + " | " + Status;
        }

        // Shared helpers to mark the result.
        protected bool Reject(string reason)
        {
            Status = "Rejected";
            Message = reason;
            return false;
        }

        protected bool Succeed(string text)
        {
            Status = "Successful";
            Message = text;
            return true;
        }
    }
}
