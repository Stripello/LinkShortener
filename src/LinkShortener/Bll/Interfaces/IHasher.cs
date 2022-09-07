namespace LinkShortener.Pl.Interfaces
{
    public interface IHasher
    {
        public long GetHashCode(string incomingString);
    }
}