using System.Linq.Expressions;
using Shadowchats.Conversations.Application.Enums;

namespace Shadowchats.Conversations.Application.Interfaces;

/// <summary>
/// Базовый репозиторий для работы с сущностями.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Добавляет новую сущность в контекст для последующего сохранения.
    /// </summary>
    /// <param name="entity">Сущность для добавления.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task Add(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Находит первую сущность, удовлетворяющую условию.
    /// </summary>
    /// <param name="predicate">Условие фильтрации.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <param name="lockMode">Режим блокировки строк в БД.</param>
    /// <param name="queryModifier">Опциональная функция для модификации запроса (например, сортировка, дополнительные фильтры).
    /// Может использоваться только для целей модификации IQueryable внутри репозитория.</param>
    /// <param name="includes">Связанные сущности для загрузки (Include).</param>
    /// <returns>Найденная сущность или null, если не найдена.</returns>
    /// <remarks>
    /// <para><b>Режимы блокировки (lockMode):</b></para>
    /// <list type="bullet">
    ///   <item><b>LockMode.None</b> (по умолчанию): без блокировки, обычное чтение.</item>
    ///   <item><b>LockMode.ForUpdateSkipLocked</b>: пессимистическая блокировка с пропуском занятых строк.
    ///         Если строка уже заблокирована другой транзакцией, она будет пропущена (не будет ожидания).
    ///         Используется для конкурентной обработки задач/событий несколькими воркерами.</item>
    /// </list>
    /// 
    /// <para><b>Важно при использовании LockMode.ForUpdateSkipLocked:</b></para>
    /// <list type="bullet">
    ///   <item>Блокируются только строки основной сущности (TEntity).</item>
    ///   <item>Связанные сущности, загруженные через <paramref name="includes"/>, 
    ///         НЕ блокируются и могут быть изменены другими транзакциями.</item>
    ///   <item>Если требуется блокировка связанных сущностей, выполняйте отдельные запросы 
    ///         с блокировкой для каждой таблицы.</item>
    ///   <item>Работает только с PostgreSQL (требует Npgsql провайдера).</item>
    ///   <item>Применимо только к простым запросам. Не используйте с агрегацией, группировкой, DISTINCT.</item>
    /// </list>
    /// </remarks>
    Task<TEntity?> Find(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken,
        LockMode lockMode = LockMode.None,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null,
        params Expression<Func<TEntity, object>>[] includes);

    /// <summary>
    /// Находит все сущности, удовлетворяющие условию.
    /// </summary>
    /// <param name="predicate">Условие фильтрации.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <param name="batchSize">Максимальное количество возвращаемых записей (LIMIT).</param>
    /// <param name="lockMode">Режим блокировки строк в БД.</param>
    /// <param name="queryModifier">Опциональная функция для модификации запроса (например, сортировка, дополнительные фильтры).
    /// Может использоваться только для целей модификации IQueryable внутри репозитория.</param>
    /// <param name="includes">Связанные сущности для загрузки (Include).</param>
    /// <returns>Список найденных сущностей (может быть пустым).</returns>
    /// <remarks>
    /// <para><b>Режимы блокировки (lockMode):</b></para>
    /// <list type="bullet">
    ///   <item><b>LockMode.None</b> (по умолчанию): без блокировки, обычное чтение.</item>
    ///   <item><b>LockMode.ForUpdateSkipLocked</b>: пессимистическая блокировка с пропуском занятых строк.
    ///         Блокирует найденные строки для текущей транзакции, пропускает уже заблокированные.
    ///         Идеально для пакетной обработки задач несколькими воркерами.</item>
    /// </list>
    /// 
    /// <para><b>Важно при использовании LockMode.ForUpdateSkipLocked:</b></para>
    /// <list type="bullet">
    ///   <item>Блокируются только строки основной сущности (TEntity).</item>
    ///   <item>Связанные сущности, загруженные через <paramref name="includes"/>, 
    ///         НЕ блокируются и могут быть изменены другими транзакциями.</item>
    ///   <item>Возвращаемый список может содержать меньше элементов, чем <paramref name="batchSize"/>, 
    ///         если часть строк заблокирована другими транзакциями.</item>
    ///   <item>Работает только с PostgreSQL (требует Npgsql провайдера).</item>
    ///   <item>Применимо только к простым запросам. Не используйте с агрегацией, группировкой, DISTINCT.</item>
    /// </list>
    /// </remarks>
    Task<List<TEntity>> FindAll(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken,
        int batchSize = int.MaxValue,
        LockMode lockMode = LockMode.None,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryModifier = null,
        params Expression<Func<TEntity, object>>[] includes);
}