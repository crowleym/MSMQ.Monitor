using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace MSMQ.Monitor
{
    public class LocalDataContext : DbContext
    {
        public DbSet<MSMQDataLog> MSMQDataLogs { get; set; }
    }
}
