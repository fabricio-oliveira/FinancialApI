using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinancialApi.Models.Entity
{
    [Table("Receipt")]
    public class Receipt : Entry
     {

        public Receipt() 
        {
            this.Type = "receipt";
        }

        public Receipt(string Description, string DestinationAccount, string DestinationBank,
                       string TypeAccount, string DestinationIdentity, decimal Value,
                       decimal FinancialCharges, DateTime Date) :
                base(Description, DestinationAccount, DestinationBank, TypeAccount, DestinationIdentity,
                        Value, FinancialCharges, Date)
        {
            this.Type = "receipt";
        }
        
     }
}