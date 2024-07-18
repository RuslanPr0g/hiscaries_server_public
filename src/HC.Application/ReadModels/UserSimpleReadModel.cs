using System;

public sealed class UserSimpleReadModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string BirthDate { get; set; }
    public int Age { get; set; }
    public bool Banned { get; set; }
    public string Role { get; set; }
    public int TotalStories { get; set; }
    public int TotalReads { get; set; }
    public double AverageScore { get; set; }
}