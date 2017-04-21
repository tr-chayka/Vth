using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project
{
    public class Movement
    {
        [Key]
        public int ID { get; set; }

        public bool User { get; set; }
        public int Position { get; set; }
    }

    public class GameLog
    {
        [Key]
        public int ID { get; set; }

        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public int Result { get; set; }

        public List<Movement> Log { get; set; }

        public GameLog()
        {
            Log = new List<Movement>();
        }
    }

    public class DatabaseModel : DbContext
    {
        public DatabaseModel() : base("GameDatabase")
        { }

        public DbSet<GameLog> GameLog { get; set; }
        public DbSet<Movement> Movement { get; set; }
    }
}