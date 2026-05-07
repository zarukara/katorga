namespace ServiceSystem
{
    public interface ISaver
    {
        void SaveScore(int score, string path = null);

        int LoadScore(string path = null);
    }
}