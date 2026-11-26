using System;
using System.Collections.Generic;

namespace DigitalWallet
{
    // Owns the private lists of accounts and transactions and runs every operation.
    public class WalletSystem
    {
        private List<WalletAccount> accounts = new List<WalletAccount>();
        private List<WalletTransaction> transactions = new List<WalletTransaction>();
        private int referenceCounter = 0;

        public WalletSystem()
        {
            // 5 fictional accounts. All names, IDs and balances are made up.
            accounts.Add(new WalletAccount("ACC-1001", "Maria Sample", 5000m));
            accounts.Add(new WalletAccount("ACC-1002", "Juan Test", 2500m));
            accounts.Add(new WalletAccount("ACC-1003", "Ana Demo", 1000m));
            accounts.Add(new WalletAccount("ACC-1004", "Pedro Mock", 750m));
            accounts.Add(new WalletAccount("ACC-1005", "Lia Practice", 10000m));
        }

        public List<WalletAccount> GetAccounts()
        {
            return new List<WalletAccount>(accounts);
        }

        // Returns the account with this ID, or null if it does not exist.
        public WalletAccount FindAccount(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return null;
            }
            foreach (WalletAccount account in accounts)
            {
                if (string.Equals(account.AccountId, accountId.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return account;
                }
            }
            return null;
        }

        public WalletTransaction CashIn(string accountId, decimal amount)
        {
            WalletTransaction transaction = new CashInTransaction(FindAccount(accountId), amount);
            return Run(transaction);
        }

        public WalletTransaction Transfer(string senderId, string receiverId, decimal amount)
        {
            WalletTransaction transaction = new TransferTransaction(FindAccount(senderId), FindAccount(receiverId), amount);
            return Run(transaction);
        }

        public WalletTransaction PayBill(string accountId, string billerName, decimal amount)
        {
            WalletTransaction transaction = new BillPaymentTransaction(FindAccount(accountId), billerName, amount);
            return Run(transaction);
        }

        // POLYMORPHISM: 'transaction' is a WalletTransaction variable, but when we call
        // Process() C# runs the version that belongs to the real object
        // (CashIn, Transfer or BillPayment). We do not need any if/else on the type.
        private WalletTransaction Run(WalletTransaction transaction)
        {
            bool success = transaction.Process();
            if (success)
            {
                referenceCounter = referenceCounter + 1;
                transaction.SetReference("TXN-" + referenceCounter.ToString("D4"));
                transactions.Add(transaction);
            }
            return transaction;
        }
    }
}
