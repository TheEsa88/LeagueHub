using LeagueHub.Exceptions;
using LeagueHub.Models;
using LeagueHub.Repositories;

namespace LeagueHub.Services;

public class MatchService : IMatchService
{
    private readonly ITeamRepository _teams;
    private readonly IMatchRepository _matches;

    public MatchService(ITeamRepository teams, IMatchRepository matches)
    {
        _teams = teams;
        _matches = matches;
    }

    public List<Match> GetAll()
    {
        return _matches.GetAll();
    }

    public Match GetById(int id)
    {
        Match? match = _matches.GetById(id);

        if (match is null)
        {
            throw new NotFoundException("Ottelua ei löydy.");
        }

        return match;
    }

    public Match Create(int homeTeamId, int awayTeamId, DateTime scheduledAt)
    {
        if (homeTeamId == awayTeamId)
        {
            throw new BusinessRuleException("Koti ja vieras eivät voi olla sama joukkue.");
        }

        if (_teams.GetById(homeTeamId) is null || _teams.GetById(awayTeamId) is null)
        {
            throw new NotFoundException("Molempien joukkueiden pitää olla olemassa.");
        }

        return _matches.Add(new Match
        {
            HomeTeamId = homeTeamId,
            AwayTeamId = awayTeamId,
            ScheduledAt = scheduledAt
        });
    }

    public Match RecordResult(int matchId, int homeGoals, int awayGoals)
    {
        Match match = GetById(matchId);

        if (match.HomeGoals is not null)
        {
            throw new BusinessRuleException("Tulos on jo kirjattu.");
        }

        if (match.ScheduledAt > DateTime.UtcNow)
        {
            throw new BusinessRuleException("Ottelu ei ole vielä pelattu.");
        }

        if (homeGoals < 0 || awayGoals < 0)
        {
            throw new BusinessRuleException("Maalit eivät voi olla negatiivisia.");
        }

        match.HomeGoals = homeGoals;
        match.AwayGoals = awayGoals;
        return match;
    }

    public List<Standing> GetStandings()
    {
        List<Standing> rows = new();

        foreach (Team team in _teams.GetAll())
        {
            int wins = 0;
            int draws = 0;
            int losses = 0;

            foreach (Match match in _matches.GetAll())
            {
                if (match.HomeGoals is null)
                {
                    continue;
                }

                bool isHome = match.HomeTeamId == team.Id;
                bool isAway = match.AwayTeamId == team.Id;

                if (!isHome && !isAway)
                {
                    continue;
                }

                int ours = isHome ? match.HomeGoals.Value : match.AwayGoals!.Value;
                int theirs = isHome ? match.AwayGoals!.Value : match.HomeGoals.Value;

                if (ours > theirs)
                {
                    wins++;
                }
                else if (ours == theirs)
                {
                    draws++;
                }
                else
                {
                    losses++;
                }
            }

            rows.Add(new Standing
            {
                TeamId = team.Id,
                TeamName = team.Name,
                Wins = wins,
                Draws = draws,
                Losses = losses,
                Points = wins * 3 + draws
            });
        }

        return rows.OrderByDescending(r => r.Points).ToList();
    }
}
