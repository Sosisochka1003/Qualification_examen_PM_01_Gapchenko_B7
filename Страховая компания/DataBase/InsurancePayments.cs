using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Страховая_компания.DataBase
{
    public class InsurancePayments
    {
        [Key]
        public int ipid { get; set; }
        public DateOnly date { get; set; }
        [ForeignKey("Treaty")]
        public int id_treaty { get; set; }
        public Treaty treaty { get; set; }
        public int payout_amount { get; set; }
    }
}
