namespace GeoChallenger.Services.Interfaces.DTO.Users
{
    /// <summary>
    ///     Carry Account entity
    /// </summary>
    public class AccountDto
    {
        /// <summary>
        ///     Social network account unique identificator
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        ///     Social account type
        /// </summary>
        public AccountTypeDto Type { get; set; }
    }
}