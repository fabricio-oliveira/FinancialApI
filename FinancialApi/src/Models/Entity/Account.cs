﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.Models.Entity
{
    [Table("Account")]
    public class Account
    {
        public Account() {}

        public Account(string number, string bank, string identity, string type) 
        {
            this.Number = number;
            this.Bank = bank;
            this.TypeAccount = type;
            this.Identity = identity;
        }

        [Key]
        public long? ID { get; set; }

        public string Number { get; set; }

        public string Bank { get; set; }

        public string TypeAccount { get; set; }

        public string Identity { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        //RelationShip
        public ICollection<Balance> Balances { get; set; }

    }
}