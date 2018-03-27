using System;
using System.Collections.Generic;

namespace FriendProject.Data
{
    /// <summary>
    /// Custom Graph class
    /// </summary>
    /// <typeparam name="User"></typeparam>
    /// <remarks>Class needs to be created because .Net doesn't have a builtin graph class</remarks>
    public class Graph<User>
    {
        /// <summary>
        /// Constructor for Graph
        /// </summary>
        public Graph() { }

        /// <summary>
        /// Graph struct
        /// </summary>
        /// <param name="vertices">Nodes of Users</param>
        /// <param name="edges">Relationships between Users</param>
        public Graph(IEnumerable<User> vertices, IEnumerable<Tuple<User, User>> edges)
        {
            foreach (var vertex in vertices)
                AddVertex(vertex);

            foreach (var edge in edges)
                AddEdge(edge);
        }

        /// <summary>
        /// New graph
        /// </summary>
        public Dictionary<User, HashSet<User>> AdjacencyList { get; } = new Dictionary<User, HashSet<User>>();

        /// <summary>
        /// Method to add a user to the graph
        /// </summary>
        /// <param name="vertex"></param>
        public void AddVertex(User vertex)
        {
            AdjacencyList[vertex] = new HashSet<User>();
        }

        /// <summary>
        /// Method to Add a relationship between two users
        /// </summary>
        /// <param name="edge">The relationship</param>
        public void AddEdge(Tuple<User, User> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
            }
        }

        /// <summary>
        /// Method to Delete a relationship between two users
        /// </summary>
        /// <param name="edge">The relationship.</param>
        public void DeleteEdge(Tuple<User, User> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Remove(edge.Item2);
                AdjacencyList[edge.Item2].Remove(edge.Item1);
            }
        }
    }
}