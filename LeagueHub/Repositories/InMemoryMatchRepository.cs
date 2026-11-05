using LeagueHub.Models;

namespace LeagueHub.Repositories;

public class InMemoryMatchRepository : IMatchRepository
{
    private readonly List<Match> _matches = new()
    {
        new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2, ScheduledAt = DateTime.UtcNow.AddDays(-2) },
        new Match { Id = 2, HomeTeamId = 2, AwayTeamId = 3, ScheduledAt = DateTime.UtcNow.AddDays(7) }
    };

    private int _nextId = 3;

    public List<Match> GetAll()
    {
        return _matches;
    }

    public Match? GetById(int id)
    {
        return _matches.FirstOrDefault(m => m.Id == id);
    }

    public Match Add(Match match)
    {
        match.Id = _nextId++;
        _matches.Add(match);
        return match;
    }

    public void Remove(Match match)
    {
        _matches.Remove(match);
    }
}
