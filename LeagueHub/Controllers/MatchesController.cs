using LeagueHub.Exceptions;
using LeagueHub.Models;
using LeagueHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeagueHub.Controllers;

[ApiController]
[Route("api")]
public class MatchesController : ControllerBase
{
    private readonly IMatchService _matches;

    public MatchesController(IMatchService matches)
    {
        _matches = matches;
    }

    [HttpGet("matches")]
    public IActionResult GetAll()
    {
        return Ok(_matches.GetAll());
    }

    [HttpPost("matches")]
    public IActionResult Create(Match match)
    {
        try
        {
            Match created = _matches.Create(match.HomeTeamId, match.AwayTeamId, match.ScheduledAt);
            return Created($"/api/matches/{created.Id}", created);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("matches/{id}/result")]
    public IActionResult RecordResult(int id, ResultRequest request)
    {
        try
        {
            return Ok(_matches.RecordResult(id, request.HomeGoals, request.AwayGoals));
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("standings")]
    public IActionResult Standings()
    {
        return Ok(_matches.GetStandings());
    }
}
