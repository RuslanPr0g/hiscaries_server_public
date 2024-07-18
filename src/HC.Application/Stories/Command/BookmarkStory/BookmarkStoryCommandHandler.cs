using System;
using System.Threading;
using System.Threading.Tasks;
using HC.Application.Interface;
using HC.Application.Models.Response;
using HC.Domain.Story;
using MediatR;

namespace HC.Application.Stories.Command.BookmarkStory;

internal class BookmarkStoryCommandHandler : IRequestHandler<BookmarkStoryCommand, UpdateStoryInfoResult>
{
    private readonly IStoryWriteService _storyService;
    private readonly IUserWriteService _userService;

    public BookmarkStoryCommandHandler(IStoryWriteService storyService, IUserWriteService userService)
    {
        _storyService = storyService;
        _userService = userService;
    }

    public async Task<UpdateStoryInfoResult> Handle(BookmarkStoryCommand request, CancellationToken cancellationToken)
    {
        StoryBookMark bookMark = new StoryBookMark
        {
            User = new User
            {
                Id = request.UserId
            },
            Story = new Story
            {
                Id = request.StoryId
            },
            DateAdded = DateTime.Now
        };

        return await _storyService.BookmarkStory(bookMark, request.User);
    }
}