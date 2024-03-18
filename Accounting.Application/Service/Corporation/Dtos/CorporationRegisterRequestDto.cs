using Accounting.Common.Enum;
using Accounting.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Application.Service.Corporation.Dtos
{
    public class CorporationRegisterRequestDto
    {
        public string Number { get; set; }
        [Required]
        public bool Title { get; set; }
        public string TCKN { get; set; }
        public string VKN { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public decimal CurrentBalance { get; set; }
        [Required]
        public CorporationType CorporationType { get; set; }
       
    }
}
