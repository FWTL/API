//using System.Collections.Generic;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using FWTL.Domain.Users;
//using IdentityServer4.Extensions;
//using IdentityServer4.Models;
//using IdentityServer4.Services;
//using Microsoft.AspNetCore.Identity;

//namespace FWTL.Common.Net.Helpers
//{
//    public class UserProfileService : IProfileService
//    {
//        private readonly UserManager<User> _userManager;

//        public UserProfileService(UserManager<User> userManager)
//        {
//            _userManager = userManager;
//        }

//        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
//        {
//            string sub = context.Subject.GetSubjectId();
//            User user = await _userManager.FindByIdAsync(sub);

//            var claims = new List<Claim>
//            {
//                new Claim("phone_number", user.PhoneNumber ?? string.Empty)
//            };

//            context.IssuedClaims = claims;
//        }

//        public async Task IsActiveAsync(IsActiveContext context)
//        {
//            string sub = context.Subject.GetSubjectId();
//            User user = await _userManager.FindByIdAsync(sub);
//            context.IsActive = user != null;
//        }
//    }
//}