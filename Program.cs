using System;
using System.Collections.Generic;
using System.Linq;

namespace Labboneimplementdesignpatterns
{
    public class CardHolder          // Singleton pattern for cardholder class only one instance of the class can be created and it also provides a global point access.
    {
        private string cardNum;
        private int pin;
        private string firstName;
        private string lastName;
        private double balance;

        public CardHolder(string cardNum, int pin, string firstName, string lastName, double balance)
        {
            this.cardNum = cardNum;
            this.pin = pin;
            this.firstName = firstName;
            this.lastName = lastName;
            this.balance = balance;
        }

        public string GetCardNum()
        {
            return cardNum;
        }

        public int GetPin()
        {
            return pin;
        }

        public string GetFirstName()
        {
            return firstName;
        }

        public string GetLastName()
        {
            return lastName;
        }

        public double GetBalance()
        {
            return balance;
        }

        public void SetCardNum(string newCardNum)
        {
            cardNum = newCardNum;
        }

        public void SetPin(int newPin)
        {
            pin = newPin;
        }

        public void SetFirstName(string newFirstName)
        {
            firstName = newFirstName;
        }

        public void SetLastName(string newLastName)
        {
            lastName = newLastName;
        }

        public void SetBalance(double newBalance)
        {
            balance = newBalance;
        }
    }

    public interface ITransactionStrategy  // Strategy pattern the interface is defining a commonstrategy and the classes withdraw deposit and balance implements it.
    {
        void PerformTransaction(CardHolder currentUser);
    }

    public class DepositStrategy : ITransactionStrategy
    {
        public void PerformTransaction(CardHolder currentUser)
        {
            Console.WriteLine("How much $$ would you like to deposit: ");
            double deposit = double.Parse(Console.ReadLine());
            currentUser.SetBalance(currentUser.GetBalance() + deposit);
            Console.WriteLine("Thank you for your $$$. Your new balance is: " + currentUser.GetBalance());
        }
    }

    public class WithdrawStrategy : ITransactionStrategy
    {
        public void PerformTransaction(CardHolder currentUser)
        {
            Console.WriteLine("How much $$ would you like to withdraw: ");
            double withdrawal = double.Parse(Console.ReadLine());
            if (currentUser.GetBalance() < withdrawal)
            {
                Console.WriteLine("Insufficient balance :(");
            }
            else
            {
                currentUser.SetBalance(currentUser.GetBalance() - withdrawal);
                Console.WriteLine("You're good to go! Thank you :) ");
            }
        }
    }

    public class BalanceStrategy : ITransactionStrategy
    {
        public void PerformTransaction(CardHolder currentUser)
        {
            Console.WriteLine("Current balance: " + currentUser.GetBalance());
        }
    }

    public interface ITransactionStrategyFactory
    {
        ITransactionStrategy CreateStrategy(int option);
    }

    public class TransactionStrategyFactory : ITransactionStrategyFactory
    {
        public ITransactionStrategy CreateStrategy(int option)
        {
            switch (option)
            {
                case 1:
                    return new DepositStrategy();
                case 2:
                    return new WithdrawStrategy();
                case 3:
                    return new BalanceStrategy();
                default:
                    throw new ArgumentException("Invalid option.");
            }
        }
    }

    public class TransactionContext
    {
        private ITransactionStrategy strategy;

        public TransactionContext(ITransactionStrategy strategy)
        {
            this.strategy = strategy;
        }

        public void ExecuteStrategy(CardHolder currentUser)
        {
            strategy.PerformTransaction(currentUser);
        }
    }

    public class CardHolderSingleton
    {
        private static CardHolderSingleton instance;
        private List<CardHolder> cardHolders;
        private CardHolder currentUser;

        private CardHolderSingleton()
        {
            cardHolders = new List<CardHolder>
        {
            new CardHolder("889955665", 8899, "Elin", "Thim", 460.55),
            new CardHolder("287555669", 1050, "Malin", "Lindblom", 980.55),
            new CardHolder("658955667", 2266, "Andreas", "Blom", 880.55),
            new CardHolder("487955664", 9966, "Martin", "Karlsson", 550.55),
            new CardHolder("286655662", 4422, "Kristina", "Eriksson", 680.55)
        };
        }

        public static CardHolderSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new CardHolderSingleton();
            }
            return instance;
        }

        public void RunBankSystem()
        {
            Console.WriteLine("Welcome to your Swedish bank");
            Console.WriteLine("Please insert your debit card: ");
            string debitCardNum = "";

            while (true)
            {
                try
                {
                    debitCardNum = Console.ReadLine();
                    currentUser = cardHolders.FirstOrDefault(a => a.GetCardNum() == debitCardNum);
                    if (currentUser != null)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Card is not recognized, please try again");
                    }
                }
                catch
                {
                    Console.WriteLine("Card is not recognized, please try again");
                }
            }

            Console.WriteLine("Please enter your pin: ");
            int userPin = 0;
            while (true)
            {
                try
                {
                    userPin = int.Parse(Console.ReadLine());
                    if (currentUser != null && currentUser.GetPin() == userPin)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect pin, please try again");
                    }
                }
                catch
                {
                    Console.WriteLine("Incorrect pin, please try again");
                }
            }

            Console.WriteLine("Welcome " + currentUser.GetFirstName() + " :)");

            ITransactionStrategyFactory strategyFactory = new TransactionStrategyFactory(); // The factory method itransactionstrategyfactory defines a factory interface with a createstrategy methodand with the transactionstrategyfactory class it implements the interface to create a specific transaction that is based upon the provided option.

            int option = 0;
            do
            {
                PrintOptions();
                try
                {
                    option = int.Parse(Console.ReadLine());
                    if (option >= 1 && option <= 3)
                    {
                        ITransactionStrategy strategy = strategyFactory.CreateStrategy(option);
                        TransactionContext context = new TransactionContext(strategy);
                        context.ExecuteStrategy(currentUser);
                    }
                    else if (option == 4)
                    {
                        break;
                    }
                    else
                    {
                        option = 2;
                    }
                }
                catch
                {
                    option = 0;
                }
            }
            while (option != 4);

            Console.WriteLine("Thank you! The Swedish bank wishes you a nice day :)");
        }

        private void PrintOptions()
        {
            Console.WriteLine("Choose one of the following options");
            Console.WriteLine("1. Make a deposit");
            Console.WriteLine("2. Make a withdrawal");
            Console.WriteLine("3. Show your bank balance");
            Console.WriteLine("4. Exit");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CardHolderSingleton cardHolderSingleton = CardHolderSingleton.GetInstance();
            cardHolderSingleton.RunBankSystem();
        }
    }
}