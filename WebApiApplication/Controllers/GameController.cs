using BehavioralAlgorithms.Behaviors;
using BehavioralAlgorithms.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using WebApiApplication.DTO;
using WebApiApplication.Models.Requests;
using WebApiApplication.Models.Responses;
using WebApiApplication.Options;

namespace WebApiApplication.Controllers
{
    [ApiController]
    [Route("/")]
    public class GameController : ControllerBase
    {
        private readonly IOptions<SnakeOptions> _options;
        private readonly IMoveBehavior<BFSBehavior> _behavior;

        public GameController(IOptions<SnakeOptions> options, IMoveBehavior<BFSBehavior> behavior)
        {
            _options = options;
            _behavior = behavior;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }


        [HttpPost("start")]
        public IActionResult Start([FromBody]StartRequest request)
        {
            try
            {
                _behavior.Init(request.GameId, request.Height, request.Width);

                return Ok(new StartResponse
                {
                    Color = _options.Value.Color,
                    HeadType = _options.Value.HeadType,
                    Name = _options.Value.Name,
                    HeadUrl = _options.Value.HeadUrl,
                    SecondaryColor = _options.Value.SecondaryColor,
                    TailType = _options.Value.TailType,
                    Taunt = _options.Value.Taunt
                });
            }
            catch
            {
                return Ok();
            }
        }

        [HttpPost("move")]
        public IActionResult Move([FromBody] MoveDto move)
        {
            try
            {
                return Ok(_behavior.Move(MoveDto.MapFromDto(move)));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
