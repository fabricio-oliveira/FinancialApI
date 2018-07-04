using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace FinancialApi.Models.Entity
{
    public class Payment :Entry
     {
        public Payment(){
            this.Type = "payment";
        }

        public Payment(string Description, string DestinationAccount, string DestinationBank,
                       string TypeAccount, string DestinationIdentity, decimal Value, 
                       decimal FinancialCharges, DateTime Date):
                base(Description, DestinationAccount, DestinationBank, TypeAccount, DestinationIdentity, 
                        Value,FinancialCharges, Date){
            this.Type = "payment";
        }
        
    }
}
