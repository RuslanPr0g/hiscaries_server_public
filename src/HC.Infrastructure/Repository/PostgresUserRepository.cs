//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Threading.Tasks;
//using Dapper;
//using HC.Application.Interface;
//using HC.Application.Models.Connection;
//using HC.Application.Options;
//using HC.Domain.User;
//using Microsoft.Extensions.Options;
//using Npgsql;

//namespace HC.Infrastructure.Repository;

//public class PostgresUserRepository : IUserRepository
//{
//    private readonly DbConnectionStrings _connection;

//    public PostgresUserRepository(IOptionsSnapshot<DbConnectionStrings> connection)
//    {
//        _connection = connection.Value;
//    }

//    public async Task<RefreshToken> GetRefreshToken(string refreshToken, UserConnection connection)
//    {
//        const string sqlExpressionToGetRefreshToken = @"select * from getrefreshtoken(@token);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<RefreshToken> tokens = await dbconnection.QueryAsync<RefreshToken>(sqlExpressionToGetRefreshToken,
//            new
//            {
//                token = refreshToken
//            });

//        return tokens.ToList().FirstOrDefault();
//    }

//    public async Task<List<User>> GetUsers(UserConnection connection)
//    {
//        const string sqlExpressionToGetAllUsers = @"select * from getallusers();";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<User> users = await dbconnection.QueryAsync<User>(sqlExpressionToGetAllUsers);

//        return users.ToList();
//    }

//    public async Task<List<Review>> GetReviews(UserConnection connection)
//    {
//        const string sql = @"select * from reviews;";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<Review> reviews = await dbconnection.QueryAsync<Review>(sql);

//        return reviews.ToList();
//    }

//    public async Task InsertReview(Review review, UserConnection connection)
//    {
//        const string sql = @"select * from addreview(@publisherId, @reviewerId, @message);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sql, new
//        {
//            publisherId = review.PublisherId,
//            reviewerId = review.ReviewerId,
//            message = review.Message
//        });
//    }

//    public async Task DeleteReview(Review review, UserConnection connection)
//    {
//        const string sql = @"select * from deletereview(@id);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sql, new
//        {
//            id = review.Id
//        });
//    }

//    public async Task<User> GetUserById(int userId, UserConnection connection)
//    {
//        const string sqlExpressionToGetSpecificUser = @"select * from getuserbyid(@id);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<User> users = await dbconnection.QueryAsync<User>(sqlExpressionToGetSpecificUser, new
//        {
//            id = userId
//        });

//        return users.ToList().FirstOrDefault();
//    }

//    public async Task<User> GetUserByUsername(string username, UserConnection connection)
//    {
//        const string sqlExpressionToGetSpecificUser = @"select * from getuserbyusername(@username);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<User> users = await dbconnection.QueryAsync<User>(sqlExpressionToGetSpecificUser, new
//        {
//            username
//        });

//        return users.FirstOrDefault();
//    }

//    public async Task<string> GetUserRoleByUsername(string username, UserConnection connection)
//    {
//        const string sqlExpressionToGetSpecificUser = @"select * from getuserrolebyusername(@username);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<UserRole> users = await dbconnection.QueryAsync<UserRole>(sqlExpressionToGetSpecificUser, new
//        {
//            username
//        });

//        UserRole isAdmin = users.Where(x => x.Role == "admin").FirstOrDefault();
//        UserRole isPublisher = users.Where(x => x.Role == "publisher").FirstOrDefault();

//        if (isAdmin is not null) return "admin";

//        if (isPublisher is not null) return "publisher";

//        return "reader";
//    }

//    public async Task BecomePublisher(string username, UserConnection connection)
//    {
//        const string sqlExpressionToBecomePublisher = @"select * from grantrole(@username, @role);
//                    select * from adduserrole(@userId, @role);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        User user = await GetUserByUsername(username, connection);

//        IEnumerable<dynamic> users = await dbconnection.QueryAsync(sqlExpressionToBecomePublisher, new
//        {
//            username,
//            userId = user.Id,
//            role = "publisher"
//        });
//    }

