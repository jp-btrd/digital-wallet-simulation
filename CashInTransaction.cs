namespace DigitalWallet
{
    // INHERITANCE: CashInTransaction gets Reference, Amount, Timestamp, Status
    // and Reject()/Succeed() from WalletTransaction.
    // Its own rule: money is only ADDED to one account.
    public class CashInTransaction : WalletTransaction
    {
        // CLASSROOM RULE (not a real GCash rule): max simulated cash-in per transaction.
        public const decimal MaxCashIn = 50000m;

        private WalletAccount account;

        public CashInTransaction(WalletAccount account, decimal amount) : base(amount)
        {
            this.account = account;
        }

        public override string TypeName
        {
            get { return "Cash-In"; }
        }

        public override bool Process()
        {
            if (account == null)
            {
                return Reject("Account not found.");
            }
            if (Amount <= 0)
            {
                return Reject("Amount must be greater than zero.");
            }
            if (Amount > MaxCashIn)
            {
                return Reject("Classroom rule: cash-in is limited to PHP " + MaxCashIn.ToString("N2") + " per transaction.");
            }

            account.Credit(Amount);
            account.AddHistory(this);
            return Succeed("Cash-in successful.");
        }

        public override string GetSummary()
        {
            return BaseSummary() + " | Account: " + account.AccountId;
        }
    }
}
