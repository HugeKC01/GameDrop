using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GameDrop.Areas.Identity.Data;

// Add profile data for application users by adding properties to the GameDropUser class
public class GameDropUser : IdentityUser
{
    [PersonalData]
    public string? FirstName { get; set; }
    [PersonalData]
    public string? LastName { get; set; }

    [PersonalData]
    [NotMapped]
    [Display(Name = "Profile Image")]
    public IFormFile? ProfileImage { get; set; }
    public byte[]? ProfileImageData { get; set; }
    public string? ProfileImageType { get; set; }
}

