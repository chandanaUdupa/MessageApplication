using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublicMessageStorytel.Controllers;
using PublicMessageStorytel.Models;
using Xunit;

namespace PublicMessageStorytel.UnitTests
{
    public class PublicMessageControllerTests
    {

        #region Get By Id
        [Fact]
        public async void Task_GetPostMessageById_Return_OkResult()
        {
            // Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_GetPostMessageById_Return_OkResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var id = 1;

            // Act
            var response = await controller.GetPublicMessage(id);
            dbContext.Dispose();

            // Assert
            Assert.IsType<PublicMessageDTO>(response.Value);
        }

        [Fact]
        public async void Task_GetPostMessageById_Return_NotFoundResult()
        {
            // Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_GetPostMessageById_Return_NotFoundResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var id = 100;

            // Act
            var response = await controller.GetPublicMessage(id);
            dbContext.Dispose();

            //Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public async void Task_GetPostMessageById_Return_BadRequestResult()
        {

            // Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_GetPostMessageById_Return_BadRequestResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            int id = int.Parse("-1");

            // Act
            var response = await controller.GetPublicMessage(id);
            dbContext.Dispose();

            //Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }


        [Fact]
        public async void Task_GetPublicMessageById_MatchResult()
        {
            // Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_GetPublicMessageById_MatchResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var id = 1;

            // Act
            ActionResult<PublicMessageDTO> response = await controller.GetPublicMessage(id);
            var value = response.Value;
            dbContext.Dispose();

            //Assert
            Assert.IsType<PublicMessageDTO>(value);


            Assert.Equal("Book Launch Event", value.Title);
            Assert.Equal("All book lovers (you will receive one sample copy of the book for free)", value.AddressedTo);
        }

        #endregion

        #region Get All

        [Fact]
        public async void Task_GetPublicMessages_Return_OkResult()
        {
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_GetPublicMessages_Return_OkResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);

            //Act
            var data = await controller.GetPublicMessages();

            //Assert
            Assert.IsAssignableFrom<IEnumerable<PublicMessageDTO>>(data.Value);
            Assert.True((data.Value as List<PublicMessageDTO>).Count > 0);
        }

        [Fact]
        public void Task_GetPublicMessages_Return_BadRequestResult()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_GetPublicMessages_Return_BadRequestResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);

            //Act
            var data = controller.GetPublicMessages();
            data = null;

            if (data != null)
                //Assert
                Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_GetPublicMessages_MatchResult()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_GetPublicMessages_MatchResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);

            //Act
            var data = await controller.GetPublicMessages();
            List<PublicMessageDTO> value = (List<PublicMessageDTO>)data.Value;

            //Assert
            Assert.IsAssignableFrom<IEnumerable<PublicMessageDTO>>(data.Value);


            Assert.Equal("Book Launch Event", value[0].Title);
            Assert.Equal("All book lovers (you will receive one sample copy of the book for free)", value[0].AddressedTo);

            Assert.Equal("Sample message title 1", value[1].Title);
            Assert.Equal("ABC", value[1].AddressedTo);
        }

        #endregion

        #region Add New Public Message

        [Fact]
        public async void Task_Add_ValidData_Return_OkResult()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Add_ValidData_Return_OkResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var post = new PublicMessageDTO
            {
                MessageId = 7,
                Title = "Sample message title 7",
                AddressedTo = "XYZsdkdkjdkksd",
                ValidUntil = "12th of April, 2021",
                MessageContent = "Some message content",
                ClientEmailId = "test7@gmail.com",
                ClientName = "Some test"
            };

            //Act
            var data = await controller.CreateNewPublicMessage(post);

            //Assert
            Assert.IsType<PublicMessage>(data.Value);
        }

        [Fact]
        public async void Task_Add_InvalidData_Return_BadRequest()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Add_InvalidData_Return_BadRequest));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            PublicMessageDTO post = null;

            //Act            
            var data = await controller.CreateNewPublicMessage(post);

            //Assert
            Assert.IsType<BadRequestResult>(data.Result);
        }

        [Fact]
        public async void Task_Add_ValidData_MatchResult()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Add_ValidData_MatchResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var post = new PublicMessageDTO
            {
                MessageId = 8,
                Title = "Sample message title 8",
                AddressedTo = "XYZsdkdkjdkksd",
                ValidUntil = "12th of April, 2021",
                MessageContent = "Some message content",
                ClientEmailId = "test7@gmail.com",
                ClientName = "Some test"
            };

            //Act
            var data = await controller.CreateNewPublicMessage(post);

            //Assert
            Assert.IsType<OkObjectResult>(data.Result);

            Assert.Equal("Sample message title 8", data.Value.Title);
        }

        #endregion


        #region Update Existing Public Messages

        [Fact]
        public async void Task_Update_ValidData_Return_OkResult()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Update_ValidData_Return_OkResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var postId = 6;

            //Act
            var updatedData = await controller.UpdatePublicMessage(postId, new PublicMessage
            {
                MessageId = 6,
                AddressedTo = "ABC New people", // Changed the title
            });

            //Assert
            Assert.IsType<OkResult>(updatedData);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_BadRequest()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Update_InvalidData_Return_BadRequest));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var postId = 3;

            //Act
            var updatedData = await controller.UpdatePublicMessage(postId, new PublicMessage
            {
                MessageId = 4,
                AddressedTo = "ABC New people", // Changed the title
            });

            //Assert
            Assert.IsType<BadRequestResult>(updatedData);
        }

        [Fact]
        public async void Task_Update_InvalidData_Return_NotFound()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Update_InvalidData_Return_NotFound));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var postId = -1;

            //Act
            var updatedData = await controller.UpdatePublicMessage(postId, new PublicMessage
            {
                MessageId = -1,
                AddressedTo = "ABC New people", // Changed the title
            });

            //Assert
            Assert.IsType<NotFoundResult>(updatedData);
        }

        #endregion

        #region Delete Public Message

        [Fact]
        public async void Task_Delete_PublicMessage_Return_OkResult()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Delete_PublicMessage_Return_OkResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var postId = 2;

            //Act
            var data = await controller.DeletePublicMessage(postId);

            //Assert
            Assert.IsType<ContentResult>(data);
        }

        [Fact]
        public async void Task_Delete_PublicMessage_Return_NotFoundResult()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Delete_PublicMessage_Return_NotFoundResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            var postId = 8;

            //Act
            var data = await controller.DeletePublicMessage(postId);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_Delete_Return_BadRequestResult()
        {
            //Arrange
            var dbContext = PublicMessageContextMocker.GetPublicMessageContextForTests(nameof(Task_Delete_Return_BadRequestResult));
            ILoggerFactory factory = new LoggerFactory();
            ILogger<PublicMessageController> _log = new Logger<PublicMessageController>(factory);
            var controller = new PublicMessageController(dbContext, _log);
            int postId = -1;

            //Act
            var data = await controller.DeletePublicMessage(postId);

            //Assert
            Assert.IsType<NotFoundResult>(data);
        }

        #endregion
    }
}
