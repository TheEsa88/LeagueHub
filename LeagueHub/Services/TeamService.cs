using LeagueHub.Exceptions;
using LeagueHub.Models;
using LeagueHub.Repositories;

namespace LeagueHub.Services;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teams;

    public TeamService(ITeamRepository teams)
    {
        _teams = teams;
    }

    public List<Team> GetAll()
    {
        return _teams.GetAll();
    }

    public Team GetById(int id)
    {
        Team? team = _teams.GetById(id);

        if (team is null)
        {
            throw new NotFoundException("Joukkuetta ei löydy.");
        }

        return team;
    }

    public Team Create(Team team)
    {
        if (string.IsNullOrWhiteSpace(team.Name))
        {
            throw new BusinessRuleException("Joukkueen nimi on pakollinen.");
        }

        if (team.MaxRoster <= 0)
        {
            throw new BusinessRuleException("maxRoster pitää olla positiivinen.");
        }

        return _teams.Add(team);
    }
}