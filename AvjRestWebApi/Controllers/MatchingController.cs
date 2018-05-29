using AvjRestWebApi.DataCache;
using DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AvjRestWebApi.Controllers
{
    public class MatchingController : ApiController
    {
        [HttpPost]
        [Route("")]
        public UserModel UserMatching (BjModel bj, UserModel user)
        {
            //user.Id = Guid.NewGuid();
            //UsersController.Users.Add(user);

            var rankBjModels = RankBjDataCache.Instance.GetRankBjModels;

            var rankUserModels = RankUserModelDataCache.Instance.GetRankUserModels;


            return user;
        }
    }
}