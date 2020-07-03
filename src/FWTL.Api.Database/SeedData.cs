using FWTL.Common.Extensions;
using FWTL.Core.Enums;
using FWTL.Domain.Users;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FWTL.Api.Database
{
    public class SeedData
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        private Role _userRole;

        public SeedData(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task UpdateAsync()
        {
            await CreateRolesAsync();
            await SeedRolesAsync();
        }

        private async Task SeedRolesAsync()
        {
            if ((await _roleManager.FindByNameAsync(nameof(Roles.User))).IsNull())
            {
                await _roleManager.CreateAsync(_userRole);
            }
        }

        private Task CreateRolesAsync()
        {
            _userRole = new Role { Id = Roles.User, Name = nameof(Roles.User) };
            return Task.CompletedTask;
        }
    }
}