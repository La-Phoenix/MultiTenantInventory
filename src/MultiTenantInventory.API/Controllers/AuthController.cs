using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.Application.Common;
using MultiTenantInventory.Application.Common.Interfaces;
using MultiTenantInventory.Application.DTOs;
using MultiTenantInventory.Domain.Entities;
using MultiTenantInventory.Infrastructure.Persistence;
using System.Globalization;

namespace MultiTenantInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ITenantProvider _tenantProvider;

        public AuthController(AppDbContext dbContext, IJwtTokenService jwtTokenService, ITenantProvider tenant)
        {
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
            _tenantProvider = tenant;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var tenant = _tenantProvider.GetTenant();
            if (tenant == null) return Unauthorized(new ApiResponse<string>("Tenant not resolved."));

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.TenantId == tenant.Id);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new ApiResponse<string>("Invalid credentials."));
            }

            var token = _jwtTokenService.GenerateToken(user.Id, tenant.Id, user.Email);
            return Ok(new ApiResponse<string>(token, "Login Successful."));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            var tenant = _tenantProvider.GetTenant();
            if (tenant == null) return Unauthorized(new ApiResponse<string>("Tenant not resolved."));

            var userExists = await _dbContext.Users.AnyAsync(u => u.Email == request.Email && u.TenantId == tenant.Id);
            if (userExists)
            {
                return Conflict(new ApiResponse<string>("User already exists."));
            }

            var textInfo = CultureInfo.CurrentCulture.TextInfo;
            var fullName = textInfo.ToTitleCase(request.FirstName.ToLower()) + " " +
                            textInfo.ToTitleCase(request.LastName.ToLower()); // e.g . "John Doe"
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                TenantId = tenant.Id,
                CreatedAt = DateTime.UtcNow,
                FullName = fullName,
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var token = _jwtTokenService.GenerateToken(user.Id, tenant.Id, user.Email);
            return Ok(new ApiResponse<string>(token, "User registered successfully."));
        }
    }
}
