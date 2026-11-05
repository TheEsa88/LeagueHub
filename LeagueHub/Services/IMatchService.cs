using LeagueHub.Models;

namespace LeagueHub.Services;

public interface IMatchService
{
    List<Match> GetAll();
    Match GetById(int id);
    Match Create(int homeTeamId, int awayTeamId, DateTime scheduledAt);
    Match RecordResult(int matchId, int homeGoals, int awayGoals);
    List<Standing> GetStandings();
}
