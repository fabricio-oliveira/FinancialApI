﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.src.Models.Entity
{
    [Table("Inputs")]
    public class Input : Entry
    {
        
        public Input()
        {
            this.Type = "input";
        }
    }
}
