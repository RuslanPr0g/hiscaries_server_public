﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HC.Application.DTOs;
using HC.Application.Helpers;
using HC.Application.Interface;
using HC.Application.Models.Response;

using HC.Domain.Story;
using HC.Domain.Story.Comment;
using HC.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HC.Application.Services;

public class StoryService : IStoryWriteService
{
    private readonly IMapper _mapper;
    private readonly IStoryWriteRepository _storyRepository;

    public StoryService(IStoryWriteRepository storyRepository, IMapper mapper)
    {
        _storyRepository = storyRepository;
        _mapper = mapper;
    }

    public async Task<int> DeleteStory(int story)
    {
        return await _storyRepository.DeleteStory(story);
    }

    //    Task<IEnumerable<StoryReadModel>> SearchForStory(GetStoryListQuery request);
    //            if (!string.IsNullOrEmpty(searchRequest))
    //            stories = stories.Where(s =>
    //                s.Title.Contains(searchRequest, StringComparison.InvariantCultureIgnoreCase) ||
    //                s.Description.Contains(searchRequest, StringComparison.InvariantCultureIgnoreCase)).ToList();


    //        if (!string.IsNullOrEmpty(genreRequest))
    //        {
    //            List<StoryReadDto> sttoriesWithGenres = new List<StoryReadDto>();

    //            foreach (StoryReadDto story in stories)
    //            {
    //                bool isAny = story.Genres.Any(x =>
    //                    x.Name.Contains(genreRequest, StringComparison.InvariantCultureIgnoreCase)
    //                    || x.Description.Contains(genreRequest, StringComparison.InvariantCultureIgnoreCase));

    //                if (isAny) sttoriesWithGenres.Add(story);
    //}

    //stories = sttoriesWithGenres;
    //        }







    //[HttpGet("audio")]
    //public async Task<IActionResult> GetAudioForStory([FromQuery] int storyId)
    //{
    //    UserConnection user = new(GetCurrentUsername(), GetCurrentHash());

    //    if (user.Username is null)
    //        return BadRequest("Token expired");

    //    List<StoryAudio> audioModels = await _storyRepository.GetAudio(storyId, user);
    //    StoryAudio story = audioModels.FirstOrDefault();

    //    byte[] result = await System.IO.File.ReadAllBytesAsync("audios/" + story?.FileId + ".mp3");

    //    List<GetStoryFilesRequest> storyFilesRead = audioModels.Select(x =>
    //        new GetStoryFiles(x.Id, x.FileId, x.DateAdded, x.Name, result)).ToList();

    //    return Ok(storyFilesRead);
    //}

    public async Task<List<StoryReadDto>> GetAllStories()
    {
        IReadOnlyCollection<Story> stories = await _storyRepository.GetStories();
        List<StoryReadDto> mappedStories = _mapper.Map<List<StoryReadDto>>(stories);
        return await SetStoryInfo(mappedStories);
    }

    public async Task<Story> GetStoryById(int storyId)
    {
        return await _storyRepository.GetStory(storyId);
    }

    public async Task<UpdateStoryInfoResult> PublishStory(Story story)
    {
        int publishedStoryId = await _storyRepository.AddStory(story);
        return new UpdateStoryInfoResult(ResultStatus.Success, string.Empty, publishedStoryId);
    }

    public async Task AddImageToStory(int storyId, string imagePath)
    {
        Story story = await _storyRepository.GetStory(storyId);
        story.SetImage(ImageHelper.ImageToByteArrayFromFilePath(imagePath));
        await _storyRepository.UpdateStory(story);
    }

    public async Task AddImageToStoryByBase64(int storyId, byte[] base64)
    {
        Story story = await _storyRepository.GetStory(storyId);
        story.SetImage(base64);
        await _storyRepository.UpdateStory(story);
    }

    public async Task<int> ReadStoryHistory(StoryReadHistory story)
    {
        List<StoryReadHistory> reads = await _storyRepository.GetStoryReadHistory();
        IEnumerable<StoryReadHistory> exists = reads.Where(x => x.Story.Id == story.Story.Id
                                                                && x.User.Id == story.User.Id
                                                                && x.PageRead == story.PageRead);
        if (exists.Any()) return 1;
        return await _storyRepository.ReadStoryHistory(story);
    }

    public async Task<List<Genre>> GetBookMarks()
    {
        return await _storyRepository.GetGenres();
    }

    public async Task<List<StoryRating>> GetScores()
    {
        return await _storyRepository.GetStoryScores();
    }

    public async Task UpdateStoryScore(StoryRating storyScore)
    {
        await _storyRepository.UpdateStoryScore(storyScore);
    }

    public async Task AddStoryScore(StoryRating storyScore)
    {
        await _storyRepository.InsertStoryScore(storyScore);
    }

    public async Task<List<StoryReadHistoryProgress>> GetHistory(int userId)
    {
        List<StoryReadHistoryProgress> reads = await _storyRepository.GetStoryReadHistoryProgress(userId);
        foreach (StoryReadHistoryProgress read in reads)
            if (read.Total != 0)
                read.UpdatePercentage(read.Count / (double)read.Total);
            else
                read.UpdatePercentage(1);
        return reads;
    }

    public async Task<UpdateStoryInfoResult> UpdateStory(Story story)
    {
        await _storyRepository.UpdateStory(story);
        return new PublishStoryResult(ResultStatus.Success, string.Empty, story.Id);
    }

