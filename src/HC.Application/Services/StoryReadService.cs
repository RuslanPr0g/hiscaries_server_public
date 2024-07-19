using HC.Application.Interface;
using HC.Application.Stories.Query;
using HC.Domain.Stories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public sealed class StoryReadService : IStoryReadService
{
    private readonly IStoryReadRepository _repository;

    public StoryReadService(IStoryReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<GenreReadModel>> GetAllGenres() => await _repository.GetAllGenres();

    public async Task<StoryReadModel> GetStoryById(StoryId storyId)
    {
        var result = await _repository.GetStory(storyId);

        await SetAudioForStory(result);

        return result;
    }

    public async Task<IEnumerable<StorySimpleReadModel>> GetStoryRecommendations(GetStoryRecommendationsQuery request) =>
        await _repository.GetStoryRecommendations(request.Username);

    public async Task<IEnumerable<StorySimpleReadModel>> SearchForStory(GetStoryListQuery request) =>
        await _repository.GetStoriesBy(request.SearchTerm, request.Genre);

    private async Task SetAudioForStory(StoryReadModel story)
    {
        IEnumerable<StoryAudioReadModel> fileIds = story.Audios;

        StoryAudioReadModel? audio = fileIds.FirstOrDefault();

        if (audio is null)
        {
            return;
        }

        byte[] result = await System.IO.File.ReadAllBytesAsync("audios/" + audio.Id + ".mp3");

        story.Audio = result;
        audio.Bytes = result;
    }
}