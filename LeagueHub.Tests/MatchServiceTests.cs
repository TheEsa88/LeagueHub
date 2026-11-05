using LeagueHub.Exceptions;
using LeagueHub.Models;
using LeagueHub.Services;
using LeagueHub.Tests.Fakes;

namespace LeagueHub.Tests;

public class MatchServiceTests
{
    [Fact]
    public void Create_Throws_WhenHomeAndAwayAreTheSameTeam()
    {
        FakeTeamRepository teams = new FakeTeamRepository();
        teams.Add(new Team { Id = 1, Name = "A", MaxRoster = 10 });
        FakeMatchRepository matches = new FakeMatchRepository();
        MatchService service = new MatchService(teams, matches);

        Assert.Throws<BusinessRuleException>(() =>
            service.Create(1, 1, DateTime.UtcNow.AddDays(1)));
    }

    [Fact]
    public void RecordResult_Throws_WhenResultAlreadyExists()
    {
        FakeTeamRepository teams = new FakeTeamRepository();
        teams.Add(new Team { Id = 1, Name = "A", MaxRoster = 10 });
        teams.Add(new Team { Id = 2, Name = "B", MaxRoster = 10 });
        FakeMatchRepository matches = new FakeMatchRepository();
        matches.Add(new Match
        {
            HomeTeamId = 1,
            AwayTeamId = 2,
            ScheduledAt = DateTime.UtcNow.AddDays(-1),
            HomeGoals = 1,
            AwayGoals = 0
        });
        MatchService service = new MatchService(teams, matches);

        Assert.Throws<BusinessRuleException>(() =>
            service.RecordResult(1, 2, 2));
    }
}
