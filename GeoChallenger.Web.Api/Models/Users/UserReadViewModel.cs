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
        ///     User Firts Name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     User Last Name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     User geo-location address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     User geo-location latitude
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        ///     User geo-location longitude
        /// </summary>
        public double? Longitude { get; set; }
    }
}