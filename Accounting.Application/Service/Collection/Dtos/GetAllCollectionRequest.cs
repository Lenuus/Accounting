using Accounting.Common.Enum;
using AccountingsTracker.Common.Dtos;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Collection.Dtos
{
    public class GetAllCollectionRequest:PagedRequestDto
    {
        public string Number { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
