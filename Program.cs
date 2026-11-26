using System;
using System.Globalization;

namespace DigitalWallet
{
    // Console controller: only handles typing and printing.
    // All the rules live in WalletSystem and the transaction classes.
    public class Program
    {
        static WalletSystem wallet = new WalletSystem();

        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            Console.WriteLine("=====================================================");
            Console.WriteLine("  DIGITAL WALLET SIMULATION (CLASSROOM PROTOTYPE)");
            Console.WriteLine("  Fictional accounts and fake money only.");
            Console.WriteLine("  NOT the official GCash app. No real transactions.");
            Console.WriteLine("=====================================================");

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                if (choice == null)
                {
                    break;   // input ended
                }
                choice = choice.Trim();

                if (choice == "1") ListAccounts();
                else if (choice == "2") ViewBalance();
                else if (choice == "3") CashIn();
                else if (choice == "4") Transfer();
                else if (choice == "5") PayBill();
                else if (choice == "6") ViewHistory();
                else if (choice == "7")
                {
                    Console.WriteLine("Goodbye! Thank you for trying the simulation.");
                    running = false;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please type a number from 1 to 7.");
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("---------------- MAIN MENU ----------------");
            Console.WriteLine("1. List sample accounts");
            Console.WriteLine("2. View balance");
            Console.WriteLine("3. Simulated cash-in");
            Console.WriteLine("4. Transfer funds");
            Console.WriteLine("5. Pay a mock bill");
            Console.WriteLine("6. View transaction history");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option (1-7): ");
        }

        static string Ask(string question)
        {
            Console.Write(question);
            string answer = Console.ReadLine();
            if (answer == null)
            {
                return "";
            }
            return answer.Trim();
        }

        // Reads an amount. Returns false (and prints a message) if it is empty or not a number.
        static bool ReadAmount(out decimal amount)
        {
            string text = Ask("Enter amount (PHP): ");
            if (text == "")
            {
                Console.WriteLine("Amount cannot be empty.");
                amount = 0;
                return false;
            }
            if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out amount))
            {
                Console.WriteLine("Invalid amount. Please type numbers only, like 500 or 250.75.");
                return false;
            }
            return true;
        }

        static void ListAccounts()
        {
            Console.WriteLine();
            Console.WriteLine("Sample accounts (fictional):");
            foreach (WalletAccount account in wallet.GetAccounts())
            {
                Console.WriteLine("  " + account.AccountId + " | " + account.DisplayName);
            }
        }

        static void ViewBalance()
        {
            string id = Ask("Enter account ID (example ACC-1001): ");
            WalletAccount account = wallet.FindAccount(id);
            if (account == null)
            {
                Console.WriteLine("Account not found. Use option 1 to see the valid account IDs.");
                return;
            }
            Console.WriteLine(account.DisplayName + " (" + account.AccountId + ") balance: PHP " + account.GetBalance().ToString("N2"));
        }

        static void CashIn()
        {
            string id = Ask("Enter account ID: ");
            decimal amount;
            if (!ReadAmount(out amount)) return;

            WalletTransaction result = wallet.CashIn(id, amount);
            ShowResult(result, wallet.FindAccount(id));
        }

        static void Transfer()
        {
            string senderId = Ask("Enter SENDER account ID: ");
            string receiverId = Ask("Enter RECIPIENT account ID: ");
            decimal amount;
            if (!ReadAmount(out amount)) return;

            WalletTransaction result = wallet.Transfer(senderId, receiverId, amount);
            ShowResult(result, wallet.FindAccount(senderId));
        }

        static void PayBill()
        {
            string id = Ask("Enter account ID: ");
            Console.WriteLine("Mock billers:");
            foreach (string biller in BillPaymentTransaction.MockBillers)
            {
                Console.WriteLine("  - " + biller);
            }
            string billerName = Ask("Type the biller name: ");
            decimal amount;
            if (!ReadAmount(out amount)) return;

            WalletTransaction result = wallet.PayBill(id, billerName, amount);
            ShowResult(result, wallet.FindAccount(id));
        }

        static void ViewHistory()
        {
            string id = Ask("Enter account ID: ");
            WalletAccount account = wallet.FindAccount(id);
            if (account == null)
            {
                Console.WriteLine("Account not found. Use option 1 to see the valid account IDs.");
                return;
            }

            Console.WriteLine("Transaction history for " + account.DisplayName + " (" + account.AccountId + "):");
            if (account.GetHistory().Count == 0)
            {
                Console.WriteLine("  No transactions yet.");
                return;
            }

            // POLYMORPHISM: each item is a WalletTransaction, but GetSummary()
            // shows the details of the real type (cash-in, transfer or bill payment).
            foreach (WalletTransaction transaction in account.GetHistory())
            {
                Console.WriteLine("  " + transaction.GetSummary());
            }
        }

        static void ShowResult(WalletTransaction result, WalletAccount account)
        {
            if (result.Status == "Successful")
            {
                Console.WriteLine("SUCCESS: " + result.Message);
                Console.WriteLine("Reference: " + result.Reference);
                Console.WriteLine("New balance: PHP " + account.GetBalance().ToString("N2"));
            }
            else
            {
                Console.WriteLine("REJECTED: " + result.Message + " No balance was changed.");
            }
        }
    }
}
