using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public class ExpenseType
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
