using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Страховая_компания.DataBase
{
    public class ObjectsOfInsurance
    {
        [Key]
        public int ooiid { get; set; }
        public string name { get; set; }
    }
}
