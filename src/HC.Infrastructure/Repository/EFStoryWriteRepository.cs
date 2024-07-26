using HC.Application.Interface;
using HC.Domain.Stories;
using HC.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HC.Infrastructure.Repository;

public sealed class EFStoryWriteRepository : IStoryWriteRepository
{
    private readonly HiscaryContext _context;

    public EFStoryWriteRepository(HiscaryContext context)
    {
        _context = context;
    }

    public async Task<Genre?> GetGenre(GenreId genreId) =>
        await _context.Genres.FirstOrDefaultAsync(x => x.Id == genreId);

    public async Task AddGenre(Genre genre) => await _context.Genres.AddAsync(genre);

    public async Task AddStory(Story story) =>
        await _context.Stories.AddAsync(story);

    public void DeleteGenre(Genre genre) => _context.Genres.Remove(genre);

    public void DeleteStory(Story story) =>
        _context.Stories.Remove(story);

    public async Task<Story?> GetStory(StoryId storyId) =>
        await _context.Stories.FirstOrDefaultAsync(x => x.Id == storyId);
}