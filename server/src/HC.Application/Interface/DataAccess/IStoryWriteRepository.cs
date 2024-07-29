using HC.Domain.Stories;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IStoryWriteRepository
{
    Task<Story?> GetStory(StoryId storyId);
    Task AddStory(Story story);
    void DeleteStory(Story story);

    Task<Genre?> GetGenre(GenreId genreId);
    Task AddGenre(Genre genre);
    void DeleteGenre(Genre genre);
}