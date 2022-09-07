namespace LinkShortener.Pl.Interfaces
{
    public interface ILinkService
    {
        public string StoreLongLink(string @long);
        public string GetLongLink(string @short);
    }
}