    public async Task<List<Story>> GetStoryBookMarksByUserId(int userId)
    {
        return await _storyRepository.GetStoryBookMarks(userId);
    }

    public async Task<UpdateStoryInfoResult> BookmarkStory(StoryBookMark bookMark)
    {
        List<StoryBookMark> marks = await GetBookMarksAll();
        IEnumerable<StoryBookMark> marked = marks.Where(mark => mark.User.Id == bookMark.User.Id
                                                                && mark.Story.Id == bookMark.Story.Id);
        if (marked.Any())
            await _storyRepository.UnBookMarkStory(bookMark);
        else
            await _storyRepository.BookMarkStory(bookMark);
        return new PublishStoryResult(ResultStatus.Success, string.Empty, bookMark.Story.Id);
    }

    public async Task<int> AddComment(Comment comment)
    {
        return await _storyRepository.AddStoryComment(comment);
    }

    public async Task<int> UpdateComment(Comment comment)
    {
        return await _storyRepository.UpdateStoryComment(comment);
    }

    public async Task<int> DeleteComment(Comment comment)
    {
        return await _storyRepository.DeleteStoryComment(comment);
    }

    public async Task<List<Comment>> GetAllComments()
    {
        return await _storyRepository.GetStoryComments();
    }

    public async Task<List<StoryRating>> GetStoryScores()
    {
        return await _storyRepository.GetStoryScores();
    }

    //[HttpPost("audio")]
    //[AllowAnonymous]
    //public async Task<IActionResult> AddAudioForStory([FromBody] CreateAudioModelRequest audio)
    //{
    //    Guid newAudioId = Guid.NewGuid();
    //    DateTimeOffset currentDate = DateTimeOffset.Now;
    //    StoryAudio storyAudio = new(newAudioId, currentDate.Date, audio.Name);

    //    SaveByteArrayToFileWithBinaryWriter(audio.Audio, "audios/" + newAudioId + ".mp3");

    //    int result = await _storyRepository.CreateAudio(storyAudio, user);
    //    return Ok(result);
    //}

    //public static void SaveByteArrayToFileWithBinaryWriter(byte[] data, string filePath)
    //{
    //    using BinaryWriter writer = new(System.IO.File.OpenWrite(filePath));
    //    writer.Write(data);
    //}

    //[HttpPut("audio")]
    //public async Task<IActionResult> ChangeAudioForStory([FromBody] UpdateAudioRequest audio)
    //{
    //    StoryAudio existingAudio = await _storyRepository.GetAudioById(audio.AudioId);

    //    if (existingAudio is null)
    //        return NotFound("Audio not found");

    //    SaveByteArrayToFileWithBinaryWriter(audio.Audio, "audios/" + existingAudio.FileId + ".mp3");

    //    return Ok();
    //}

    //[HttpDelete("audio")]
    //public async Task<IActionResult> DeleteAudioForStory([FromBody] int[] audioIds)
    //{
    //    StoryAudio existingAudio = await _storyRepository.GetAudioById(audioIds[0]);

    //    if (existingAudio is null)
    //        return NotFound("Audio not found");

    //    bool result = await _storyRepository.DeleteAudio(audioIds, user);

    //    string path = "audios/" + existingAudio.FileId + ".mp3";

    //    if (!result || !System.IO.File.Exists(path)) return NoContent();

    //    System.IO.File.Delete(path);
    //    return Ok();
    //}

    public async Task<List<StoryReadDto>> SetStoryInfo(List<StoryReadDto> storyReadDtos)
    {
        List<StoryRating> scores = await _storyRepository.GetStoryScores();
        List<Comment> comments = await _storyRepository.GetStoryComments();
        List<StoryReadHistory> reads = await _storyRepository.GetStoryReadHistory();

        foreach (StoryReadDto storyDto in storyReadDtos)
        {
            int pagesCount = await _storyRepository.GetStoryPagesCount(storyDto.Id);
            IEnumerable<StoryRating> scoresByStoryId = scores.Where(s => s.Story.Id == storyDto.Id);
            storyDto.AverageScore = scoresByStoryId.Any() ? scoresByStoryId.Average(s => s.Score) : -1;
            storyDto.CommentCount = comments.Count(c => c.Story.Id == storyDto.Id);
            storyDto.ReadCount = reads.Count(c => c.Story.Id == storyDto.Id);
            storyDto.PageCount = pagesCount;
            storyDto.Publisher.TotalStories = storyReadDtos.Count(c => c.Publisher.Id == storyDto.Publisher.Id);
            var userreads = from read in reads
                join story in storyReadDtos on read.Story.Id equals story.Id
                select new { read.User.Id, PublisherId = story.Publisher.Id };
            storyDto.Publisher.TotalReads = userreads.Count(ur => ur.PublisherId == storyDto.Publisher.Id);
            var userscores = from score in scores
                join story in storyReadDtos on score.Story.Id equals story.Id
                select new { score.User.Id, PublisherId = story.Publisher.Id, score.Score };
            var userscoresByPubId = userscores.Where(us => us.PublisherId == storyDto.Publisher.Id);
            storyDto.Publisher.AverageScore = userscoresByPubId.Any() ? userscoresByPubId.Average(us => us.Score) : -1;
        }

        return storyReadDtos;
    }

    public async Task<List<StoryBookMark>> GetBookMarksAll()
    {
        return await _storyRepository.GetBookMarks();
    }
}