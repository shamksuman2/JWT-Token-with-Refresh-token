using System.Collections.Generic;

namespace AuthorizationServer.Api.Entities
{
    public class UserInfos
    {
        public UserInfos()
        {
            //AuthLevel = new List<UserAuthLevel>();
        }
        public int BCID { get; set; }

        public int CEID { get; set; }

        public int ProfileID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FeatureList { get; set; }

        public bool IsHoneyWell { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        //public List<UserAuthLevel> AuthLevel { get; private set; }

        /// <summary>
        /// Is the current logged in user a dealer user
        /// </summary>        
        public bool IsDealer { get; set; }
    }
}
