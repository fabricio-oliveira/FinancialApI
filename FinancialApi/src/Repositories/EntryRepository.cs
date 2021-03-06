﻿using System.Collections.Generic;
using System.Linq;
using FinancialApi.Config;
using FinancialApi.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi.Repositories
{
    public class EntryRepository : GenericRepository, IEntryRepository
    {
        readonly DataBaseContext _context;

        public EntryRepository(DataBaseContext context) : base(context)
        {
            _context = context;
        }

        public long Count()
        {
            return _context.Entries.Count();
        }

        public void Save(Entry entry)
        {
            _context.Entries.Add(entry);
            _context.SaveChanges();
        }

        public void Update(Entry entry)
        {
            _context.Entries.Update(entry);
            _context.SaveChanges();
        }

        public Entry Find(long? id)
        {
            return _context.Entries.Find(id);
        }
    }
}
