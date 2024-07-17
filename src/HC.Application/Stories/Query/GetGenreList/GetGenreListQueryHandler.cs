﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HC.Application.Interface;

using MediatR;

namespace HC.Application.Stories.Query.GetGenreList;

public class GetGenreListQueryHandler : IRequestHandler<GetGenreListQuery, List<Genre>>
{
    private readonly IStoryService _storySevice;

    public GetGenreListQueryHandler(IStoryService storyService)
    {
        _storySevice = storyService;
    }

    public async Task<List<Genre>> Handle(GetGenreListQuery request, CancellationToken cancellationToken)
    {
        return await _storySevice.GetBookMarks(request.User);
    }
}