using System;
using System.Collections.Generic;

namespace GeoChallenger.Services.Interfaces.DTO.Users
{
    /// <summary>
    ///     Carry User enttiy
    /// </summary>
    public class UserDto
    {
        /// <summary>
        ///     User Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     User Full Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     User created date in UTC
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>
        ///     User Accounts. Relationship property
        /// </summary>
        public List<AccountDto> Accounts { get; set; }
    }
}
