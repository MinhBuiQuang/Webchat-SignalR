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
    public class YeuCausController : ApiController
    {
        private WebChatEntities db = new WebChatEntities();

        // GET: api/YeuCaus
        public List<YeuCauModel> GetYeuCaus()
        {
            var model = db.YeuCaus.ToList().Select(c => new YeuCauModel
            {
                YeuCauID = c.YeuCauID,
                MessageLogs = c.MessageLogs.Where(p => p.MessageID == c.MessageLogs.Max(x => x.MessageID)).Select(k => new MessageLogModel {
                    MessageID = k.MessageID,
                    Message = k.Message,
                    YeuCauID = k.YeuCauID,
                    Timestamp = k.Timestamp,
                    UserID = k.UserID,
                    User = new UserModel
                    {
                        UserID = k.User.UserID,
                        Username = k.User.Username,
                        ClientChatID = k.User.ClientChatID,
                        IsAdmin = k.User.IsAdmin,
                        LastOnline = k.User.LastOnline
                    }
                }).ToList(),
                TieuDe = c.TieuDe
            }).ToList();
            //var model = from c in db.YeuCaus
            //            select new YeuCau
            //            {
            //                YeuCauID = c.YeuCauID,
            //                TieuDe = c.TieuDe,
            //                MessageLogs = c.MessageLogs.Where(p => p.MessageID == 4).ToList()
            //            };
            return model;
        }

        // GET: api/YeuCaus/5
        [ResponseType(typeof(YeuCau))]
        public IHttpActionResult GetYeuCau(int id)
        {
            YeuCau yeuCau = db.YeuCaus.Find(id);
            if (yeuCau == null)
            {
                return NotFound();
            }

            return Ok(yeuCau);
        }

        // PUT: api/YeuCaus/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutYeuCau(int id, YeuCau yeuCau)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != yeuCau.YeuCauID)
            {
                return BadRequest();
            }

            db.Entry(yeuCau).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YeuCauExists(id))
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

        // POST: api/YeuCaus
        [ResponseType(typeof(YeuCau))]
        public IHttpActionResult PostYeuCau(YeuCau yeuCau)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.YeuCaus.Add(yeuCau);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = yeuCau.YeuCauID }, yeuCau);
        }

        // DELETE: api/YeuCaus/5
        [ResponseType(typeof(YeuCau))]
        public IHttpActionResult DeleteYeuCau(int id)
        {
            YeuCau yeuCau = db.YeuCaus.Find(id);
            if (yeuCau == null)
            {
                return NotFound();
            }

            db.YeuCaus.Remove(yeuCau);
            db.SaveChanges();

            return Ok(yeuCau);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool YeuCauExists(int id)
        {
            return db.YeuCaus.Count(e => e.YeuCauID == id) > 0;
        }
    }
}