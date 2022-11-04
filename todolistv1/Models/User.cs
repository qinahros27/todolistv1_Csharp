using Microsoft.Extensions.Hosting;

namespace todolistv1.Models
{
    public class User
    {
        public User()
        {
            CreatedDate = DateTime.Now;
            UpdateDate = DateTime.Now;
        }
        public int Id { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<Todo>? Todos { get; set; }

    }
}
