using System;
using System.Collections.Generic;

// === INTERFEJS STANU ===
public interface IAccountState
{
    void Deposit(Account account, decimal amount);
    void Withdraw(Account account, decimal amount, string code);
    void CheckDeactivation(Account account);
    void Close(Account account);
    void CheckStatus(Account account);
}

// === KONKRETNE STANY ===
public class ActiveState : IAccountState
{
    public void Deposit(Account account, decimal amount)
    {
        account.Balance += amount;
        account.UpdateLastActivity();
        Console.WriteLine($"Wpłacono {amount:C}. Aktualne saldo: {account.Balance:C}");
    }

    public void Withdraw(Account account, decimal amount, string code)
    {
        if (account.Balance >= amount)
        {
            if (code == "1234")
            {
                account.Balance -= amount;
                account.UpdateLastActivity();
                Console.WriteLine($"✅ Wypłacono {amount:C}. Pozostałe saldo: {account.Balance:C}");
            }
            else Console.WriteLine("❌ Niepoprawny kod weryfikacyjny!");
        }
        else Console.WriteLine("❌ Niewystarczające środki.");
    }

    public void CheckDeactivation(Account account)
    {
        if (DateTime.Now - account.LastOperation > Account.InactivityTimeout)
        {
            Console.WriteLine("⚠ Konto zdezaktywowane z powodu braku aktywności.");
            account.SetState(new InactiveState());
        }
    }

    public void Close(Account account)
    {
        account.SetState(new ClosedState());
        Console.WriteLine("Konto zostało zamknięte.");
    }

    public void CheckStatus(Account account)
    {
        Console.WriteLine($"Saldo: {account.Balance:C}, Stan: AKTYWNE");
    }
}

public class InactiveState : IAccountState
{
    public void Deposit(Account account, decimal amount)
    {
        account.Balance += amount;
        account.UpdateLastActivity();
        account.SetState(new ActiveState());
        Console.WriteLine($"Konto aktywowane. Wpłacono {amount:C}. Aktualne saldo: {account.Balance:C}");
    }

    public void Withdraw(Account account, decimal amount, string code)
    {
        Console.WriteLine("❌ Konto jest nieaktywne. Wpłać środki, aby aktywować.");
    }

    public void CheckDeactivation(Account account) => Console.WriteLine("Konto już jest nieaktywne.");

    public void Close(Account account)
    {
        account.SetState(new ClosedState());
        Console.WriteLine("Konto zostało zamknięte.");
    }

    public void CheckStatus(Account account)
    {
        Console.WriteLine($"Saldo: {account.Balance:C}, Stan: NIEAKTYWNE");
    }
}

public class ClosedState : IAccountState
{
    public void Deposit(Account account, decimal amount) => Console.WriteLine("❌ Konto zamknięte. Operacja niemożliwa.");
    public void Withdraw(Account account, decimal amount, string code) => Console.WriteLine("❌ Konto zamknięte. Operacja niemożliwa.");
    public void CheckDeactivation(Account account) => Console.WriteLine("Konto zamknięte. Nie sprawdzamy dezaktywacji.");
    public void Close(Account account) => Console.WriteLine("Konto już jest zamknięte.");
    public void CheckStatus(Account account) => Console.WriteLine($"Saldo: {account.Balance:C}, Stan: ZAMKNIĘTE");
}

// === KONTEKST ===
public class Account
{
    public string Id { get; }
    private string password;
    public decimal Balance { get; set; }
    private IAccountState state;
    public DateTime LastOperation { get; private set; }
    public static readonly TimeSpan InactivityTimeout = TimeSpan.FromMinutes(5);

    public Account(string id, string password)
    {
        Id = id;
        this.password = password;
        Balance = 0;
        state = new ActiveState();
        LastOperation = DateTime.Now;
    }

    public bool VerifyPassword(string? input) => input != null && password == input;
    public void Deposit(decimal amount) => state.Deposit(this, amount);
    public void Withdraw(decimal amount, string code = "1234") => state.Withdraw(this, amount, code);
    public void CheckDeactivation() => state.CheckDeactivation(this);
    public void Close() => state.Close(this);
    public void CheckStatus() => state.CheckStatus(this);
    public void UpdateLastActivity() => LastOperation = DateTime.Now;
    public void SetState(IAccountState newState) => state = newState;
}
