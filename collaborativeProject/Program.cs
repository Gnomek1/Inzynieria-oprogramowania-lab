using System;
using System.Threading;

namespace collaborativeProjectSln
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Podaj swoje imię: ");
            string name = Console.ReadLine();

            var account = new BankAccount(name);

            try
            {
                account.Deposit(100);

                Console.Write("Podaj imię do wypłaty 50 zł (weryfikacja): ");
                string verifyName = Console.ReadLine();

                account.Withdraw(50, verifyName);

                Console.WriteLine("\nCzekam 30 sekund bez aktywności, żeby zasymulować brak operacji...");
                Thread.Sleep(30 * 1000); // 30 sekund

                Console.WriteLine("\nWykonuję wpłatę 30 zł po dłuższej nieaktywności (powinna aktywować konto):");
                account.Deposit(30);

                Console.WriteLine("\nZamykam konto:");
                account.CloseAccount();

                Console.WriteLine("\nSpróbujmy jeszcze wpłacić po zamknięciu konta:");
                account.Deposit(10); // powinno rzucić wyjątek
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Błąd] {ex.Message}");
            }

            account.ShowLog();

            Console.WriteLine("\nKoniec programu.");
        }
    }
}