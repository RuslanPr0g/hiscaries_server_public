namespace HC.API;

public static class APIConstants
{
    public const string User = "api/v{version:apiVersion}/user";
    public const string UpdateProfile = "update-profile";
    public const string Register = "register";
    public const string Login = "login";
    public const string BecomePublisher = "becomepublisher";
    public const string Refresh = "refresh";
    public const string Shuffle = "shuffle";
    public const string ReadStory = "readstory";
    public const string Genres = "genres";
    public const string History = "history";
    public const string BookmarkStory = "bookmark";
    public const string Score = "score";
    public const string Review = "review";
    public const string DeleteReview = "review/force";


    public const string AddComment = "comment";
    public const string UpdateComment = "comment";
    public const string DeleteComment = "comment/force";
    public const string GetAllComments = "comment";


    public const string Story = "api/story";
    public const string GetPage = "page/{storyId}";
    public const string AddPage = "page";
    public const string Pages = "pages";
    public const string DeleteStory = "delete";
}