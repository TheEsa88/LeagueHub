using LeagueHub.Models;

namespace LeagueHub.Repositories;

public interface IMatchRepository
{
    List<Match> GetAll();
    Match? GetById(int id);
    Match Add(Match match);
    void Remove(Match match);
}
