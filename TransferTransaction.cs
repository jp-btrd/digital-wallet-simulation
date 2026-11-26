namespace DigitalWallet
{
    // INHERITANCE: TransferTransaction reuses the shared parts of WalletTransaction.
    // Its own rule: money moves from a sender to a different receiver.
    public class TransferTransaction : WalletTransaction
    {
        private WalletAccount sender;
        private WalletAccount receiver;

        public TransferTransaction(WalletAccount sender, WalletAccount receiver, decimal amount) : base(amount)
        {
            this.sender = sender;
            this.receiver = receiver;
        }

        public override string TypeName
        {
            get { return "Transfer"; }
        }

        public override bool Process()
        {
            // Check everything FIRST. No balance is touched until all checks pass.
            if (sender == null)
            {
                return Reject("Sender account not found.");
            }
            if (receiver == null)
            {
                return Reject("Recipient account not found.");
            }
            if (sender.AccountId == receiver.AccountId)
            {
                return Reject("Sender and recipient must be different accounts.");
            }
            if (Amount <= 0)
            {
                return Reject("Amount must be greater than zero.");
            }
            if (sender.GetBalance() < Amount)
            {
                return Reject("Insufficient balance.");
            }

            sender.Debit(Amount);
            receiver.Credit(Amount);
            sender.AddHistory(this);
            receiver.AddHistory(this);
            return Succeed("Transfer successful.");
        }

        public override string GetSummary()
        {
            return BaseSummary() + " | From: " + sender.AccountId + " | To: " + receiver.AccountId;
        }
    }
}
