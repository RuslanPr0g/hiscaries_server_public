namespace HC.Application.Common.Constants;

// TODO: create meaningfull messages
public static class UserFriendlyMessages
{
    public static string UsernameEmpty = "Username is empty.";

    public static string ReviewMessageCannotBeEmpty { get; internal set; }
    public static string UserIsNotFound { get; internal set; }
    public static string PasswordMismatch { get; internal set; }
    public static string UserIsBanned { get; internal set; }
    public static string TryAgainLater { get; internal set; }
    public static string UserWithUsernameExists { get; internal set; }
    public static string UserWithEmailExists { get; internal set; }
    public static string PleaseRelogin { get; internal set; }
    public static string RefreshTokenIsNotExpired { get; internal set; }
    public static string RefreshTokenIsExpired { get; internal set; }
    public static string StoryWasNotFound { get; internal set; }
}
