using BehavioralAlgorithms.Behaviors;
using BehavioralAlgorithms.Interfaces;
using Microsoft.Extensions.Options;
using WebApiApplication.Options;

namespace WebApiApplication.Controllers
{
    public class BFSController : BaseController
    {
        public BFSController(IOptions<SnakeOptions> options, IMoveBehavior<BFSBehavior> behavior)
            : base(options, behavior)
        {
        }
    }
}
