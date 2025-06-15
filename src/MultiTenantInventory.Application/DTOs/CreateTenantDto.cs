using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Application.DTOs
{
    public class CreateTenantDto
    {
        public string AdminFirstName { get; set; } = default!;
        public string AdminLastName { get; set; } = default!;
        public string Subdomain { get; set; } = default!;
        public string AdminEmail { get; set; } = default!;
        public string AdminPassword { get; set; } = default!;

    }
}
