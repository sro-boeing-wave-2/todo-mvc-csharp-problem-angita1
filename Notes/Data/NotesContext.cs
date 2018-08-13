using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Notes.Models
{
    public class NotesContext
    {
        //{
        //    public NotesContext (DbContextOptions<NotesContext> options)
        //        : base(options)
        //    {
        //    }

        //    public DbSet<Notes.Models.Note> Note { get; set; }
        //}

        private readonly IMongoDatabase _database = null;

        public NotesContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Note> Notes
        {
            get
            {
                return _database.GetCollection<Note>("Note");
            }
        }
    }
}
