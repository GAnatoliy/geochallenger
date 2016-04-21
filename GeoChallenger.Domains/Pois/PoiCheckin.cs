using System;
using GeoChallenger.Domains.Users;


namespace GeoChallenger.Domains.Pois
{
    /// <summary>
    /// Checkin poi by user.
    /// </summary>
    public class PoiCheckin
    {
        /// <summary>
        /// Constructor for ef.
        /// </summary>
        protected PoiCheckin()
        {
            
        }

        public PoiCheckin(int points, User user, Poi poi)
        {
            Points = points;
            User = user;
            UserId = user.Id;
            Poi = poi;
            PoiId = poi.Id;
        }

        #region Data

        public int Id { get; set; }

        /// <summary>
        /// Number of points for checkin.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Time when poi was checked in.
        /// </summary>
        public DateTime CheckedInAtUtc { get; set; } = DateTime.UtcNow;

        #endregion

        #region Relations

        /// <summary>
        /// Id of a user that checked in point.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User that checked in point.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Id of point that was checked in.
        /// </summary>
        public int PoiId { get; set; }

        /// <summary>
        /// Poi that was checked in.
        /// </summary>
        public Poi Poi { get; set; }
        #endregion
    }
}