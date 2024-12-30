namespace UrlShortener.Core;

public class TokenProvider
{
    private static readonly Lock TokenLock = new();
    private long _token = 0;
    public void AssignRange(int start, int end)
    {
        AssignRange(new TokenRange(start, end));
    }

    public void AssignRange(TokenRange tokenRange)
    {
        _token = tokenRange.Start;
    }

    public long GetToken()
    {
        lock (TokenLock)
        {
            return _token++;
        }
    }
}