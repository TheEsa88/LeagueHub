using Microsoft.AspNetCore.Mvc;

namespace LeagueHub.Controllers;

[ApiController]
[Route("api")]
public class LeagueController : ControllerBase
{
    private static readonly List<Team> _teams = new()
    {
        new Team { Id = 1, Name = "Mikkelin Maila", City = "Mikkeli", MaxRoster = 12 },
        new Team { Id = 2, Name = "Kouvolan Kiekko", City = "Kouvola", MaxRoster = 12 },
        new Team { Id = 3, Name = "Savonlinnan Sähly", City = "Savonlinna", MaxRoster = 10 }
    };

    private static readonly List<Match> _matches = new()
    {
        new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2, ScheduledAt = DateTime.UtcNow.AddDays(-2) },
        new Match { Id = 2, HomeTeamId = 2, AwayTeamId = 3, ScheduledAt = DateTime.UtcNow.AddDays(7) }
    };

    private static int _nextTeamId = 4;
    private static int _nextMatchId = 3;

    [HttpGet("teams")]
    public IActionResult GetTeams()
    {
        return Ok(_teams);
    }

    [HttpPost("teams")]
    public IActionResult CreateTeam(Team team)
    {
        if (string.IsNullOrWhiteSpace(team.Name))
        {
            return BadRequest("Joukkueen nimi on pakollinen.");
        }

        if (team.MaxRoster <= 0)
        {
            return BadRequest("maxRoster pitää olla positiivinen.");
        }

        team.Id = _nextTeamId++;
        _teams.Add(team);
        return Created($"/api/teams/{team.Id}", team);
    }

    [HttpGet("matches")]
    public IActionResult GetMatches()
    {
        return Ok(_matches);
    }

    [HttpPost("matches")]
    public IActionResult CreateMatch(Match match)
    {
        if (match.HomeTeamId == match.AwayTeamId)
        {
            return BadRequest("Koti ja vieras eivät voi olla sama joukkue.");
        }

        if (_teams.All(t => t.Id != match.HomeTeamId) || _teams.All(t => t.Id != match.AwayTeamId))
        {
            return BadRequest("Molempien joukkueiden pitää olla olemassa.");
        }

        match.Id = _nextMatchId++;
        _matches.Add(match);
        return Created($"/api/matches/{match.Id}", match);
    }

    [HttpPost("matches/{id}/result")]
    public IActionResult RecordResult(int id, ResultRequest request)
    {
        Match? match = _matches.FirstOrDefault(m => m.Id == id);

        if (match == null)
        {
            return NotFound("Ottelua ei löydy.");
        }

        if (match.HomeGoals is not null)
        {
            return BadRequest("Tulos on jo kirjattu.");
        }

        if (match.ScheduledAt > DateTime.UtcNow)
        {
            return BadRequest("Ottelu ei ole vielä pelattu.");
        }

        if (request.HomeGoals < 0 || request.AwayGoals < 0)
        {
            return BadRequest("Maalit eivät voi olla negatiivisia.");
        }

        match.HomeGoals = request.HomeGoals;
        match.AwayGoals = request.AwayGoals;
        return Ok(match);
    }

    [HttpGet("standings")]
    public IActionResult GetStandings()
    {
        List<object> rows = new();

        foreach (Team team in _teams)
        {
            int wins = 0;
            int draws = 0;
            int losses = 0;

            foreach (Match match in _matches)
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

            rows.Add(new
            {
                team.Id,
                team.Name,
                Wins = wins,
                Draws = draws,
                Losses = losses,
                Points = wins * 3 + draws
            });
        }

        return Ok(rows.OrderByDescending(r => ((dynamic)r).Points));
    }
}

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int MaxRoster { get; set; }
}

public class Match
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
}

public class ResultRequest
{
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
}
