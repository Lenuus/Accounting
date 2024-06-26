﻿using Accounting.Application.Service.Order.Dtos;
using Accounting.Common.Enum;

namespace Accounting.Application.Service.Corporation.Dtos
{
    public class CorporationUpdateRequestDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public bool Title { get; set; }
        public string TCKN { get; set; }
        public string VKN { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public decimal CurrentBalance { get; set; }
        public CorporationType CorporationType { get; set; }
    }
}