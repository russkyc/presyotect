﻿using Presyotect.Core.Contracts;

namespace Presyotect.Features.Users.Models;

public class User : DbEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }
}