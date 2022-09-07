using LinkShortener.Pl.Interfaces;

namespace LinkShortener.Pl
{
    public class Hasher : IHasher
    {
        public long GetHashCode(string incomingString)
        {
            long answer = 0;
            var pow = 1;
            //difference between ASCII max element and min element allowed for URL
            const int powStep = 93;
            const int minASCIIValue = 33;
            foreach (char c in incomingString)
            {
                answer += ((int)c - minASCIIValue) * pow;
                pow *= powStep;
            }
            return answer;
        }
    }
}