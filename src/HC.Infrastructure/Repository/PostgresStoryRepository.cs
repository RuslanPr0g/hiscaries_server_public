//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using Dapper;
//using HC.Application.Interface;
//using HC.Application.Models.Connection;
//using HC.Application.Options;

//using HC.Domain.Story;
//using HC.Domain.Story.Comment;
//using HC.Domain.User;
//using Microsoft.Extensions.Options;
//using Npgsql;

//namespace HC.Infrastructure.Repository;

//public class PostgresStoryRepository : IStoryWriteRepository
//{
//    private readonly DbConnectionStrings _connection;

//    public PostgresStoryRepository(IOptionsSnapshot<DbConnectionStrings> connection)
//    {
//        _connection = connection.Value;
//    }

//    public async Task<int> GetNumberOfPages(int storyId, UserConnection connection)
//    {
//        const string sqlExpressionToGetAllPages = @"SELECT COUNT(*) FROM storydomain.storypagecontent;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> pages = await dbconnection.QueryAsync<int>(sqlExpressionToGetAllPages, new { });

//        return pages.First();
//    }

//    public async Task<List<Story>> GetStories(UserConnection connection)
//    {
//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        const string sqlExpressionToGetAllStories = @"SELECT s.id as Id,
//s.title as Title, 
//s.description as Description, 
//s.author_name as AuthorName, 
//s.age_limit as AgeLimit, 
//s.image_preview as ImagePreview, 
//s.published as DatePublished, 
//s.written as DateWritten,

//u.id as Id, 
//u.username as Username, 
//u.email as Email, 
//u.created as Created, 
//u.birthdate as BirthDate,
//u.banned as Banned

//FROM storydomain.story as s
//LEFT JOIN userdomain.user u on u.id = s.publisher_id;";

//        IEnumerable<Story> rows = await dbconnection.QueryAsync<Story, User, Story>(sqlExpressionToGetAllStories,
//            (story, user) =>
//            {
//                story.Publisher = user;
//                return story;
//            }, splitOn: "Id");

//        List<Story> stories = rows.ToList();

//        foreach (Story story in stories) story.Genres = await GetStoryGenres(story.Id, connection);

//        return stories;
//    }

//    public async Task<Story> GetStory(int storyId, UserConnection connection)
//    {
//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        const string sqlExpressionToGetAllStories = @"SELECT s.id as Id,
//s.title as Title, 
//s.description as Description, 
//s.author_name as AuthorName, 
//s.age_limit as AgeLimit, 
//s.image_preview as ImagePreview, 
//s.published as DatePublished, 
//s.written as DateWritten,

//u.id as Id, 
//u.username as Username, 
//u.email as Email, 
//u.created as Created, 
//u.birthdate as BirthDate,
//u.banned as Banned

//FROM storydomain.story as s
//LEFT JOIN userdomain.user u on u.id = s.publisher_id
//WHERE s.id = @storyId;";

//        IEnumerable<Story> rows = await dbconnection.QueryAsync<Story, User, Story>(sqlExpressionToGetAllStories,
//            (story, user) =>
//            {
//                story.Publisher = user;
//                return story;
//            }, splitOn: "Id", param: new { storyId });

//        Story story = rows.ToList().FirstOrDefault();
//        story.Genres = await GetStoryGenres(story.Id, connection);
//        return story;
//    }

//    public async Task<List<Story>> GetStoryBookMarks(int userId, UserConnection connection)
//    {
//        const string sql = @"select 
//	story_id as Id
//	, user_id as UserId
//	, date_added as DateAdded,

//	s.title as Title, 
//	s.description as Description, 
//	s.author_name as AuthorName, 
//	s.age_limit as AgeLimit, 
//	s.image_preview as ImagePreview, 
//	s.published as DatePublished, 
//	s.written as DateWritten
//from userstorydomain.storybookmark sr
//inner join storydomain.story s on sr.story_id = s.id
//where sr.user_id = @userId;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<Story> pages = await dbconnection.QueryAsync<Story>(sql, new
//        {
//            userId
//        });

//        return pages.ToList();
//    }

//    public async Task<int> AddStoryComment(Comment comment, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"INSERT INTO userstorydomain.comment(
//	story_id, user_id, content, commented)
//	VALUES (@storyId, @userId, @content, @commented::date) RETURNING id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> ids = await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            storyId = comment.StoryId,
//            userId = comment.UserId,
//            content = comment.Content,
//            commented = comment.CommentedAt.ToShortDateString()
//        });

//        return ids.Single();
//    }

//    public async Task<int> AddGenre(Genre genre, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"INSERT INTO storydomain.genre(
//	name, description)
//	VALUES (@name, @description) RETURNING id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> ids = await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            name = genre.Name,
//            description = genre.Description
//        });

