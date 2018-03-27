using FriendProject.Data;
using FriendProject.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FriendProject.Controllers
{
    /// <summary>
    /// Friends RESTful API Controller
    /// </summary>
    public class FriendsController : ApiController
    {
        /// <summary>
        /// Add a relationship between two users
        /// </summary>
        /// <param name="userId">The user id of the person adding a friend</param>
        /// <param name="friendUserId">The user ID of the friend being added</param>
        [HttpPost, Route("friends/{userId}/{friendUserId}")]
        public void AddFriend(int userId, int friendUserId)
        {
            UserRepo.Instance.AddFriendship(userId, friendUserId);
        }

        /// <summary>
        /// Delete a relationship between two users who have a relationship
        /// </summary>
        /// <param name="userId">The user id of the person deleting a friend</param>
        /// <param name="friendUserId">The user ID of the friend being deleted</param>
        [HttpDelete, Route("friends/{userId}/{friendUserId}")]
        public void DeleteFriend(int userId, int friendUserId)
        {
            UserRepo.Instance.DeleteFriendship(userId, friendUserId);
        }

        /// <summary>
        /// List friends of user
        /// </summary>
        /// <param name="userId">User whose friends will be listed</param>
        /// <returns>A list of Users who are friends of the User</returns>
        [HttpGet, Route("friends/{userId}")]
        public List<UserResponse> GetFriends(int userId)
        {
            var query = from u in UserRepo.Instance.GetFriendList(userId)
                        select new UserResponse(u);

            return query.ToList();
        }

        /// <summary>
        /// List of friends of User's friends
        /// </summary>
        /// <param name="userId">User whose friends of friends will be listed</param>
        /// <returns>A List of Users who are friends of the friends of the User</returns>
        [HttpGet, Route("friends/ofFriends/{userId}")]
        public List<UserResponse> GetFriendsOfFriends(int userId)
        {
            var query = from u in UserRepo.Instance.GetPotentialFriendsList(userId)
                        select new UserResponse(u);

            return query.ToList();
        }

        /// <summary>
        /// Find the path from the user to another user.
        /// </summary>
        /// <param name="userId">The user id of the person from who the path starts</param>
        /// <param name="friendUserId">The user id of the person to who the path starts</param>
        /// <returns>A List of users from user to other user</returns>
        [HttpGet, Route("friends/path/{userId}/{friendUserId}")]
        public List<UserResponse> FindFriendPath(int userId, int friendUserId)
        {
            var query = from u in UserRepo.Instance.FindPathToFriendList(userId, friendUserId)
                        select new UserResponse(u);

            return query.ToList();
        }

    }
}
