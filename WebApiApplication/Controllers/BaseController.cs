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
    [Route("[Controller]")]
    public abstract class BaseController : ControllerBase
    {
        private readonly IOptions<SnakeOptions> _options;
        private readonly IMoveBehavior _behavior;

        public BaseController(IOptions<SnakeOptions> options, IMoveBehavior behavior)
        {
            _options = options;
            _behavior = behavior;
        }

        [HttpGet("ping")]
        public virtual IActionResult Ping()
        {
            return Ok($"Controller type is => {GetType()}");
        }


        [HttpPost("start")]
        [HttpPost("{name}/start")]
        public virtual IActionResult Start([FromBody]StartRequest request, [FromRoute] string name)
        {
            try
            {
                _behavior.Init(request.GameId, request.Height, request.Width);

                // Если имя змейки не передали в параметрах, то берем из конфигов
                name = string.IsNullOrEmpty(name) ? _options.Value.Name : name;

                return Ok(new StartResponse
                {
                    Name = name,
                    Color = _options.Value.Color,
                    HeadType = _options.Value.HeadType,
                    HeadUrl = _options.Value.HeadUrl,
                    SecondaryColor = _options.Value.SecondaryColor,
                    TailType = _options.Value.TailType,
                    Taunt = _options.Value.Taunt
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вызове Start => {ex.Message}");
                return Ok();
            }
        }

        [HttpPost("move")]
        [HttpPost("{_}/move")] // Для универсальности ¯\_(ツ)_/¯
        public virtual IActionResult Move([FromBody] MoveDto move)
        {
            try
            {
                var startTime = DateTime.Now;
                var result = _behavior.Move(MoveDto.MapFromDto(move));

                var diffMilliseconds = (DateTime.Now - startTime).Milliseconds;
                if (diffMilliseconds > 200)
                {
                    Console.WriteLine($"!!!!!!! Вычисление заняло {diffMilliseconds}");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вызове Move => {ex.Message}");
                return Ok();
            }
        }
    }
}