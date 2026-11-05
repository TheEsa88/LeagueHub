using LeagueHub.Models;

namespace LeagueHub.Repositories;

public interface ITeamRepository
{
    List<Team> GetAll();
    Team? GetById(int id);
    Team Add(Team team);
}