//    public async Task<int> AddUser(User user, UserConnection connection)
//    {
//        string sqlExpressionToInsert = @"select * from adduser(@username, @email, @created, @birthdate, @banned);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        IEnumerable<int> ids = await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            username = user.Username,
//            email = user.Email,
//            created = user.AccountCreated.ToShortDateString(),
//            birthdate = user.BirthDate.ToShortDateString(),
//            banned = user.Banned
//        });

//        return ids.Single();
//    }

//    public async Task UpdateUserData(int userId, User user, UserConnection connection)
//    {
//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        if (user is not null)
//        {
//            string sqlUserDataUpdate =
//                @"select * from updateuserdata(@username, @email, @created, @birthdate, @banned, @userId);";

//            await dbconnection.QueryAsync<int>(sqlUserDataUpdate, new
//            {
//                userId,
//                username = user.Username,
//                email = user.Email,
//                created = user.AccountCreated.ToShortDateString(),
//                birthdate = user.BirthDate.ToShortDateString(),
//                banned = user.Banned
//            });
//        }
//    }

//    public async Task InsertRefreshToken(RefreshToken refreshToken, UserConnection connection)
//    {
//        const string sqlExpressionToInsert =
//            @"select * from addrefreshtoken(@token, @creationDate, @expiryDate, @jwtId, @used, @invalidated, @userId);";

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sqlExpressionToInsert, new
//        {
//            token = refreshToken.Token,
//            creationDate = refreshToken.CreationDate,
//            expiryDate = refreshToken.ExpiryDate,
//            jwtId = refreshToken.JwtId,
//            used = refreshToken.Used,
//            invalidated = refreshToken.Invalidated,
//            userId = refreshToken.UserId
//        });
//    }

//    public async Task UpdateRefreshToken(RefreshToken refreshToken, UserConnection connection)
//    {
//        const string sqlExpressionToUpdate =
//            @"select * from updaterefreshtoken(@token, @creationDate, @expiryDate, @jwtId, @used, @invalidated, @userId, @tokenId);";

//        RefreshToken token = await GetRefreshToken(refreshToken.Token, connection);

//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        await dbconnection.QueryAsync<int>(sqlExpressionToUpdate, new
//        {
//            tokenId = token.Id,
//            token = refreshToken.Token,
//            creationDate = refreshToken.CreationDate,
//            expiryDate = refreshToken.ExpiryDate,
//            jwtId = refreshToken.JwtId,
//            used = refreshToken.Used,
//            invalidated = refreshToken.Invalidated,
//            userId = refreshToken.UserId
//        });
//    }

//    public async Task AddUserRoleLogin(User user, UserConnection connection)
//    {
//        string connectionString = GetConnectionString(_connection.Postgres, connection);

//        using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//        if (user is not null)
//        {
//            string sqlUserDataUpdate = @"select * from adduserrolelogin(@username, @password);
//                select * from adduserrole(@userId, @role);";

//            await dbconnection.QueryAsync<int>(sqlUserDataUpdate, new
//            {
//                userId = user.Id,
//                username = user.Username,
//                password = user.Password,
//                role = "reader"
//            });
//        }
//    }

//    public async Task<bool> VerifyConnection(UserConnection connection)
//    {
//        try
//        {
//            string connectionString = GetConnectionString(_connection.Postgres, connection);

//            using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//            const string sqlToVerifyConnection = @"select 1";

//            await dbconnection.QueryAsync<int>(sqlToVerifyConnection);

//            return true;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    public async Task Test(UserConnection connection)
//    {
//        try
//        {
//            string connectionString = GetConnectionString(_connection.Postgres, connection);

//            using IDbConnection dbconnection = new NpgsqlConnection(connectionString);

//            const string sqlToVerifyConnection = @"select count(*) from storydomain.story;";

//            IEnumerable<int> a = await dbconnection.QueryAsync<int>(sqlToVerifyConnection);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//        }
//    }

//    private static string GetConnectionString(string connectionstringtemp, UserConnection connection)
//    {
//        return string.Format(connectionstringtemp, connection.Username, connection.Password);
//    }
//}