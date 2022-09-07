namespace LinkShortener.Pl.Interfaces
{
    public interface ILinkConverter
    {
        /// <summary>
		/// Converts standart short url to int.
		/// </summary>
		/// <param name="incomingShortURL">>Only uppercase english letters allowed. Summaru length less then 7 chars.</param>
		/// <returns>Int value related to recieved link.</returns>
		/// <exception cref="ArgumentException"></exception>
        public int LinkToInt(string incomingShortURL);

		/// <summary>
		/// Converts int to standart short URL.
		/// </summary>
		/// <param name="incomingInt">Must be positive.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public string IntToLink(int incomingInt);
    }
}