//        return ids.Single();
//    }

//    public async Task<int> UpdateGenre(Genre genre, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"UPDATE storydomain.genre
//	SET name=@name, description=@description
//	WHERE id=@id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync(sqlExpressionToInsert, new
//        {
//            id = genre.Id,
//            name = genre.Name,
//            description = genre.Description
//        });

//        return genre.Id;
//    }

//    public async Task<int> DeleteGenre(Genre genre, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"DELETE FROM storydomain.genre
//	WHERE id=@id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync(sqlExpressionToInsert, new
//        {
//            id = genre.Id,
//            name = genre.Name,
//            description = genre.Description
//        });

//        return genre.Id;
//    }

//    public async Task<int> UpdateStoryComment(Comment comment, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"UPDATE userstorydomain.comment
//	SET story_id=@storyId, user_id=@userId, content=@content, commented=@commented
//	WHERE id = @id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            id = comment.Id,
//            storyId = comment.Story.Id,
//            user_id = comment.UserId,
//            content = comment.Content,
//            commented = comment.CommentedAt.ToShortDateString()
//        });

//        return comment.Id;
//    }

//    public async Task<int> DeleteStoryComment(Comment comment, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"DELETE FROM userstorydomain.comment
//	WHERE id = @id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            id = comment.Id
//        });

//        return comment.Id;
//    }

//    public async Task<int> DeleteStory(Story story, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"
//DELETE FROM storydomain.story
//	WHERE id = @id;
//        ";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            id = story.Id
//        });

//        return story.Id;
//    }

//    public async Task<List<StoryPage>> GetStoryPages(int storyId, UserConnection connection)
//    {
//        const string sqlExpressionToGetPagesOfStory =
//            @"SELECT id as Id, story_id as StoryId, page as Page, content as Content
//    FROM storydomain.storypagecontent WHERE story_id = @storyId;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<StoryPage> storyPages = await dbconnection.QueryAsync<StoryPage>(sqlExpressionToGetPagesOfStory, new
//        {
//            storyId
//        });

//        return storyPages.ToList();
//    }

//    public async Task<int> GetStoryPagesCount(int storyId, UserConnection connection)
//    {
//        const string sqlExpressionToGetPagesOfStory = @"SELECT COUNT(*) FROM storydomain.storypagecontent
//            WHERE story_id = @storyId;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> storyPages = await dbconnection.QueryAsync<int>(sqlExpressionToGetPagesOfStory, new
//        {
//            storyId
//        });

//        return storyPages.SingleOrDefault();
//    }

//    public async Task<int> TotalReadsForUser(int id, UserConnection connection)
//    {
//        const string sqlExpressionToTotalReadsForUser = @"select count(*) from userstorydomain.storyreadhistory
//where user_id = @id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        try
//        {
//            IEnumerable<int> storyPages = await dbconnection.QueryAsync<int>(sqlExpressionToTotalReadsForUser, new
//            {
//                id
//            });

//            return storyPages.SingleOrDefault();
//        }
//        catch
//        {
//            return -1;
//        }
//    }

//    public async Task<int> TotalStoriesForUser(int id, UserConnection connection)
//    {
//        const string sqlExpressionToTotalStoriesForUser = @"select count(*) from storydomain.story
//where publisher_id = @id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        try
//        {
//            IEnumerable<int> storyPages = await dbconnection.QueryAsync<int>(sqlExpressionToTotalStoriesForUser, new
//            {
//                id
//            });

//            return storyPages.SingleOrDefault();
//        }
//        catch
//        {
//            return -1;
//        }
//    }

//    public async Task<double> AverageScoreForUser(int id, UserConnection connection)
//    {
//        const string sqlExpressionToAverageScoreForUser =
//            @"select ROUND(avg(sc.score), 2) from userstorydomain.storyscore sc
//inner join storydomain.story s on s.id = sc.story_id
//where s.publisher_id = @id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        try
//        {
//            IEnumerable<double> storyPages = await dbconnection.QueryAsync<double>(sqlExpressionToAverageScoreForUser,
//                new
//                {
//                    id
//                });

//            return storyPages.SingleOrDefault();
//        }
//        catch
//        {
//            return -1;
//        }
//    }

//    public async Task<List<StoryBookMark>> GetBookMarks(UserConnection connection)
//    {
//        const string sql = @"SELECT id as Id, story_id as StoryId, user_id as UserId, date_added as DateAdded
//	FROM userstorydomain.storybookmark;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<StoryBookMark> pages = await dbconnection.QueryAsync<StoryBookMark>(sql, new { });

//        return pages.ToList();
//    }

