using System;
using System.Threading;
using threads.Model;

namespace threads
{
    public static class WithdrawExample
    {
        private static Account Account;
        private static Object _lock;
        private static Random Random;

        /// <summary>
        /// Set the variables
        /// </summary>
        private static void Init()
        {
            Account = new Account(2500.80M, 50.00M);
            Random = new Random();
            _lock = new Object();
        }

        /// <summary>
        /// Run the example
        /// </summary>
        public static void Run()
        {
            Init();

            for (var i = 0; i < 10; i++)
                new Thread(new ThreadStart(DoTransactions)).Start();

            Thread.Sleep(15000);

            Account.AddBalance(5000m);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine($"A deposit of $5000.00 has been made.");
            Console.WriteLine($"Balance: ${Account.Balance}");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Thread.Sleep(3000);

            for (var i = 0; i < 2; i++)
                new Thread(new ThreadStart(DoTransactions)).Start();
        }

        /// <summary>
        /// Execute the Withdraw
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static decimal Withdraw(decimal amount)
        {
            Console.WriteLine($"Withdraw Request: -${amount}");

            if (!IsWithdrawAuthorized())
                return -1;

            // IN HERE: Locks the other threads until the first thread completes the Withdraw below
            lock (_lock)
            {
                /* 
                    Once the object is release, continues from here and starts checking if the balance 
                    has amount enough to be proceed with an Withdraw request
                */
                if (!IsWithdrawAuthorized())
                    return -1;

                Console.WriteLine();
                Console.WriteLine("********** Account Locked **********");
                Account.PrintBalance();
                Account.PrintAmountToWithdraw(amount);

                Account.AddWithdraw(amount);

                Account.PrintBalance();
                Console.WriteLine("********** Account Released **********");
                Console.WriteLine();

                TakeANap();
                return Account.Balance;
            }
        }

        /// <summary>
        /// Gives a little break before continues :)
        /// </summary>
        public static void TakeANap()
        {
            // Sleep for a 1.5 seconds just to make more evident that the other threads are waiting
            // for the object LockObject to be unlocked.
            Thread.Sleep(1500);
        }

        /// <summary>
        /// It will check if the Account has balance enough to have a Withdraw
        /// </summary>
        /// <returns></returns>
        public static bool IsWithdrawAuthorized()
        {
            if (Account.Balance <= 0)
            {
                Account.PrintUnauthorizedWithdraw();
                return false;
            }

            return true;
        }

        /// <summary>
        /// It will generate a random amount to be requested as Withdraw from the current account balance.
        /// </summary>
        public static void DoTransactions()
        {
            var amount = Math.Round(Random.Next(20, 20000) / 13.6m, 2);
            Withdraw(amount);
        }
    }
}