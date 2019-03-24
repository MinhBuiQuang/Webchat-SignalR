using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ChatApp;
using ChatApp.Models;

namespace ChatApp.Controllers
{
    public class MessageLogsController : ApiController
    {
        private WebChatEntities db = new WebChatEntities();

        // GET: api/MessageLogs
        public IQueryable<MessageLogModel> GetMessageLogs(int yeucauid)
        {
            var model = db.MessageLogs.Where(p => p.YeuCauID == yeucauid)
                .Select(c => new MessageLogModel
                {
                    MessageID = c.MessageID,
                    Message = c.Message,
                    YeuCauID = c.YeuCauID,
                    Timestamp = c.Timestamp,
                    UserID = c.UserID,
                    User = new UserModel {
                        UserID = c.User.UserID,
                        Username = c.User.Username,
                        ClientChatID = c.User.ClientChatID,
                        IsAdmin = c.User.IsAdmin,
                        LastOnline = c.User.LastOnline
                    }
                });
            return model;
        }

        // GET: api/MessageLogs/5
        [ResponseType(typeof(MessageLog))]
        public IHttpActionResult GetMessageLog(int id)
        {
            MessageLog messageLog = db.MessageLogs.Find(id);
            if (messageLog == null)
            {
                return NotFound();
            }

            return Ok(messageLog);
        }

        // PUT: api/MessageLogs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMessageLog(int id, MessageLog messageLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != messageLog.MessageID)
            {
                return BadRequest();
            }

            db.Entry(messageLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/MessageLogs
        [ResponseType(typeof(MessageLog))]
        public IHttpActionResult PostMessageLog(MessageLog messageLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MessageLogs.Add(messageLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = messageLog.MessageID }, messageLog);
        }

        // DELETE: api/MessageLogs/5
        [ResponseType(typeof(MessageLog))]
        public IHttpActionResult DeleteMessageLog(int id)
        {
            MessageLog messageLog = db.MessageLogs.Find(id);
            if (messageLog == null)
            {
                return NotFound();
            }

            db.MessageLogs.Remove(messageLog);
            db.SaveChanges();

            return Ok(messageLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageLogExists(int id)
        {
            return db.MessageLogs.Count(e => e.MessageID == id) > 0;
        }
    }
}