public class LeaderboardData
{
    public string Name;
    public LeaderboardPlayerData[] PlayerData;

    public LeaderboardData(string name)
    {
        Name = name;
    }

    public LeaderboardData(string name, LeaderboardPlayerData[] playerData)
    {
        Name = name;
        PlayerData = playerData;
    }
}
