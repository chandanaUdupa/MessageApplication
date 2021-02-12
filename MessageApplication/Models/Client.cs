using System.ComponentModel.DataAnnotations;

namespace PublicMessageStorytel.Models
{
    public class Client
    {
        [Key]
        public long ClientId { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
    }
}
