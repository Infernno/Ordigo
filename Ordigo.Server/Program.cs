using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Ordigo.Server
{
    /// <summary>
    /// Главный класс приложения
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Входная точка приложения
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
        private static void Main(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

            var host = builder.Build();

            host.Run();
        }
    }
}
