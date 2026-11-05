using LeagueHub.Models;
using LeagueHub.Repositories;

namespace LeagueHub.Tests.Fakes;

public class FakeMatchRepository : IMatchRepository
{
    private readonly List<Match> _matches = new();
    private int _nextId = 1;

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
        if (match.Id == 0)
        {
            match.Id = _nextId++;
        }

        _matches.Add(match);
        return match;
    }

    public void Remove(Match match)
    {
        _matches.Remove(match);
    }
}
