using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Domain
{
    public interface ISoftUpdatable
    {
        public DateTime UpdatedDate { get; set; }
        public Guid? UpdatedById { get; set; }
    }
}
