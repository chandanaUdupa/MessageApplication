using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PublicMessageStorytel.Models
{
    public class PublicMessageDTO
    {
        public long MessageId { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string MessageContent { get; set; }

        public string AddressedTo { get; set; }

        [Required]
        public string ClientName { get; set; }
        
        [Required]
        public string ClientEmailId { get; set; }

        public string ValidUntil { get; set; }
    }
}
