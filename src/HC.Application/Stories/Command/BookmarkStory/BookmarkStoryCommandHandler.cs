using HC.Application.Interface;
using HC.Application.Models.Response;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace HC.Application.Stories.Command.BookmarkStory;

internal class BookmarkStoryCommandHandler : IRequestHandler<BookmarkStoryCommand, BaseResult>
{
    private readonly IStoryWriteService _storyService;
    private readonly IUserWriteService _userService;

    public BookmarkStoryCommandHandler(IStoryWriteService storyService, IUserWriteService userService)
    {
        _storyService = storyService;
        _userService = userService;
    }

    public async Task<BaseResult> Handle(BookmarkStoryCommand request, CancellationToken cancellationToken)
    {
        return await _storyService.BookmarkStory(request);
    }
}