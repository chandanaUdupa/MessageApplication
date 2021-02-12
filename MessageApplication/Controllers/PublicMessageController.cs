using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Server.Diagnostic;
using StandardModel.Diagnostic;
using PublicMessageStorytel.Models;
using Microsoft.AspNetCore.Authorization;

namespace PublicMessageStorytel.Controllers
{
    [Authorize]
    [Produces("application/json")]
    // In case, a newer version of our API is released in the future
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PublicMessageController : ControllerBase
    {
        private readonly PublicMessageContext _context;
        private readonly ILogger _log;

        public PublicMessageController(PublicMessageContext context, ILogger<PublicMessageController> logger)
        {
            _context = context;
            _log = logger;
        }

        /// <summary>
        /// Retrieves all messages
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// GET /api/v1/PublicMessage
        ///
        /// </remarks>
        /// <returns>Returns all the messages created until now</returns>
        /// <response code="201">If the messages are found and returned succesfully</response>
        /// <response code="500">If there is any error</response>   
        // GET: api/v1/PublicMessage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublicMessageDTO>>> GetPublicMessages()
        {
            var result = await _context.PublicMessages.AsNoTracking()
                .Select(x => PublicMessageToDTOForGetAll(x))
                .ToListAsync();
            return result;
        }

        /// <summary>
        /// Returns just a single message matching the id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// GET /api/v1/PublicMessage/{id}
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A newly created Message</returns>
        /// <response code="201">If the message is found and returned succesfully</response>
        /// <response code="500">If there is any error</response>   
        // GET: api/v1/PublicMessage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicMessageDTO>> GetPublicMessage(long id)
        {
            Stopwatch sw = new Stopwatch();
            //string error = null;
            sw.Start();

            try
            {
                var publicMessage = await _context.PublicMessages.FindAsync(id);

                if (publicMessage == null)
                {
                    return NotFound();
                   // throw new ArgumentNullException("Public Message", String.Format("Public Message with id {0} does not exist.", id));
                }
                publicMessage.Client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync();


                return PublicMessageToDTO(publicMessage);
            }
            catch (Exception ex)
            {
                _log.PublicMessageSingleLogError(ex.Message + " Stack trace: " + ex.StackTrace);
                return StatusCode(500, new MessageError(ex.Message, ex.StackTrace));
            }
            finally
            {
                sw.Stop();
                _log.PublicMessageSingleLogError($"Public Message Controller. Elapsed : { sw.Elapsed.TotalMilliseconds}");
            }
        }

        /// <summary>
        /// Updates a message
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// PUT /api/v1/PublicMessage/1
        ///{
        ///"MessageId": 1
        ///"Title": "English speaking housekeeper needed for central milan",
        ///"MessageContent": "Available Feb 2021 - Feb 2021 Seeking Part-time, Live Out $501-$1000/wk Must be available on short notice Lasted logged in 04 Feb 2021 Member since 04 Feb 2021",
        ///"ClientName": "Chandana",
        ///"ClientEmailId": "Chandana.Stylish@gmail.com",
        ///"AddressedTo": "All housekeeping job seekers",
        ///"ValidUntil": "20th of March"
        ///}
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="publicMessage"></param>
        /// <returns>A newly created Message</returns>
        /// <response code="200">Updates the message with new content</response>
        /// <response code="400">If the message is null</response>   
        // PUT: api/v1/PublicMessage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublicMessage(long id, PublicMessage publicMessage)
        {
            if (publicMessage == null)
            {
                throw new ArgumentNullException("Public Message", "Public Message cannot be null");
            }

            if (id != publicMessage.MessageId)
            {
                return BadRequest();
            }

            _context.Entry(publicMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicMessageExists(id))
                {
                    //throw new ArgumentNullException("Public Message", String.Format("Public Message with id {0} does not exist.", id));
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a message
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/v1/PublicMessage
        ///{
        ///"Title": "English speaking housekeeper needed for central milan",
        ///"MessageContent": "Available Feb 2021 - Feb 2021 Seeking Part-time, Live Out $501-$1000/wk Must be available on short notice Lasted logged in 04 Feb 2021 Member since 04 Feb 2021",
        ///"ClientName": "Chandana",
        ///"ClientEmailId": "Chandana.Stylish@gmail.com",
        ///"AddressedTo": "All housekeeping job seekers",
        ///"ValidUntil": "20th of March"
        ///}
        ///
        /// </remarks>
        /// <param name="publicMessageDTO"></param>
        /// <returns>A newly created Message</returns>
        /// <response code="201">Returns the newly created message</response>
        /// <response code="400">If the message is null</response>   
        // POST: api/v1/PublicMessage
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicMessage>> CreateNewPublicMessage(PublicMessageDTO publicMessageDTO)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                if (publicMessageDTO == null)
                {
                    //throw new ArgumentNullException("Public Message", "Public Message cannot be null");
                    return BadRequest();
                }

                PublicMessage publicMessage = PublicMessageDTOToPublicMessage(publicMessageDTO);
                _context.PublicMessages.Append(publicMessage);
                _context.Clients.Append(publicMessage.Client);
                await _context.SaveChangesAsync();

                //return CreatedAtAction("GetPublicMessage", new { id = publicMessage.MessageId }, publicMessage);
                return CreatedAtAction(nameof(GetPublicMessage), new { id = publicMessage.MessageId }, PublicMessageToDTO(publicMessage));
            }
            catch (ArgumentNullException ex)
            {
                _log.PublicMessageSingleLogError(ex.Message + " Stack trace: " + ex.StackTrace);
                return StatusCode(404, new MessageError(ex.Message, ex.StackTrace));
            }
            catch (Exception ex)
            {
                _log.PublicMessageSingleLogError(ex.Message + " Stack trace: " + ex.StackTrace);
                return StatusCode(500, new MessageError(ex.Message, ex.StackTrace));
            }
            finally
            {
                sw.Stop();
                _log.PublicMessageSingleLogError($"PublicMessage Controller. Elapsed : { sw.Elapsed.TotalMilliseconds}");
            }

        }

        /// <summary>
        /// Creates a message
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        /// DELETE /api/v1/PublicMessage/1
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>A newly created Message</returns>
        /// <response code="200">Returns the deleted message whose id matches the parameter</response>
        /// <response code="400">If the message is null</response>   
        // DELETE: api/v1/PublicMessage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublicMessage(long id)
        {
            var publicMessage = await _context.PublicMessages.FindAsync(id);
            if (publicMessage == null)
            {
                //throw new ArgumentNullException("Public Message", String.Format("Public Message with id {0} does not exist.", id));
                return NotFound();
            }

            _context.PublicMessages.Remove(publicMessage);
            await _context.SaveChangesAsync();

            return Content("Public message with title " + publicMessage.Title+ " is deleted");
        }

        #region PublicMessageController Helper code

        /// <summary>
        /// Method used by controller to convert public message to data transfer object
        /// </summary>
        /// <param name="publicMessage"></param>
        /// <returns>PublicMessageDTO object</returns>
        private static PublicMessageDTO PublicMessageToDTO(PublicMessage publicMessage) =>
            new PublicMessageDTO
            {
                Title = publicMessage.Title,
                MessageContent = publicMessage.MessageContent,
                ValidUntil = publicMessage.ValidUntil,
                AddressedTo = publicMessage.AddressedTo,
                ClientName = publicMessage.Client.FullName,
                ClientEmailId = publicMessage.Client.EmailId,
                MessageId = publicMessage.MessageId
            };

        /// <summary>
        /// Method used by controller to convert public message to data transfer object
        /// </summary>
        /// <param name="publicMessage"></param>
        /// <returns>PublicMessageDTO object</returns>
        private static PublicMessageDTO PublicMessageToDTOForGetAll(PublicMessage publicMessage) =>
            new PublicMessageDTO
            {
                Title = publicMessage.Title,
                MessageContent = publicMessage.MessageContent,
                ValidUntil = publicMessage.ValidUntil,
                AddressedTo = publicMessage.AddressedTo,
                MessageId = publicMessage.MessageId
            };



        /// <summary>
        /// Method used by controller to convert public message DTO to public message 
        /// </summary>
        /// <param name="publicMessageDTO"></param>
        /// <returns>PublicMessage object</returns>
        private static PublicMessage PublicMessageDTOToPublicMessage(PublicMessageDTO publicMessageDTO) =>
            new PublicMessage
            {
                Title = publicMessageDTO.Title,
                MessageContent = publicMessageDTO.MessageContent,
                PostedOn = DateTime.Now.ToLongDateString(),
                ValidUntil = publicMessageDTO.ValidUntil,
                AddressedTo = publicMessageDTO.AddressedTo,
                Client = new Client { FullName = publicMessageDTO.ClientName, EmailId = publicMessageDTO.ClientEmailId }

            };

        /// <summary>
        /// Method to check if public message exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true</returns> if it exists
        /// <returns>true</returns> if it does not exists
        private bool PublicMessageExists(long id)
        {
            return _context.PublicMessages.Any(e => e.MessageId == id);
        }
        #endregion

    }
}
