using System.Reflection.Metadata;

namespace todolistv1.Models
{
    public class Todo
    {
        public Todo()
        {
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
        public int Id { get; set; } 
        public string Name { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public enum status
        {
            NotStarted = 0 , Ongoing =  1, Completed = 2
        }
        public status Status { get; set; }

    }
}
