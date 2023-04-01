using System.ComponentModel.DataAnnotations;

namespace AspNetAngular.Models
{
    public class RegisterRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        [MaxLength(10, ErrorMessage = "Слишком длинный у тебя город, брат")]
        public string City { get; set; }
    }
}
