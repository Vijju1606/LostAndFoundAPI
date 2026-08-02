using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.MicrosoftExtensions;

namespace LostAndFoundAPI.DTOs
{
    public class RegisterDto
    {
        [Required (ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required (ErrorMessage ="Email is required")]
        [EmailAddress (ErrorMessage ="Invalid email adress.")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Passwprd is required.")]
        [MinLength(6,ErrorMessage ="Password mustbe atleast 6 characters.")]
        public string Password { get; set; }
    }
}