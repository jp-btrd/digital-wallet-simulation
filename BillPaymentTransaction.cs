using System;

namespace DigitalWallet
{
    // INHERITANCE: BillPaymentTransaction reuses the shared parts of WalletTransaction.
    // Its own rule: money leaves the account and goes to a MOCK biller from a short list.
    public class BillPaymentTransaction : WalletTransaction
    {
        // Fictional billers. No real company is contacted.
        public static readonly string[] MockBillers =
        {
            "Sample Power Co.",
            "Sample Water District",
            "Sample Internet Inc.",
            "Sample Mobile Load"
        };

        private WalletAccount account;
        private string billerName;

        public BillPaymentTransaction(WalletAccount account, string billerName, decimal amount) : base(amount)
        {
            this.account = account;
            this.billerName = billerName;
        }

        public override string TypeName
        {
            get { return "Bill Payment"; }
        }

        public override bool Process()
        {
            if (account == null)
            {
                return Reject("Account not found.");
            }

            // Look for the biller in the mock list (ignoring upper/lower case).
            string foundBiller = null;
            foreach (string biller in MockBillers)
            {
                if (string.Equals(biller, billerName, StringComparison.OrdinalIgnoreCase))
                {
                    foundBiller = biller;
                }
            }
            if (foundBiller == null)
            {
                return Reject("Unknown biller. Please choose a biller from the list.");
            }

            if (Amount <= 0)
            {
                return Reject("Amount must be greater than zero.");
            }
            if (account.GetBalance() < Amount)
            {
                return Reject("Insufficient balance.");
            }

            account.Debit(Amount);
            billerName = foundBiller;
            account.AddHistory(this);
            return Succeed("Bill payment successful.");
        }

        public override string GetSummary()
        {
            return BaseSummary() + " | Account: " + account.AccountId + " | Biller: " + billerName;
        }
    }
}
