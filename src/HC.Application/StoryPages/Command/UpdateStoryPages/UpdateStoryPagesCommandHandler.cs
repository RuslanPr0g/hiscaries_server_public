﻿using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.StoryPages.Command.CreateStoryPages;

public class UpdateStoryPagesCommandHandler : IRequestHandler<UpdateStoryPagesCommand, AddStoryPageResult>
{
    private readonly IStoryWriteService _storyService;

    public UpdateStoryPagesCommandHandler(IStoryWriteService storyPageService)
    {
        _storyService = storyPageService;
    }

    public async Task<AddStoryPageResult> Handle(UpdateStoryPagesCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.UpdatePages(request);
    }
}