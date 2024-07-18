using System;

public class UpdateUserDataRequest
{
    public string UpdatedUsername { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public bool Banned { get; set; } = false;

    // TODO: separate password update process
    public string PreviousPassword { get; set; }
    public string NewPassword { get; set; }
}