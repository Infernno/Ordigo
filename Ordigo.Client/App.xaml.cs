using MaterialDesignThemes.Wpf;
using NLog;
using Ordigo.API.Contracts;
using Ordigo.API.Infrastructure;
using Ordigo.Client.Controls.Pages;
using Ordigo.Client.Core.UI;
using Ordigo.Client.Core.ViewModels;
using RestSharp;
using SimpleInjector;
using System;
using System.Configuration;
using System.IO;
using System.Windows;

namespace Ordigo.Client
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <inheritdoc />
        /// <summary>
        /// Входная точка приложения
        /// </summary>
        /// <param name="e">Аргументы для события при старте приложения</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if DEBUG
            if (File.Exists("log.txt"))
            {
                File.Delete("log.txt");
            }
#endif

            var window = new MainWindow();
            var container = ConfigureContainer();

            ConfigureExceptionHandler();

            NavigationManager.Current.SetResolver(container); // Запоминаем контейнер с зависимосями
                                                              // чтобы можно было создавать View
            NavigationManager.Current.SetRoot(window); // MainWindow является самим окном, в котором будут находится View
            NavigationManager.Current.GoTo("SignInPage"); // Переход к экрану входа

            window.Show();
        }

        /// <summary>
        /// Создаем контейнер и регистрируем все классы которые нам понадобятся
        /// </summary>
        /// <returns><see cref="Container"/>, содержащий все зависимости</returns>
        private static Container ConfigureContainer()
        {
            var container = new Container();

            // Означает что например если класс потребует в конструкторе IApiClient, то мы отдадим ему ApiClient
            // Lifestyle.Singleton - между разными классами будет использоваться один и тот же экземпляр класса
            container.Register<IApiClient, ApiClient>(Lifestyle.Singleton);

            container.RegisterInstance<IRestClient>(new RestClient(ConfigurationManager.AppSettings.Get("ServerUrl")));
            container.RegisterInstance<ISnackbarMessageQueue>(new SnackbarMessageQueue(TimeSpan.FromSeconds(5)));

            container.Register<SignInViewModel>();
            container.Register<SignUpViewModel>();
            container.Register<MainViewModel>();

            container.Register<SignInPage>();
            container.Register<SignUpPage>();
            container.Register<MainPage>();

            return container;
        }

        private static void ConfigureExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var logger = LogManager.GetCurrentClassLogger(typeof(App));

                logger.Fatal("**** FATAL ERROR ****");
                logger.Fatal("Произошла критическая ошибка в приложении!");

                logger.Fatal(sender);
                logger.Fatal(args.ExceptionObject);

                logger.Fatal("**** END ****");
            };
        }
    }
}
