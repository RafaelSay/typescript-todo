﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HelloWebApi2018.Data;
using HelloWebApi2018.Models;

namespace HelloWebApi2018.Controllers {
    public class ToDosController :ApiController {
        private ToDoContext db = new ToDoContext();

        // GET: api/ToDos
        public IHttpActionResult GetToDos() {
            return Ok(db.ToDos.ToList());
        }

        // GET: api/ToDos/5
        [ResponseType(typeof(ToDo))]
        public IHttpActionResult GetToDo(int id) {
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null) {
                return NotFound();
            }

            return Ok(toDo);
        }

        // PUT: api/ToDos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutToDo(int id, ToDo toDo) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (id != toDo.Id) {
                return BadRequest();
            }

            db.Entry(toDo).State = EntityState.Modified;

            try {
                db.SaveChanges();
            } catch (DbUpdateConcurrencyException) {
                if (!ToDoExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ToDos
        [ResponseType(typeof(ToDo))]
        public IHttpActionResult PostToDo(ToDo toDo) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            toDo.DueDate = DateTime.Now.AddDays(5);
            db.ToDos.Add(toDo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = toDo.Id }, toDo);
        }

        // DELETE: api/ToDos/5
        [ResponseType(typeof(ToDo))]
        public IHttpActionResult DeleteToDo(int id) {
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null) {
                return NotFound();
            }

            db.ToDos.Remove(toDo);
            db.SaveChanges();

            return Ok(toDo);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ToDoExists(int id) {
            return db.ToDos.Count(e => e.Id == id) > 0;
        }
    }
}