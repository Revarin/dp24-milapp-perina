using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kris.Server.Controllers;

public abstract class KrisController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected KrisController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
