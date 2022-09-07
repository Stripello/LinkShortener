namespace LinkShortener.Dal.Models;

public class LinkDto
{
	public int Id { get; set; }
    public string LongLink { get; set; }
	public long HashOfLongLink { get; set; }
	public ushort LastCallYear { get; set; }
}