using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Users;
using GeoChallenger.Services.Settings.SocialNetworks;
using GeoChallenger.Web.Api.Models.Users;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace GeoChallenger.Web.Api.Providers
{
    public class GeoChallengerOAuthProvider : OAuthAuthorizationServerProvider
    {
        private const string AuthProviderKeyName = "authProvider";

        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly AuthenticationSettings _authenticationSettings;

        private readonly Dictionary<string, AccountTypeViewModel> _accountTypes;

        public GeoChallengerOAuthProvider(IMapper mapper, AuthenticationSettings authenticationSettings)
        {
            _mapper = mapper;
            _authenticationSettings = authenticationSettings;
            _accountTypes = GetAccountTypes();
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var authProvider = GetAccountTypes().Keys
                .SingleOrDefault(a => a.Equals(context.OwinContext.Get<string>(AuthProviderKeyName), StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrEmpty(authProvider)) {
                context.Rejected();
                context.SetError("invalidGrant", "Wrong client authentication provider.");
                return;
            }

            if (string.IsNullOrEmpty(context.UserName) || string.IsNullOrEmpty(context.Password)) {
                context.Rejected();
                context.SetError("invalidGrant", "Wrong username or password combination.");
                return;
            }

            var userDto = await GetUserAsync(context, authProvider);
            if (userDto == null) {
                context.Rejected();
                context.SetError("invalidGrant", "Wrong username or password combination.");
                return;
            }

            AuthenticateUser(context, userDto);

        }

        #region Private methods

        private static Dictionary<string, AccountTypeViewModel> GetAccountTypes()
        {
            return Enum.GetNames(typeof(AccountTypeViewModel))
                .ToDictionary(name => name, name => (AccountTypeViewModel)Enum.Parse(typeof(AccountTypeViewModel), name));
        }

        private async Task<UserDto> GetUserAsync(OAuthGrantResourceOwnerCredentialsContext context, string authProvider)
        {
            var userDto = await _usersService.GetUserAsync(context.Password, _mapper.Map<AccountTypeDto>(_accountTypes[authProvider]));
            
            return userDto;
        }

        private void AuthenticateUser(OAuthGrantResourceOwnerCredentialsContext context, UserDto user)
        {
            var identity = new ClaimsIdentity(new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            },
            DefaultAuthenticationTypes.ExternalBearer);

            var properties = new AuthenticationProperties() {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(_authenticationSettings.UserTokenLifetimeInDays)
            };

            var ticket = new AuthenticationTicket(identity, properties);
            context.Validated(ticket);
        }

        #endregion
    }
}