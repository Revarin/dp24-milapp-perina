using AutoMapper;

namespace Kris.Server.Core.Handlers;

public abstract class BaseHandler
{
    protected readonly IMapper _mapper;

    protected BaseHandler(IMapper mapper)
    {
        _mapper = mapper;
    }
}
