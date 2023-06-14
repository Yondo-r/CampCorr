using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Repositories.Interfaces;

namespace CampCorr.Repositories
{
    public class LogsRepository : ILogsRepository 
    {
        private readonly AppDbContext _context;

        public LogsRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Salvar(Logs logs) 
        {
            _context.Add(logs);
            _context.SaveChanges();
        }
    }
}
