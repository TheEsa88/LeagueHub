using LeagueHub.Models;

namespace LeagueHub.Repositories;

public class InMemoryTeamRepository : ITeamRepository
{
    private readonly List<Team> _teams = new()
    {
        new Team { Id = 1, Name = "Mikkelin Maila", City = "Mikkeli", MaxRoster = 12 },
        new Team { Id = 2, Name = "Kouvolan Kiekko", City = "Kouvola", MaxRoster = 12 },
        new Team { Id = 3, Name = "Savonlinnan Sähly", City = "Savonlinna", MaxRoster = 10 }
    };

    private int _nextId = 4;

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
        team.Id = _nextId++;
        _teams.Add(team);
        return team;
    }
}
