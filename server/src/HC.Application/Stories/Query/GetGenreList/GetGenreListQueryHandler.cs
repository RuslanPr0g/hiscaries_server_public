using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HC.Application.Interface;

using MediatR;

namespace HC.Application.Stories.Query.GetGenreList;

public class GetGenreListQueryHandler : IRequestHandler<GetGenreListQuery, IEnumerable<GenreReadModel>>
{
    private readonly IStoryReadService _storySevice;

    public GetGenreListQueryHandler(IStoryReadService storyService)
    {
        _storySevice = storyService;
    }

    public async Task<IEnumerable<GenreReadModel>> Handle(GetGenreListQuery request, CancellationToken cancellationToken)
    {
        return await _storySevice.GetAllGenres();
    }
}