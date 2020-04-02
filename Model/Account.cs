using System;

namespace threads.Model
{
    public class Account
    {
        public decimal Balance { get; private set; }
        public decimal MinimumBalance { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="initialBalance"></param>
        /// <param name="minimumAmount"></param>
        public Account(decimal initialBalance, decimal minimumAmount)
        {
            this.Balance = initialBalance;
            this.MinimumBalance = minimumAmount;
        }

        /// <summary>
        /// Add value to the balance.
        /// </summary>
        /// <param name="amount"></param>
        public void AddBalance(decimal amount)
        {
            this.Balance += amount;
        }

        /// <summary>
        /// Remove the amount requested from the current balance.
        /// </summary>
        /// <param name="amount"></param>
        public void AddWithdraw(decimal amount)
        {
            this.Balance -= amount;

            if (HasReachedMinimumAmount())
                SendNotificationLowBalance();
        }

        /// <summary>
        /// Print current balance.
        /// </summary>
        public void PrintBalance()
        {
            Console.Write("Balance: ");
            Console.ForegroundColor = this.Balance >= 0 ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{AmountToString(this.Balance)}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

        }

        /// <summary>
        /// Print the amount to be taken from the account.
        /// </summary>
        /// <param name="amount"></param>
        public void PrintAmountToWithdraw(decimal amount)
        {
            Console.Write("Withdraw: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{AmountToString(amount > 0 ? -amount : amount)}");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        /// <summary>
        /// Print unauthorized Withdraw
        /// </summary>
        public void PrintUnauthorizedWithdraw()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("===========================");
            Console.WriteLine("Withdraw not authorized!");
            Console.WriteLine($"Balance: {AmountToString(this.Balance)}");
            Console.WriteLine("===========================");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Returns true if the current balance is lower than the minimum amount.
        /// </summary>
        /// <returns></returns>
        public bool HasReachedMinimumAmount()
        {
            return this.Balance <= this.MinimumBalance;
        }

        /// <summary>
        /// It will send a notification saying that the minimum amount has been reached
        /// </summary>
        public void SendNotificationLowBalance()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"WARNING: Account with low balance.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Giving a decimal amount, return a string on monetary format
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        private string AmountToString(decimal amount)
        {
            return $"{ (amount > 0 ? $"${amount}" : $"-${amount.ToString().Replace("-", "")}" )}";
        }
    }
}