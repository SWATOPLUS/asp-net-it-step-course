using Microsoft.EntityFrameworkCore;

namespace AspNetAngular.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    public class ApplicationUser
    {
        public Guid ApplicationUserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string City { get; set; }
    }
}
