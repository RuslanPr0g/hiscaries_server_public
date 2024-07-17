﻿using System.Threading;
using System.Threading.Tasks;
using HC.Application.Interface;
using MediatR;

namespace HC.Application.Stories.Command.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, int>
{
    private readonly IStoryWriteService _storyService;

    public AddCommentCommandHandler(IStoryWriteService storyService)
    {
        _storyService = storyService;
    }

    public async Task<int> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.AddComment(request.Comment, request.User);
    }
}