using LinkShortener.Dal.Models;

namespace LinkShortener.Dal.Interfaces
{
    public interface ILinkRepository
    {
        /// <summary>
        /// Returns DTO object at selected Id;
        /// </summary>
        public LinkDto ElementAt(int id);
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate
        /// </summary>
        public LinkDto Find(long hash, string fullName);
        /// <summary>
        /// Stores data into current DB.
        /// </summary>
        /// <returns>Capacity of sorage.</returns>
        public int Add(LinkDto linkDto);

        /// <summary>
        /// Removes all the elements with last call date older then incoming.
        /// </summary>
        /// <returns>-1 if process of adding element was failed.</returns>
        /// <param name="year">Year after 2000. For example for call 2021 you must enter 21.</param>
        public void RemoveAll(ushort year);
    }
}