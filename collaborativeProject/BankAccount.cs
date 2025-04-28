using System;
using System.Collections.Generic;

namespace collaborativeProjectSln
{
    public class BankAccount
    {
        private decimal _balance;
        private bool _isClosed;
        private bool _isDeactivated;
        private readonly string _ownerName;
        private readonly List<string> _log;
        private DateTime _lastActivity;
        private readonly TimeSpan _inactivityLimit = TimeSpan.FromMinutes(2);

        public BankAccount(string ownerName)
        {
            _balance = 0;
            _isClosed = false;
            _isDeactivated = false;
            _ownerName = ownerName;
            _log = new List<string>();
            _lastActivity = DateTime.Now;
            Log($"[INIT] Konto utworzone dla: {_ownerName}");
        }

        public void Deposit(decimal amount)
        {
            CheckDeactivation();
            if (amount <= 0) throw new ArgumentException("Kwota musi być większa od zera");
            if (_isClosed) throw new InvalidOperationException("Konto jest zamknięte");

            if (_isDeactivated)
            {
                Reactivate();
            }

            _balance += amount;
            _lastActivity = DateTime.Now;
            Log($"[DEPOSIT] +{amount} zł. Saldo: {_balance} zł");
        }

        public void Withdraw(decimal amount, string providedName)
        {
            CheckDeactivation();
            if (_isClosed) throw new InvalidOperationException("Konto jest zamknięte");

            if (!VerifyIdentity(providedName))
            {
                Log("[WITHDRAW] Weryfikacja tożsamości nieudana");
                throw new InvalidOperationException("Weryfikacja tożsamości nie powiodła się");
            }

            if (amount > _balance) throw new InvalidOperationException("Brak wystarczających środków");

            if (_isDeactivated)
            {
                Reactivate();
            }

            _balance -= amount;
            _lastActivity = DateTime.Now;
            Log($"[WITHDRAW] -{amount} zł. Saldo: {_balance} zł");
        }

        private bool VerifyIdentity(string providedName)
        {
            return string.Equals(_ownerName, providedName, StringComparison.OrdinalIgnoreCase);
        }

        public void CloseAccount()
        {
            _isClosed = true;
            Log("[CLOSE] Konto zostało zamknięte");
        }

        public void Deactivate()
        {
            if (!_isClosed && !_isDeactivated)
            {
                _isDeactivated = true;
                Log("[DEACTIVATE] Konto zostało zdezaktywowane");
            }
        }

        private void Reactivate()
        {
            _isDeactivated = false;
            Log("[REACTIVATE] Konto zostało ponownie aktywowane (akcja dodatkowa)");
            Console.WriteLine(">> Konto zostało ponownie aktywowane.");
        }

        private void CheckDeactivation()
        {
            if (!_isDeactivated && !_isClosed)
            {
                var inactiveTime = DateTime.Now - _lastActivity;
                if (inactiveTime >= _inactivityLimit)
                {
                    Deactivate();
                }
            }
        }

        private void Log(string message)
        {
            string entry = $"{DateTime.Now:HH:mm:ss} {message}";
            _log.Add(entry);
            Console.WriteLine(entry);
        }

        public void ShowLog()
        {
            Console.WriteLine("\n=== HISTORIA KONTA ===");
            foreach (var entry in _log)
            {
                Console.WriteLine(entry);
            }
        }

        public decimal GetBalance() => _balance;
        public bool IsClosed => _isClosed;
        public bool IsDeactivated => _isDeactivated;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace collaborativeProjectSln
{
    internal class BankAccount
    {
    }
}
