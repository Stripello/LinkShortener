namespace LinkShortener.Dal
{
    public class LinkRepositorySettings
    {
        internal string _connectionString { get; }

        public LinkRepositorySettings(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}