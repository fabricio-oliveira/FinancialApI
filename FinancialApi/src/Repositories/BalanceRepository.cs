﻿using System;
using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using FinancialApi.Utils;


namespace FinancialApi.Repositories
{
    public class BalanceRepository : GenericRepository, IBalanceRepository
    {
        readonly DataBaseContext _context;

        public BalanceRepository(DataBaseContext context) : base(context)
        {
            _context = context;
        }

        public long Count()
        {
            return _context.Balances.Count();
        }

        public void Save(Balance balance)
        {
            _context.Balances.Add(balance);
            _context.SaveChanges();
        }

        public void Update(Balance balance)
        {
            _context.Balances.Update(balance);
            _context.SaveChanges();
        }

        public Balance Find(long? id) => _context.Balances.Find(id);



        public void UpdateCurrentAndFutureBalance(DateTime date, long accountId)
        {

            var balancesToUpdate = _context.Balances.Where(x => x.Date >= date.AddDays(-1) && x.AccountId == accountId)
                                            .OrderBy(x => x.Date)
                                            .ToList();

            var balancesUpdated = balancesToUpdate.Skip(1)
                                                  .Zip(balancesToUpdate,
                                                      (c, p) =>
                                                            {
                                                                c.Total = p.Total + c.Inputs.Sum(x => x.Value) - c.Outputs.Sum(x => x.Value) - c.Charges.Sum(x => x.Value);
                                                                c.UpdateDayPosition(p.Total);
                                                                return c;
                                                            })
                                            .ToArray();

            _context.UpdateRange(balancesUpdated);
            _context.SaveChanges();

        }

        public List<Balance> ListTodayMore30Ahead(long? accountId)
        {
            var lista = _context.Balances.Where(x => x.AccountId == accountId
                                         && x.Date >= DateTime.Today
                                         && x.Date < DateTime.Today.AddDays(30))
                                    .OrderBy(x => x.Date)
                                    .ToList();
            return lista;
        }

        public Balance FindBy(Account account, DateTime date)
        {
            return _context.Balances
                                   .Where(x => x.Account == account
                                          && x.Date == date)
                                    .FirstOrDefault();
        }

        public Balance FindOrCreateBy(Account account, DateTime date)
        {
            var balance = FindBy(account, date);

            if (balance != null) return balance;

            var lastBalance = LastBy(account, date);
            if (lastBalance != null)
            {
                balance = new Balance(date, new List<ShortEntryDTO>(),
                                            new List<ShortEntryDTO>(),
                                            new List<ShortEntryDTO>(), lastBalance.Total, 0m, account);
                _context.Balances.Add(balance);
                _context.SaveChanges();
                return balance;
            }

            balance = new Balance(date, new List<ShortEntryDTO>(),
                                        new List<ShortEntryDTO>(),
                                        new List<ShortEntryDTO>(), 0m, null, account);
            _context.Balances.Add(balance);
            _context.SaveChanges();

            return balance;
        }

        public Balance LastByOrDefault(Account account, DateTime date)
        {
            var balance = LastBy(account, date);
            return balance ?? new Balance(date, new List<ShortEntryDTO>(),
                                                new List<ShortEntryDTO>(),
                                                new List<ShortEntryDTO>(), 0.0m, 0.0m, account);
        }

        public Balance LastBy(Account account, DateTime date)
        {
            return _context.Balances
                                  .Where(x => x.Account.Id == account.Id &&
                                         x.Date < date)
                                  .OrderByDescending(x => x.Date)
                                  .FirstOrDefault();
        }

        public List<Balance> ToProcess(DateTime date, List<Account> accounts)
        {
            if (accounts.Count == 0)
                return new List<Balance>();

            var accountIds = accounts.Select(x => x.Id);

            var balancesExistent = _context.Balances
                                           .Where(x => x.Date.IsSameDate(date) &&
                                                  accountIds.Contains(x.AccountId))
                                           .ToList();

            var IdsAccountBalanceExistent = balancesExistent.Select(y => y.AccountId.ToString());

            var balancesToCreate = accounts.Where(x => !IdsAccountBalanceExistent.Contains(x.Id.ToString()))
                                             .Select(x => FindOrCreateBy(x, date))
                                             .ToList();

            return balancesExistent.Concat(balancesToCreate).Where(x => !x.Closed).ToList();
        }
    }
}

