using System.Collections.Generic;
using LinkShortener.Dal.Interfaces;
using LinkShortener.Dal.Models;

namespace LinkShortener.Dal;

public class LinkRepositoryStub : ILinkRepository
{
	private readonly List<LinkDto> _data = new ();

    public void RemoveAll(ushort year)
    {
        _data.RemoveAll(x => x.LastCallYear < year);
    }

    public LinkDto Find(long hash, string fullName)
    {
        var intermidiate = _data.FindAll(x => x.HashOfLongLink == hash);
        var notFoundPlug = new LinkDto { Id = -1 };
        if (intermidiate == null)
        {
            return notFoundPlug;
        }
        return intermidiate.Find(x => x.LongLink == fullName) ?? notFoundPlug;
    }

    public LinkDto ElementAt(int id)
    {
        var notFoundPlug = new LinkDto { Id = -1 };
        return _data.Find(x => x.Id == id) ?? notFoundPlug;
    }

    public int Add(LinkDto linkDto)
    {
        var exist = _data.Exists(x => x.Id == linkDto.Id);
        if (exist)
        {
            _data[linkDto.Id] = linkDto;
        }
        else
        {
            _data.Add(linkDto);
        }
        return _data.Count;
    }
}