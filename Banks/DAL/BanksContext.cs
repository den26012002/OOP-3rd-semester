using Banks.Entities;
using Microsoft.EntityFrameworkCore;

namespace Banks.DAL
{
    public class BanksContext : DbContext
    {
        public BanksContext(DbContextOptions<BanksContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<CentralBank> CentralBanks { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankConditions> BanksConditions { get; set; }
        public DbSet<BankEventManager> BankEventManagers { get; set; }
        public DbSet<BaseBankAccount> BankAccounts { get; set; }
        public DbSet<DebitsBankAccount> DebitsBankAccounts { get; set; }
        public DbSet<DepositsBankAccount> DepositsBankAccounts { get; set; }
        public DbSet<CreditsBankAccount> CreditsBankAccounts { get; set; }
        public DbSet<BaseCashCommand> CashCommands { get; set; }
        public DbSet<WithdrawalCashCommand> WithdrawalCashCommands { get; set; }
        public DbSet<DepositCashCommand> DepositCashCommands { get; set; }
        public DbSet<TransferCashCommand> TransferCashCommands { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BankEventListener> BankEventListeners { get; set; }
    }
}
