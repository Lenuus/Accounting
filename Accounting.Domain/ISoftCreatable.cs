using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public interface ISoftCreatable
    {
        public DateTime InsertedDate { get; set; }
        public Guid InsertedById { get; set; }
    }
}
