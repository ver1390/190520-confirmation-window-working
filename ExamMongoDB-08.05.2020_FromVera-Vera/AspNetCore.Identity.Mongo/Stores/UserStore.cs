﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AspNetCore.Identity.Mongo.Stores
{
    public class UserStore<TUser, TRole> :
        IUserClaimStore<TUser>,
        IUserLoginStore<TUser>,
        IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>,
     

        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IQueryableUserStore<TUser>
        
     


        where TUser : MongoUser
        where TRole : MongoRole
    {
        private readonly IRoleStore<TRole> _roleStore;

        private readonly IMongoCollection<TUser> _userCollection;

        private readonly ILookupNormalizer _normalizer;

        private static readonly InsertOneOptions InsertOneOptions = new InsertOneOptions();

        private static readonly FindOptions<TUser> FindOptions = new FindOptions<TUser>();

        private static readonly ReplaceOptions ReplaceOptions = new ReplaceOptions();

        public UserStore(IMongoCollection<TUser> userCollection, IRoleStore<TRole> roleStore, ILookupNormalizer normalizer)
        {
            _userCollection = userCollection;
            _roleStore = roleStore;
            _normalizer = normalizer;
        }

        public IQueryable<TUser> Users
        {
            get
            {
                var task = _userCollection.All();
                Task.WaitAny(task);
                return task.Result.AsQueryable();
            }
        }

        private async Task Update<TFIELD>(TUser user, Expression<Func<TUser, TFIELD>> expression, TFIELD value)
        {
            var upd = Builders<TUser>.Update.Set(expression, value);
            await _userCollection.UpdateOneAsync(x => x.Id == user.Id, upd).ConfigureAwait(false);
        }

        private async Task Add<TFIELD>(TUser user, Expression<Func<TUser, IEnumerable<TFIELD>>> expression, TFIELD value)
        {
            var upd = Builders<TUser>.Update.AddToSet(expression, value);
            await _userCollection.UpdateOneAsync(x => x.Id == user.Id, upd).ConfigureAwait(false);
        }

        private Task<TUser> ById(ObjectId id)
        {
            return _userCollection.FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var u = await _userCollection.FirstOrDefaultAsync(x => x.UserName == user.UserName).ConfigureAwait(true);
            if (u != null) return IdentityResult.Failed(new IdentityError { Code = "Username already in use" });

            await _userCollection.InsertOneAsync(user, InsertOneOptions, cancellationToken).ConfigureAwait(false);

            if (user.Email != null)
            {
                await SetEmailAsync(user, user.Email, cancellationToken).ConfigureAwait(false);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _userCollection.DeleteOneAsync(x => x.Id == user.Id, cancellationToken).ConfigureAwait(false);
            return IdentityResult.Success;
        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return ById(ObjectId.Parse(userId));
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _userCollection.FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUserName);
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await SetEmailAsync(user, user.Email, cancellationToken).ConfigureAwait(false);
            await _userCollection.ReplaceOneAsync(x => x.Id == user.Id, user, ReplaceOptions, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var claim in claims)
            {
                var identityClaim = new IdentityUserClaim<string>()
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };

                user.Claims.Add(identityClaim);
                await Add(user, x => x.Claims, identityClaim).ConfigureAwait(false);
            }
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var claims = user.Claims;

            claims.RemoveAll(x => x.ClaimType == claim.Type);
            claims.Add(new IdentityUserClaim<string>()
            {
                ClaimType = newClaim.Type,
                ClaimValue = newClaim.Value
            });
            user.Claims = claims;


            await Update(user, x => x.Claims, claims).ConfigureAwait(false);
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var claim in claims)
            {
                user.Claims.RemoveAll(x => x.ClaimType == claim.Type);
            }

            return Update(user, x => x.Claims, user.Claims);
        }

        public async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return (await _userCollection.WhereAsync(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value)))
                        .ToList();

        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.NormalizedUserName ?? _normalizer.NormalizeName(user.UserName));
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user?.Id.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.UserName);
        }

        public async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dbUser = await ById(user.Id).ConfigureAwait(true);
            return dbUser?.Claims?.Select(x => new Claim(x.ClaimType, x.ClaimValue))?.ToList() ?? new List<Claim>();
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            var name = normalizedName ?? _normalizer.NormalizeName(user.UserName);

            user.NormalizedUserName = name;
            return Update(user, x => x.NormalizedUserName, name);
        }

        public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await SetNormalizedUserNameAsync(user, _normalizer.NormalizeName(userName), cancellationToken)
                .ConfigureAwait(false);

            user.UserName = userName;
            await Update(user, x => x.UserName, userName).ConfigureAwait(false);
        }

        void IDisposable.Dispose()
        {
        }

        public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return (await ById(user.Id))?.Email ?? user.Email;
        }

        public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return (await ById(user.Id))?.EmailConfirmed ?? user.EmailConfirmed;
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _userCollection.FirstOrDefaultAsync(a => a.NormalizedEmail == normalizedEmail);
        }

        public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return (await ById(user.Id).ConfigureAwait(true))?.NormalizedEmail ?? user.NormalizedEmail;
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Update(user, x => x.EmailConfirmed, confirmed);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.NormalizedEmail = normalizedEmail ?? _normalizer.NormalizeEmail(user.Email);
            return Update(user, x => x.NormalizedEmail, user.NormalizedEmail);
        }

        public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await SetNormalizedEmailAsync(user, _normalizer.NormalizeEmail(user.Email), cancellationToken)
                .ConfigureAwait(false);
            user.Email = email;

            await Update(user, x => x.Email, user.Email).ConfigureAwait(false);
        }

      


        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var iul = new IdentityUserLogin<string>
            {
                UserId = user.Id.ToString(),
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey
            };

            user.Logins.Add(iul);

            return Add(user, x => x.Logins, iul);
        }

        public async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Logins.RemoveAll(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);

            await Update(user, x => x.Logins, user.Logins).ConfigureAwait(false);
        }

        public async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userCollection.FirstOrDefaultAsync(u =>
                u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey))
                .ConfigureAwait(true);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dbUser = await ById(user.Id).ConfigureAwait(true);
            return dbUser?.Logins?.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))?.ToList() ?? new List<UserLoginInfo>();
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return (await ById(user.Id).ConfigureAwait(true))?.PasswordHash != null;
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.PasswordHash = passwordHash;
            return Update(user, x => x.PasswordHash, passwordHash);
        }

        public async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return (await ById(user.Id).ConfigureAwait(true))?.PhoneNumber;
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return (await ById(user.Id).ConfigureAwait(true))?.PhoneNumberConfirmed ?? false;
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.PhoneNumber = phoneNumber;
            return Update(user, x => x.PhoneNumber, phoneNumber);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.PhoneNumberConfirmed = confirmed;
            return Update(user, x => x.PhoneNumberConfirmed, confirmed);
        }

        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken).ConfigureAwait(true);
            if (role == null) return;

            user.Roles.Add(role.Id.ToString());

            await Update(user, x => x.Roles, user.Roles).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken).ConfigureAwait(true);
            if (role == null) return;

            user.Roles.Remove(role.Id.ToString());

            await Update(user, x => x.Roles, user.Roles).ConfigureAwait(false);
        }


        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken).ConfigureAwait(true);
            if (role == null) return new List<TUser>();

            var filter = Builders<TUser>.Filter.AnyEq(x => x.Roles, role.Id.ToString());
            return (await _userCollection.FindAsync(filter, FindOptions, cancellationToken).ConfigureAwait(true)).ToList();
        }

        public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userDb = await ById(user.Id).ConfigureAwait(true);
            if (userDb == null) return new List<string>();

            var roles = new List<string>();

            foreach (var item in userDb.Roles)
            {
                var dbRole = await _roleStore.FindByIdAsync(item, cancellationToken).ConfigureAwait(true);

                if (dbRole != null)
                {
                    roles.Add(dbRole.Name);
                }
            }
            return roles;
        }

        public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dbUser = await ById(user.Id).ConfigureAwait(true);

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken)
                .ConfigureAwait(true);

            if (role == null) return false;

            return dbUser?.Roles.Contains(role.Id.ToString()) ?? false;
        }

      
    }
}
