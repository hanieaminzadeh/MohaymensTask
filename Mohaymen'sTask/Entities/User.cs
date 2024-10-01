﻿using System.ComponentModel.DataAnnotations;

namespace Mohaymen_sTask.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool IsAvailable { get; set; } = true;
}
