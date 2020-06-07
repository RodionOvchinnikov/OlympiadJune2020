using BehavioralAlgorithms.Behaviors;
using BehavioralAlgorithms.Interfaces;
using Microsoft.Extensions.Options;
using WebApiApplication.Options;

namespace WebApiApplication.Controllers
{
    public class HamiltonianController : BaseController
    {
        public HamiltonianController(IOptions<SnakeOptions> options, IMoveBehavior<HamiltonianBehavior> behavior)
            : base(options, behavior)
        {
        }
    }
}
