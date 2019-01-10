namespace Ordigo.Server.Core.Contracts
{
    /// <summary>
    /// Интерфейс, описывающий сущность в базе данных с уникальным идентификатором (ID)
    /// </summary>
    public interface IEntityBase
    {
        /// <summary>
        /// Номер записи в базе данных
        /// </summary>
        int Id { get; set; }
    }
}
