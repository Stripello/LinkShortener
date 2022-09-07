using LinkShortener.Pl.Interfaces;
using Serilog;

namespace LinkShortener.Pl
{
    public class ShortLinkConverter : ILinkConverter
	{
		
		public int LinkToInt(string incomingShortURL)
		{
			if (string.IsNullOrEmpty(incomingShortURL) || ( incomingShortURL.Length > 1 && incomingShortURL[0] == 'A'))
            {
				var errorMessage = $"Bll error. LinkToInt method character out of range. incomingShortUrl = {incomingShortURL}.";
				Log.Information(errorMessage);
				throw new ArgumentException("Bll error.");
			}
			const int numberOfA = 65;
			const int lettersInAlphabet = 26;
			var answer = 0;
			var power = 1;
			foreach (var ch in incomingShortURL.Reverse())
			{
				var currentCharValue = ((int)ch - numberOfA) ;
				var previousValue = answer;
				answer += currentCharValue * power;
				if (currentCharValue < 0 || currentCharValue > 25 ||answer < previousValue)
                {
					var errorMessage = $"Bll error. LinkToInt method character out of range. incomingShortUrl = {incomingShortURL}.";
					Log.Information(errorMessage);
					throw new ArgumentException("Bll error.");
				}
				power *= lettersInAlphabet;
			}
			return answer;
		}
		
		public string IntToLink(int incomingInt)
		{
			if (incomingInt < 0)
            {
				var errorMessage = $"Bll error. IntToLink method incomingInt < 0. incomingInt = {incomingInt}.";
				Log.Information(errorMessage);
				throw new ArgumentException("Bll error.");
			}
			const int numberOfA = 65;
			const int lettersInAlphaber = 26;
			var degree = 0;
			var pow = 1;
			//pretty shure it could be optimized
			while (pow * lettersInAlphaber <= incomingInt)
			{
				degree++;
				pow *= lettersInAlphaber;
			}

			var answer = new List<int>();
			for (; degree >= 0; degree--)
			{
				answer.Add(incomingInt / pow + numberOfA);
				incomingInt %= pow;
				pow /= lettersInAlphaber;
			}
			return new string(answer.Select(x => (char)x).ToArray());
		}
	}
}