//    public async Task UnBookMarkStory(StoryBookMark bookMark, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"DELETE FROM userstorydomain.storybookmark
//	WHERE user_id = @userId and story_id = @storyId;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            storyId = bookMark.Story.Id,
//            userId = bookMark.User.Id
//        });
//    }

//    public async Task BookMarkStory(StoryBookMark bookMark, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"INSERT INTO userstorydomain.storybookmark(
//	story_id, user_id, date_added)
//	VALUES (@storyId, @userId, @dateAdded::date);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            storyId = bookMark.Story.Id,
//            userId = bookMark.User.Id,
//            dateAdded = bookMark.DateAdded.ToShortDateString()
//        });
//    }

//    public async Task UpdateStoryScore(StoryRating storyScore, UserConnection connection)
//    {
//        string sql = @"UPDATE userstorydomain.storyscore
//	SET score=@score
//	WHERE id=@id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sql, new
//        {
//            id = storyScore.Id,
//            score = storyScore.Score
//        });
//    }

//    public async Task InsertStoryScore(StoryRating storyScore, UserConnection connection)
//    {
//        string sql = @"INSERT INTO userstorydomain.storyscore(
//	story_id, user_id, score)
//	VALUES (@storyId, @userId, @score);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sql, new
//        {
//            userId = storyScore.UserId,
//            storyId = storyScore.StoryId,
//            score = storyScore.Score
//        });
//    }

//    public Task<List<StoryAudio>> GetAudio(int storyId, UserConnection user)
//    {
//        throw new System.NotImplementedException();
//    }

//    public Task<bool> DeleteAudio(int[] audioIds, UserConnection user)
//    {
//        throw new System.NotImplementedException();
//    }

//    public async Task<int> CreateAudio(StoryAudio storyAudio, UserConnection connection)
//    {
//        string sqlExpressionToInsert =
//            @"INSERT INTO storydomain.audio(
//	                fileid, dateadded, name)
//                VALUES (@fileId, @dateAdded::date, @name);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> id = await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            fileId = storyAudio.FileId,
//            dateAdded = storyAudio.DateAdded.ToShortDateString(),
//            name = storyAudio.Name
//        });

//        return id.Single();
//    }

//    public Task<StoryAudio> GetAudioById(int audioId)
//    {
//        throw new System.NotImplementedException();
//    }

//    public async Task UpdateStory(Story story, UserConnection connection)
//    {
//        string sqlExpressionToInsert =
//            @"select * from updatestory2(@id, @publisherId, @title, @description, @authorName, @ageLimit, 
//                                        @imagePreview, @published, @written, @genres)";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            id = story.Id,
//            publisherId = story.Publisher.Id,
//            title = story.Title,
//            description = story.Description,
//            authorName = story.AuthorName,
//            ageLimit = story.AgeLimit,
//            imagePreview = story.ImagePreview,
//            published = story.DatePublished.ToShortDateString(),
//            written = story.DateWritten.ToShortDateString(),
//            genres = story.GenreIds
//        });
//    }

//    public async Task<int> AddPage(StoryPage page, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"INSERT INTO storydomain.storypagecontent
//                                            (story_id, content, page)
//                                            VALUES (@storyId, @content, @page) RETURNING id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> ids = await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            storyId = page.Story.Id,
//            content = page.Content,
//            page = page.Page
//        });

//        return ids.Single();
//    }

//    public async Task RemovePagesFromStory(int storyId, UserConnection connection)
//    {
//        string sqlExpressionToRemove = @"DELETE FROM storydomain.storypagecontent WHERE story_id = @storyId;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<dynamic> ids = await dbconnection.QueryAsync(sqlExpressionToRemove, new
//        {
//            storyId
//        });
//    }

//    public async Task<int> AddStory(Story story, UserConnection connection)
//    {
//        string sqlExpressionToInsert =
//            @"select * from addstory2(@publisherId, @title, @description, @authorName, @ageLimit, 
//                                        @imagePreview, @published, @written, @genres)";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> ids = await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            publisherId = story.Publisher.Id,
//            title = story.Title,
//            description = story.Description,
//            authorName = story.AuthorName,
//            ageLimit = story.AgeLimit,
//            imagePreview = story.ImagePreview,
//            published = story.DatePublished.ToShortDateString(),
//            written = story.DateWritten.ToShortDateString(),
//            genres = story.GenreIds
//        });

//        return ids.Single();
//    }

//    public async Task<int> ReadStoryHistory(StoryReadHistory story, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"INSERT INTO userstorydomain.storyreadhistory(
//	        story_id, user_id, date_read, page_read, soft_deleted)
//	        VALUES (@storyId, @userId, @dateRead::date, @pageRead, @softDeleted) RETURNING id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> ids = await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            userId = story.User.Id,
//            storyId = story.Story.Id,
//            dateRead = story.DateRead.ToShortDateString(),
//            pageRead = story.PageRead,
//            softDeleted = story.SoftDeleted
//        });

