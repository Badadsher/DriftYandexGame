public class LeaderboardPlayerData
{
    public string Name;
    public int Score;
    public int Position;
    public string ImgUrl;

    public LeaderboardPlayerData(string name, int score, int position, string imgUrl)
    {
        Name = name;
        Score = score;
        Position = position;
        ImgUrl = imgUrl;
    }
}
