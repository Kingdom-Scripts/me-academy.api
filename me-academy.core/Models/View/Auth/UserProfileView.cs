﻿namespace me_academy.core.Models.View.Auth;

public class UserProfileView
{
    public Guid Uid { get; set; } = new Guid();
    public string Email { get; internal set; }
    public string Type { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public DateTime LastLoginDate { get; set; }
}