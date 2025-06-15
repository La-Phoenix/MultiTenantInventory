using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantInventory.Application.Common.Interfaces;
using MultiTenantInventory.Application.DTOs;
using MultiTenantInventory.Domain.Entities;
using MultiTenantInventory.Infrastructure.Persistence;
using MultiTenantInventory.Infrastructure.Services;

namespace MultiTenantInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantController : ControllerBase
    {
        public readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        public TenantController(AppDbContext dbContext, IMapper mapper, IJwtTokenService jwtTokenService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto request)
        {
            Console.WriteLine(request);

            // 1. Check subdomain uniqueness
            var existingTenant = await _dbContext.Tenants
                .FirstOrDefaultAsync(t => t.Subdomain == request.Subdomain);
            var duplicateEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.AdminEmail);
            if (duplicateEmail != null) return Conflict(new ApiResponse<string>("Tenant already exists!"));

            if (existingTenant != null)
            {
                return Conflict(new ApiResponse<string>("Subdomain already exists! Please choose a different subdomain."));
            }

            // Begin transaction: transaction to ensure atomicity — so if anything fails (e.g., tenant save, user creation, or linking), nothing gets saved.
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // 2. Create and save tenant first
                var tenant = _mapper.Map<Tenant>(request);
                tenant.Id = Guid.NewGuid();

                await _dbContext.Tenants.AddAsync(tenant);
                await _dbContext.SaveChangesAsync();

                // 3. Create admin user
                var admin = _mapper.Map<User>(request);
                admin.Id = Guid.NewGuid();
                admin.TenantId = tenant.Id;
                //admin.IsAdmin = true; // Done by mapping profile

                await _dbContext.Users.AddAsync(admin);
                await _dbContext.SaveChangesAsync();

                // 4. Update tenant with owner
                tenant.OwnerUserId = admin.Id;
                _dbContext.Tenants.Update(tenant); // EF Core does not always track entities as expected after SaveChangesAsync() — especially if no additional property besides OwnerUserId is marked as modified.
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                // 5. Build response
                var token = _jwtTokenService.GenerateToken(admin.Id, tenant.Id, admin.Email);
                var accessUrl = $"https://{tenant.Subdomain}.example.com";

                var response = new CreateTenantDtoResponseData
                {
                    Subdomain = tenant.Subdomain,
                    AccessUrl = accessUrl,
                    AccessToken = token
                };

                return CreatedAtAction(nameof(CreateTenant), new ApiResponse<CreateTenantDtoResponseData>(response, "Tenant Created Successfully"));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new ApiResponse<string>($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTenants()
        {
            var tenants = await _dbContext.Tenants
                .Include(t => t.Users) // Include users if needed
                .ToListAsync();
            var tenantDtos = _mapper.Map<List<TenantDto>>(tenants);
            return Ok(new ApiResponse<List<TenantDto>>(tenantDtos, "All Tenants Fetched Successfully"));
        }
    }
}
