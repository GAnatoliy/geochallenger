namespace GeoChallenger.Web.Api.Models.Users
{
    /// <summary>
    ///     Simple user view model
    /// </summary>
    public class UserReadViewModel
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
        /// Points earned by user.
        /// </summary>
        public uint Points { get; set; }
    }
}