//        return ids.Single();
//    }

//    public async Task<List<Genre>> GetGenres(UserConnection connection)
//    {
//        const string sql = @"select id as Id, name as Name, description as Description, image_preview as ImagePreview 
//                        from storydomain.genre;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<Genre> genres = await dbconnection.QueryAsync<Genre>(sql, new { });

//        return genres.ToList();
//    }

//    public async Task<List<StoryRating>> GetStoryScores(UserConnection connection)
//    {
//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        const string sql =
//            @"select id as Id, story_id as StoryId, user_id as UserId, score as Score from userstorydomain.storyscore;";

//        IEnumerable<StoryRating> result = await dbconnection.QueryAsync<StoryRating>(sql);

//        return result.ToList();
//    }

//    public async Task<List<Comment>> GetStoryComments(UserConnection connection)
//    {
//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        const string sql = @"select c.id as Id, c.story_id as StoryId, c.user_id as UserId, 
//content as Content, commented as CommentedAt, u.username as Username,
//sc.score as Score
//from userstorydomain.comment c
//inner join userdomain.user u on c.user_id = u.id
//left join userstorydomain.storyscore sc on c.user_id = sc.user_id and c.story_id = sc.story_id;";

//        IEnumerable<Comment> result = await dbconnection.QueryAsync<Comment>(sql);

//        return result.ToList();
//    }

//    public async Task<List<StoryReadHistory>> GetStoryReadHistory(UserConnection connection)
//    {
//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        const string sql = @"select 
//	sr.id as Id
//	, story_id as StoryId
//	, user_id as UserId
//	, date_read as DateRead
//	, page_read as PageRead
//	, soft_deleted as SoftDeleted 
//	, title as Title
//	, description as Description
//from userstorydomain.storyreadhistory sr
//inner join storydomain.story ss on sr.story_id = ss.id;";

//        IEnumerable<StoryReadHistory> result = await dbconnection.QueryAsync<StoryReadHistory>(sql);

//        return result.ToList();
//    }

//    public async Task<List<StoryReadHistoryProgress>> GetStoryReadHistoryProgress(int userId, UserConnection connection)
//    {
//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        const string sql = @"select 
//s.id as StoryId,
//s.publisher_id as PublisherId,
//s.title as Title, 
//s.description as Description, 
//s.author_name as AuthorName, 
//s.age_limit as AgeLimit, 
//s.image_preview as ImagePreview, 
//s.published as DatePublished, 
//s.written as DateWritten,
//pr.LastRead as LastRead,
//pr.Total as Total,
//pr.Count as Count
//from storydomain.story s
//inner join (
//	select 
//	story_id as StoryId
//	, max(date_read) as LastRead
//	, (
//		select count(*) from storydomain.storypagecontent spc
//		where spc.story_id = sr.story_id
//	) as Total
//	, count(sr.id) as Count
//	from (
//		select * from userstorydomain.storyreadhistory srh
//		where user_id = @userId
//	) as sr
//	inner join storydomain.story ss on sr.story_id = ss.id
//	group by story_id
//) as pr
//on pr.storyId = s.id;
//";

//        IEnumerable<StoryReadHistoryProgress> result = await dbconnection.QueryAsync<StoryReadHistoryProgress>(sql, new
//        {
//            userId
//        });

//        return result.ToList();
//    }

//    public async Task<List<Story>> GetAllStoryBookMarks(UserConnection connection)
//    {
//        const string sql = @"select 
//      story_id as Id
//	, user_id as UserId
//	, date_added as DateAdded,

//	s.title as Title, 
//	s.description as Description, 
//	s.author_name as AuthorName, 
//	s.age_limit as AgeLimit, 
//	s.image_preview as ImagePreview, 
//	s.published as DatePublished, 
//	s.written as DateWritten
//from userstorydomain.storybookmark sr
//inner join storydomain.story s on sr.story_id = s.id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<Story> pages = await dbconnection.QueryAsync<Story>(sql, new
//            { });

//        return pages.ToList();
//    }

//    public async Task<List<Genre>> GetStoryGenres(int id, UserConnection connection)
//    {
//        const string sql =
//            @"select g.id as Id, g.name as Name, g.description as Description, g.image_preview as ImagePreview
//from storydomain.storygenre sg 
//inner join storydomain.genre g on g.id = sg.genre_id
//where story_id = @id;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<Genre> genres = await dbconnection.QueryAsync<Genre>(sql, new
//        {
//            id
//        });

//        return genres.ToList();
//    }

//    private static string GetConnectionString(string connectionstringtemp, UserConnection connection)
//    {
//        return string.Format(connectionstringtemp, connection.Username, connection.Password);
//    }
//}