namespace LeagueHub.Models;

public class Match
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
}
