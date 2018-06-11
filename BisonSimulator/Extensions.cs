using System;
using System.Collections.Generic;

namespace Sowalabs.Bison.ProfitSim
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a list with a given count of random elements from given collection.
        /// </summary>
        /// <typeparam name="T">Type of elements in collection.</typeparam>
        /// <param name="collection">Collection of elements to select a random subset from.</param>
        /// <param name="count">Number of elements to return.</param>
        /// <returns>A random subset of elements.</returns>
        public static List<T> GetRandomSubset<T>(this ICollection<T> collection, int count)
        {

            var random = new Random();
            var unselected = new List<T>(collection);
            var selected = new List<T>();

            while (selected.Count < count && unselected.Count > 0)
            {
                var randomElement = unselected[random.Next(unselected.Count)];
                unselected.Remove(randomElement);
                selected.Add(randomElement);
            }

            return selected;
        }

    }
}
