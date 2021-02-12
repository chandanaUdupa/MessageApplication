using System;
using System.ComponentModel.DataAnnotations;

namespace PublicMessageStorytel.Models
{
    public class PublicMessage
    {
        [Key]
        public long MessageId { get; set; }
        
        [Required]
        public string Title { get; set; }

        [Required]
        public string MessageContent { get; set; }

        public string AddressedTo { get; set; }

        public Client Client { get; set; }
        public string PostedOn { get; set; }

        public string ValidUntil { get; set; }

    }

}
