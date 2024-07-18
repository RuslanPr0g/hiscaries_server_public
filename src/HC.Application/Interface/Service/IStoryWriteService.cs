﻿using HC.Application.Models.Response;
using HC.Application.Stories.Command;
using HC.Application.Stories.Command.DeleteStory;
using HC.Application.Stories.Command.ReadStory;
using HC.Application.Stories.Command.ScoreStory;
using HC.Domain.Stories;
using System.Threading.Tasks;

namespace HC.Application.Interface;

public interface IStoryWriteService
{
    Task<Story> GetStoryById(StoryId storyId);
    Task<UpdateStoryInfoResult> PublishStory(CreateStoryCommand command);
    Task<BaseResult> DeleteStory(DeleteStoryCommand command);
    Task<BaseResult> DeleteComment(DeleteCommentCommand command);
    Task<UpdateStoryInfoResult> UpdateStory(UpdateStoryCommand command);
    Task<BaseResult> AddComment(AddCommentCommand command);
    Task<BaseResult> AddStoryScore(StoryScoreCommand command);
    Task<UpdateStoryInfoResult> BookmarkStory(BookmarkStoryCommand command);
    Task<BaseResult> AddImageToStory(StoryId storyId, string imagePath);
    Task<BaseResult> AddImageToStoryByBase64(StoryId storyId, byte[] base64);
    Task<BaseResult> ReadStoryHistory(ReadStoryCommand command);
    Task<BaseResult> CreateGenre(CreateGenreCommand request);
    Task<BaseResult> UpdateGenre(UpdateGenreCommand request);
    Task<BaseResult> DeleteGenre(DeleteGenreCommand request);
    Task<BaseResult> DeleteAudio(DeleteStoryAudioCommand request);
    Task<BaseResult> UpdateAudio(UpdateStoryAudioCommand request);
}