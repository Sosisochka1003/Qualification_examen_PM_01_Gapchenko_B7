using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Страховая_компания.DataBase
{
    public class Treaty
    {
        [Key]
        public int tid {  get; set; }
        public DateOnly date_of_conclusion { get; set; }
        [ForeignKey("Clients")]
        public int id_client { get; set; }
        public Clients clients { get; set; }
        [ForeignKey("Employees")]
        public int id_emp { get; set; }
        public Employees employees { get; set; }
        [ForeignKey("ObjectsOfInsurance")]
        public int id_object { get; set; }
        public ObjectsOfInsurance objects { get; set; }
        public int insurance_payment { get; set; }
    }
}
