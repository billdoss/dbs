using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbs.Models
{
    public class AccountHandler
    {
        public string branch_name { get; set; }
        public string label { get; set; }
        public string account_type { get; set; }
        public string status { get; set; }
        public string card_number { get; set; }
        public string address { get; set; }
    }

    public class Account : AccountHandler
    {
        public string account_number { get; set; }
    }

    //[Keyless]
    public class AccountResponse
    {
        [Key]
        public string account_number { get; set; }
        public string branch_name { get; set; }
        public string label { get; set; }
        public string account_type { get; set; }
        public string status { get; set; }
        public string card_number { get; set; }
        public string address { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime updated_date { get; set; }
    }

    
    public class AccountObj
    {
        [Key]
        public string account_number { get; set; }
        public string branch_name { get; set; }
        public string label { get; set; }
        public string account_type { get; set; }
        public string status { get; set; }
        public string card_number { get; set; }
        public string address { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime updated_date { get; set; }
    }


    public enum AccountType
    {
        Individu,
        Entreprise,
    }

    public enum Status
    {
        Actif,
        Inactif,
    }

}
