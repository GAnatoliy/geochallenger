using System;
using System.Linq;
using GeoChallenger.Domains.Users;

namespace GeoChallenger.Services.Queries
{
    public static class UsersQueries
    {
        /// <summary>
        ///     Get user by email
        /// </summary>
        /// <param name="query"></param>
        /// <param name="email">User email</param>
        /// <returns></returns>
        public static IQueryable<User> GetUser(this IQueryable<User> query, string email)
        {
            return query.Where(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Get account by acount uid and type
        /// </summary>
        /// <param name="query"></param>
        /// <param name="uid">Account uid</param>
        /// <param name="type">Account type</param>
        /// <returns></returns>
        public static IQueryable<Account> GetAccount(this IQueryable<Account> query, string uid, AccountType type)
        {
            return query.Where(a => a.Uid.Equals(uid, StringComparison.OrdinalIgnoreCase) && a.Type == type);
        } 
    }
}
