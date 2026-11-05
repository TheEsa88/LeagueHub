using LeagueHub.Models;

namespace LeagueHub.Services;

public interface ITeamService
{
    List<Team> GetAll();
    Team GetById(int id);
    Team Create(Team team);
}
