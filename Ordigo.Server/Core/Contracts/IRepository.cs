using System;
using System.Collections.Generic;

namespace Ordigo.Server.Core.Contracts
{
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Возвращает <see cref="IEnumerable{TEntity}"/>, содержащий всех модели 
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Возвращает модель соответствующую заданному предикату
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns></returns>
        TEntity Find(Func<TEntity, bool> predicate);

        /// <summary>
        /// Возвращает модель с указанным ID
        /// </summary>
        /// <param name="id">ID модели</param>
        /// <returns></returns>
        TEntity GetById(int id);

        /// <summary>
        /// Проверяет, содержится ли модель с указанным ID
        /// </summary>
        /// <param name="id">ID модели</param>
        /// <returns></returns>
        bool Contains(int id);

        /// <summary>
        /// Проверяет, содержится ли модель с указанным предикатом
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns></returns>
        bool Contains(Func<TEntity, bool> predicate);

        /// <summary>
        /// Добавляет модель в базу данных
        /// </summary>
        /// <param name="item"></param>
        void Add(TEntity item);

        /// <summary>
        /// Обновляет данные модели в базе
        /// </summary>
        /// <param name="item"></param>
        void Update(TEntity item);

        /// <summary>
        /// Удаляет модель из базы данных
        /// </summary>
        /// <param name="item"></param>
        void Remove(TEntity item);

        /// <summary>
        /// Удаляет модель из базы данных по Id
        /// </summary>
        /// <param name="id"></param>
        void Remove(int id);
    }
}
