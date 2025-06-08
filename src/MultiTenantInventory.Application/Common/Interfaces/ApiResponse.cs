using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantInventory.Application.Common.Interfaces
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; };
        public T Data { get; set; } = default!;
        public ApiResponse(T data, string? message = null)
        {
            Success = true;
            Data = data;
            Message = message;
        }
        public ApiResponse(string? message = null)
        {
            Success = false;
            Message = message;
        }
    }
}

