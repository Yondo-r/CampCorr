using CampCorr.Models;
using CampCorr.Repositories.Interfaces;
using CampCorr.Services.Interfaces;

namespace CampCorr.Services
{
    public class LogService : ILogService
    {
        private readonly ILogsRepository _logsRepository;

        public LogService(ILogsRepository logsRepository)
        {
            _logsRepository = logsRepository;
        }

        public void Salvar(Logs logs)
        {
            _logsRepository.Salvar(logs);
        }
    }
}
