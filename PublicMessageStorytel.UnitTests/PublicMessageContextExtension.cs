using Microsoft.EntityFrameworkCore;
using PublicMessageStorytel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicMessageStorytel.UnitTests
{
    public static class PublicMessageContextExtension
    {
        public static void Seed(this PublicMessageContext dbContext)
        {
            // Add entities for DbContext instance
            dbContext.PublicMessages.Add(new PublicMessage
            {
                MessageId = 1,
                Title = "Book Launch Event",
                AddressedTo = "All book lovers (you will receive one sample copy of the book for free)",
                ValidUntil = "Event will be held on 26th of Feb, 2021",
                MessageContent = "A bit about me:I am an author,blogger,speaker and lover of conscious creation.I spent decades becoming masterful at consciously creating a life I love,and now my mission is to teach others.My first book, The Map – To Our Responsive UniverseYou will be able to gather the contact information from all those who download your gift.",
                Client = new Client { EmailId = "test@gmail.com", FullName = "Renie Sen" },
                PostedOn = DateTime.Now.ToLongDateString()
            });

            dbContext.PublicMessages.Add(new PublicMessage
            {
                MessageId = 2,
                Title = "Sample message title 1",
                AddressedTo = "ABC",
                ValidUntil = "21th of March, 2021",
                MessageContent = "Some message content",
                Client = new Client { EmailId = "test5@gmail.com", FullName = "Random test" },
                PostedOn = DateTime.Now.AddDays(5).ToLongDateString()
            });

            dbContext.PublicMessages.Add(new PublicMessage
            {
                MessageId = 3,
                Title = "Sample message title 2",
                AddressedTo = "DEF",
                ValidUntil = "17th of July, 2021",
                MessageContent = "Some message content",
                Client = new Client { EmailId = "test2@gmail.com", FullName = "Fun test" },
                PostedOn = DateTime.Now.ToLongDateString()
            });

            dbContext.PublicMessages.Add(new PublicMessage
            {
                MessageId = 4,
                Title = "Sample message title 3",
                AddressedTo = "GHI",
                ValidUntil = "19th of Nov, 2021",
                MessageContent = "Some message content",
                Client = new Client { EmailId = "test3@gmail.com", FullName = "Cool test" },
                PostedOn = DateTime.Now.ToLongDateString()
            });

            dbContext.PublicMessages.Add(new PublicMessage
            {
                MessageId = 5,
                Title = "Sample message title 5",
                AddressedTo = "JKL",
                ValidUntil = "2nd of March, 2021",
                MessageContent = "Some message content",
                Client = new Client { EmailId = "test5@gmail.com", FullName = "Demo test" },
                PostedOn = DateTime.Now.ToLongDateString()
            });


            dbContext.PublicMessages.Add(new PublicMessage
            {
                MessageId = 6,
                Title = "Sample message title 5",
                AddressedTo = "XYZ",
                ValidUntil = "12th of March, 2021",
                MessageContent = "Some message content",
                Client = new Client { EmailId = "test1@gmail.com", FullName = "Some test" },
                PostedOn = DateTime.Now.ToLongDateString()
            });

            foreach (PublicMessage msg in dbContext.PublicMessages)
            {
                dbContext.Entry(msg).State = EntityState.Detached;
            }
            dbContext.SaveChanges();
        }
    }
}
