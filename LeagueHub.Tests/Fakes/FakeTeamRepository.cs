using LeagueHub.Models;
using LeagueHub.Repositories;

namespace LeagueHub.Tests.Fakes;

public class FakeTeamRepository : ITeamRepository
{
    private readonly List<Team> _teams = new();
    private int _nextId = 1;

    public List<Team> GetAll()
    {
        return _teams;
    }

    public Team? GetById(int id)
    {
        return _teams.FirstOrDefault(t => t.Id == id);
    }

    public Team Add(Team team)
    {
        if (team.Id == 0)
        {
            team.Id = _nextId++;
        }

        _teams.Add(team);
        return team;
    }
}
