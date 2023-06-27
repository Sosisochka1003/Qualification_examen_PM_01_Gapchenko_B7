using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Страховая_компания.DataBase
{
    public class Employees
    {
        [Key]
        public int eid { get; set; }
        public int branch { get; set; }
        public string last_name { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
    }
}
