using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Страховая_компания.DataBase
{
    public class Clients
    {
        [Key]
        public int cid { get; set; }
        public string last_name { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string gender { get; set; }
        public DateOnly date_of_birth { get; set; }
        public int passport_series { get; set; }
        public int passport_number { get; set; }

    }
}
