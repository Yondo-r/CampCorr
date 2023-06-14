//using System;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
using CampCorr.Context;
using CampCorr.Models;
using CampCorr.Services.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;

namespace CampCorr.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        //private readonly ILogService _logService;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger/*, ILogService logService*/)
        {
            _next = next;
            _logger = logger;
            //_logService = logService;
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

            //Grava os logs no banco de dados conforme necessário
            //Exemplo:
            Logs logs = new Logs()
            {
                Controller = controller,
                Action = action,
                Login = login ?? "",
                Ip = ip,
                Horario = DateTime.Now,
                IsError = isError
            };
            if (logs.Controller != null || logs.Action != null)
            {
                using (var scope = context.RequestServices.CreateScope())
                {
                    var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                    // Registra o log de erro no banco de dados
                    logService.Salvar(logs);
                }
            }
        }
    }

}
