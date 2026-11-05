using LeagueHub.Exceptions;
using LeagueHub.Models;
using LeagueHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeagueHub.Controllers;

[ApiController]
[Route("api/teams")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teams;

    public TeamsController(ITeamService teams)
    {
        _teams = teams;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_teams.GetAll());
    }

    [HttpPost]
    public IActionResult Create(Team team)
    {
        try
        {
            Team created = _teams.Create(team);
            return Created($"/api/teams/{created.Id}", created);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
