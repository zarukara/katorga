namespace ServiceSystem
{
    public interface ISaver
    {
        void SaveScore(string path = null);

        int LoadScore(string path = null);
    }
}
