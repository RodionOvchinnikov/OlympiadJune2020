using BehavioralAlgorithms.Behaviors;
using BehavioralAlgorithms.Interfaces;
using Microsoft.Extensions.Options;
using WebApiApplication.Options;

namespace WebApiApplication.Controllers
{
    public class RandomController : BaseController
    {
        public RandomController(IOptions<SnakeOptions> options, IMoveBehavior<RandomBehavior> behavior)
            : base(options, behavior)
        {
        }
    }
}
