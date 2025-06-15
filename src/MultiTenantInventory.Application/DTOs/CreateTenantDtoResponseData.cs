using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Application.DTOs
{
    public class CreateTenantDtoResponseData
    {
        public string AccessUrl { get; set; } = default!;
        public string Subdomain { get; set; } = default!;
        public string AccessToken { get; set; } = default!;
    }
}
