using FileHelpers;
using System;

namespace PersonalFinanceManager.Client.Helpers.CSV.Models
{
    [DelimitedRecord(";")]
    public class LuminorStatement
    {
        public string TransactionType { get; set; }

        //[FieldQuoted('"', QuoteMode.OptionalForBoth)]
        //[FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime Date { get; set; }

        public string Time { get; set; }

        //[FieldQuoted('"', QuoteMode.OptionalForBoth)]
        public float Amount { get; set; }

        public string Equivalent { get; set; }

        //[FieldQuoted('"', QuoteMode.OptionalForBoth)]
        //[FieldConverter(typeof(IsExpenseConverter))]
        public bool IsExpense { get; set; }

        public string OrigAmount { get; set; }

        public string OrigCurrency { get; set; }

        public string DocumentNumber { get; set; }

        public string TransacionId { get; set; }

        public string CusomersCode { get; set; }

        public string PaymentCode { get; set; }

        public string Details { get; set; }

        public string BIC { get; set; }

        public string DesignationOfCreditInstitution { get; set; }

        public string OtherSideAccountNumber { get; set; }

        public string Designation { get; set; }

        public string RegNumber { get; set; }

        public string CustomersCode { get; set; }

        public string AccountNumber { get; set; }

        public string Name { get; set; }

        public string RegNoOfReceiver { get; set; }

        public string AccountNumberBeneficiary { get; set; }

        public string NameOfBeneficiary { get; set; }

        public string RegNumberBeneficiary { get; set; }
    }
}
