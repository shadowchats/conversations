using Microsoft.EntityFrameworkCore;
using Shadowchats.Conversations.Infrastructure.Interceptors;

namespace Shadowchats.Conversations.Infrastructure.Extensions;

/// <summary>
/// Расширения для добавления PostgreSQL-специфичных конструкций.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Добавляет конструкцию FOR UPDATE SKIP LOCKED к запросу (PostgreSQL).
    /// Используется для пессимистической блокировки строк с пропуском уже заблокированных.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    /// <param name="query">Исходный запрос.</param>
    /// <returns>Запрос с маркером для добавления FOR UPDATE SKIP LOCKED.</returns>
    /// <remarks>
    /// <para><b>Типичное использование:</b></para>
    /// <code>
    /// var events = await context.OutboxEvents
    ///     .Where(e => e.Status == Pending)
    ///     .OrderBy(e => e.CreatedAt)
    ///     .Take(batchSize)
    ///     .ForUpdateSkipLocked()
    ///     .ToListAsync();
    /// </code>
    /// 
    /// <para><b>Сгенерированный SQL:</b></para>
    /// <code>
    /// SELECT * FROM "OutboxEvents" 
    /// WHERE "Status" = @p0 
    /// ORDER BY "CreatedAt" 
    /// LIMIT @p1
    /// FOR UPDATE SKIP LOCKED
    /// </code>
    /// 
    /// <para><b>Когда использовать:</b></para>
    /// <list type="bullet">
    ///   <item>Конкурентная обработка очередей несколькими воркерами</item>
    ///   <item>Выборка задач/событий для обработки (Outbox, Job Queue)</item>
    ///   <item>Любые сценарии, где нужно "захватить" строки без ожидания блокировок</item>
    /// </list>
    /// 
    /// <para><b>Когда НЕ использовать:</b></para>
    /// <list type="bullet">
    ///   <item>Запросы с GROUP BY, HAVING, агрегацией</item>
    ///   <item>Запросы с UNION, INTERSECT, EXCEPT, DISTINCT</item>
    ///   <item>Сложные запросы с подзапросами или множественными JOIN</item>
    ///   <item>Когда нужна блокировка связанных сущностей (Include не блокирует вложенные таблицы)</item>
    /// </list>
    /// 
    /// <para><b>Важно:</b></para>
    /// <list type="bullet">
    ///   <item>Работает только с PostgreSQL (требует Npgsql провайдера)</item>
    ///   <item>Требует регистрации ForUpdateSkipLockedInterceptor в DbContext</item>
    ///   <item>Include/ThenInclude блокирует только основную сущность, не связанные</item>
    ///   <item>Для сложных случаев используйте FromSqlInterpolated</item>
    /// </list>
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Может быть выброшено PostgreSQL, если запрос несовместим с FOR UPDATE.
    /// </exception>
    public static IQueryable<T> ForUpdateSkipLocked<T>(this IQueryable<T> query) =>
        query.TagWith(ForUpdateSkipLockedInterceptor.Marker);
}