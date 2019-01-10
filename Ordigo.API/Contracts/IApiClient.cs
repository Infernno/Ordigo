namespace Ordigo.API.Contracts
{
    /// <summary>
    /// Основной интерфейс для взаимодействия с API сервера
    /// </summary>
    public interface IApiClient : IApiInvoke, IApiAuthAsync
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        string Username { get; }
        
        /// <summary>
        /// API для работы с заметками
        /// </summary>
        INotesApi Notes { get; }
    }
}
