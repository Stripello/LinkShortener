using LinkShortener.Pl.Interfaces;
using LinkShortener.Dal;
using LinkShortener.Dal.Interfaces;
using LinkShortener.Dal.Models;
using Serilog;

namespace LinkShortener.Pl
{

    public class LinkService : ILinkService
    {
        readonly IHasher _hasher;
        readonly ILinkConverter _shortLinkConverter;
        readonly ILinkRepository _dalProvider;

        public LinkService(IHasher hasher, ILinkConverter shortLinkConverter, ILinkRepository dalProvider)
        {
            _hasher = hasher;
            _shortLinkConverter = shortLinkConverter;
            _dalProvider = dalProvider;
        }

        public string GetLongLink(string incomingShortLink)
        {
            try
            {
                return _dalProvider.ElementAt(_shortLinkConverter.LinkToInt(incomingShortLink)).LongLink;
            }
            catch (ArgumentException)
            {
                var errorMessage = $"Dal level error. GetLongLink method. IncomingShortLink = {incomingShortLink}.";
                Log.Information(errorMessage);
                throw new Exception("Dal level error.");
            }
        }

        public string StoreLongLink(string incomingLongLink)
        {
            Console.WriteLine();
            var dto = new LinkDto()
            {   LongLink = incomingLongLink,
                HashOfLongLink = _hasher.GetHashCode(incomingLongLink),
                LastCallYear = (ushort)(DateTime.Now.Year - 2000)
            };
            var Id = _dalProvider.Add(dto);
            return _shortLinkConverter.IntToLink(Id);
        }
    }
}