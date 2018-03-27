using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace FriendProject.Data
{
    public class UserRepo
    {
        private static readonly Lazy<UserRepo> LazyInstance = new Lazy<UserRepo>(() => new UserRepo());


        public static UserRepo Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        public UserRepo()
        {
            this.users = new Dictionary<int, User>();
        }

        private readonly IDictionary<int, User> users;

        public void AddUser(User user)
        {
            if (null == user)
                throw new ArgumentNullException();

            users.Add(user.Id, user);

            userGraph.AddVertex(user);
        }

        public User FindById(int id)
        {
            if (users.ContainsKey(id))
                return users[id];

            throw new KeyNotFoundException();
        }

        public IEnumerable<User> GetAll()
        {
            return this.users.Values.ToList();
        }


        /*###########################
            editing code from here on
            #########################*/

        private readonly Graph<User> userGraph = new Graph<User>();

        /// <summary>
        /// Add a relationship between two users
        /// </summary>
        /// <param name="userId">The user id of the person adding a friend</param>
        /// <param name="friendUserId">The user ID of the friend being added</param>
        public void AddFriendship(int userId, int friendUserId)
        {
            var tuple = new Tuple<User, User>(users[userId], users[friendUserId]);
            if (tuple == null || userId == friendUserId)
                throw new ArgumentNullException();

            userGraph.AddEdge(tuple);
        }

        /// <summary>
        /// Delete a relationship between two users who have a relationship
        /// </summary>
        /// <param name="userId">The user id of the person deleting a friend</param>
        /// <param name="friendUserId">The user ID of the friend being deleted</param>
        public void DeleteFriendship(int userId, int friendUserId)
        {
            var tuple = new Tuple<User, User>(users[userId], users[friendUserId]);
            if (tuple == null || userId == friendUserId)
                throw new ArgumentNullException();

            userGraph.DeleteEdge(tuple);
        }

        /// <summary>
        /// List friends of user
        /// </summary>
        /// <param name="userId">User whose friends will be listed</param>
        /// <returns>A list of Users who are friends of the User</returns>
        public HashSet<User> GetFriendList(int userId)
        {
            var startingVertex = users[userId];
            return userGraph.AdjacencyList[startingVertex];
        }

        /// <summary>
        /// Find potential friends of users.
        /// </summary>
        /// <remarks>Method containing Breadth-First search algorithm</remarks>
        /// <param name="userId">User whose friends of friends will be listed</param>
        /// <returns>A List of Users who are friends of the friends of the User</returns>
        public HashSet<User> GetPotentialFriendsList(int userId)
        {
            var startingVertex = users[userId];
            var visited = new HashSet<User>();

            if (!userGraph.AdjacencyList.ContainsKey(startingVertex))
                return visited;

            var queue = new Queue<User>();
            queue.Enqueue(startingVertex);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();

                if (visited.Contains(vertex))
                    continue;

                if (vertex != startingVertex)
                    visited.Add(vertex);


                foreach (var neighbor in userGraph.AdjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        queue.Enqueue(neighbor);
            }

            var result = new HashSet<User>();

            foreach (var friend in visited)
                if (!userGraph.AdjacencyList[startingVertex].Contains(friend))
                    result.Add(friend);

            return result;
        }

        /// <summary>
        /// Method to find the path between 2 people
        /// </summary>
        /// <param name="userId">The user id of the person from who the path starts</param>
        /// <param name="friendUserId">The user id of the person to who the path starts</param>
        /// <returns>A List of users from user to other user</returns>
        public HashSet<User> FindPathToFriendList(int userId, int friendUserId)
        {
            var previous = new Dictionary<User, User>();
            var startingVertex = users[userId];

            var queue = new Queue<User>();
            queue.Enqueue(startingVertex);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbor in userGraph.AdjacencyList[vertex])
                {
                    if (previous.ContainsKey(neighbor))
                        continue;

                    previous[neighbor] = vertex;
                    queue.Enqueue(neighbor);
                }
            }


            var path = new HashSet<User> { };

            var current = users[friendUserId];
            while (!current.Equals(startingVertex))
            {
                path.Add(current);
                current = previous[current];
            };

            path.Add(startingVertex);

            // Changing to list to reverse order. Can't reverse HashSet
            var pathList = path.ToList<User>();
            pathList.Reverse();
            path = new HashSet<User>(pathList);

            return path;
        }

        /* thought I needed this, but I don't. Commenting out in case I think of a use case
        public HashSet<User> DepthFirstSearch(int userId)
        {
            var startingVertex = users[userId];
            var visited = new HashSet<User>();

            if (!userGraph.AdjacencyList.ContainsKey(startingVertex))
                return visited;

            var stack = new Stack<User>();
            stack.Push(startingVertex);

            while (stack.Count > 0)
            {
                var vertex = stack.Pop();

                if (visited.Contains(vertex))
                    continue;

                visited.Add(vertex);

                foreach (var neighbor in userGraph.AdjacencyList[vertex])
                    if (!visited.Contains(neighbor))
                        stack.Push(neighbor);
            }

            return visited;
        }
        */

    }
}