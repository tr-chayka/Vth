using System.Collections.Generic;

namespace Project.Models
{
    public class ArchiveModel
    {
        List<GameLog> Logs = new List<GameLog>();

        public ArchiveModel()
        {
            DatabaseModel Database = new DatabaseModel();

            foreach (GameLog Log in Database.GameLog.Include("Log"))
                Logs.Add(Log);
        }

        public GameLog this[int index]
        {
            get
            {
                return Logs[index];
            }
        }

        public int LogCount
        {
            get
            {
                return Logs.Count;
            }
        }

        public List<Movement> GetGameHistory( int index)
        {
            return Logs[index].Log;
        }
    }
}