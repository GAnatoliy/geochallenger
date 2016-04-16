﻿using System;
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
        ///     User Firts Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     User Last Name
        /// </summary>
        public string LastName { get; set; }

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
