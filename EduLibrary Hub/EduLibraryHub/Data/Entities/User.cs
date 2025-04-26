using Microsoft.AspNetCore.Identity;

namespace EduLibraryHub.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual List<Review>? Reviews { get; set; }

    }
}
