using System;
using System.Collections.Generic;

class Program
{
    static Dictionary<string, Account> konta = new Dictionary<string, Account>();

    static void Main()
    {
        bool dzialanie = true;

        while (dzialanie)
        {
            Console.WriteLine("\n=== MENU GŁÓWNE ===");
            Console.WriteLine("1. Rejestracja");
            Console.WriteLine("2. Logowanie");
            Console.WriteLine("3. Wyjście");
            Console.Write("Wybierz opcję: ");
            string? opcja = Console.ReadLine();

            switch (opcja)
            {
                case "1":
                    Rejestracja();
                    break;
                case "2":
                    Logowanie();
                    break;
                case "3":
                    dzialanie = false;
                    Console.WriteLine("Zamykanie programu...");
                    break;
                default:
                    Console.WriteLine("Niepoprawny wybór, spróbuj ponownie.");
                    break;
            }
        }
    }

    static void Rejestracja()
    {
        Console.Write("Podaj ID użytkownika: ");
        string? id = Console.ReadLine();
        Console.Write("Podaj hasło: ");
        string? haslo = Console.ReadLine();

        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(haslo) && !konta.ContainsKey(id))
        {
            konta[id] = new Account(id, haslo);
            Console.WriteLine("✅ Konto zostało utworzone!");
        }
        else
        {
            Console.WriteLine("❌ Błąd: ID już istnieje lub dane są niepoprawne.");
        }
    }

    static void Logowanie()
    {
        Console.Write("Podaj ID użytkownika
