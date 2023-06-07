//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
namespace CampCorr.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Executa a próxima etapa do pipeline
                await _next(context);

                // Registra o log de sucesso no banco de dados
                LogAccess(context, false);
            }
            catch (Exception ex)
            {
                // Registra o log de erro no banco de dados
                LogAccess(context, true);

                // Re-levanta a exceção para que o tratamento de erro padrão seja executado
                throw;
            }
        }

        private void LogAccess(HttpContext context, bool isError)
        {
            string controller = context.Request.RouteValues["controller"]?.ToString();
            string action = context.Request.RouteValues["action"]?.ToString();
            string login = context.User.Identity.Name;
            string ip = context.Connection.RemoteIpAddress.ToString();

            if (isError)
            {
                _logger.LogError("Erro na action {Controller}/{Action}. Login: {Login}, IP: {IP}", controller, action, login, ip);
            }
            else
            {
                _logger.LogInformation("Acesso bem-sucedido na action {Controller}/{Action}. Login: {Login}, IP: {IP}", controller, action, login, ip);
            }

            // Grava os logs no banco de dados conforme necessário
            // Exemplo:
            // dbContext.Logs.Add(new LogEntry { Controller = controller, Action = action, Login = login, IP = ip, IsError = isError });
            // dbContext.SaveChanges();
        }
    }

}
