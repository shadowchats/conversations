using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Shadowchats.Conversations.Infrastructure.Interceptors;

/// <summary>
/// Перехватчик для добавления конструкции FOR UPDATE SKIP LOCKED к SQL-запросам.
/// Модифицирует запросы, помеченные маркером через метод расширения ForUpdateSkipLocked().
/// </summary>
/// <remarks>
/// <para><b>Применимость:</b></para>
/// <list type="bullet">
///   <item>Простые SELECT-запросы с WHERE, ORDER BY, LIMIT/OFFSET</item>
///   <item>Паттерны обработки очередей (Outbox, Job Queue, Task Queue)</item>
///   <item>Конкурентная выборка строк для обработки несколькими воркерами</item>
/// </list>
/// 
/// <para><b>Ограничения (PostgreSQL):</b></para>
/// <list type="bullet">
///   <item>Не работает с GROUP BY, HAVING, агрегатными функциями (COUNT, SUM, AVG и т.д.)</item>
///   <item>Не работает с UNION, INTERSECT, EXCEPT</item>
///   <item>Не работает с DISTINCT</item>
///   <item>При использовании Include/ThenInclude блокируются только строки основной таблицы, 
///         связанные сущности НЕ блокируются</item>
///   <item>При наличии подзапросов блокируются только строки внешнего SELECT</item>
/// </list>
/// 
/// <para><b>Поведение:</b></para>
/// <list type="bullet">
///   <item>FOR UPDATE SKIP LOCKED добавляется в конец SQL после всех других конструкций</item>
///   <item>SKIP LOCKED: если строки заблокированы другой транзакцией, они пропускаются 
///         (не ожидается освобождение блокировки)</item>
///   <item>Маркер-комментарий удаляется из финального SQL</item>
///   <item>Если конструкция FOR UPDATE уже присутствует в запросе, модификация не выполняется</item>
/// </list>
/// 
/// <para><b>Для сложных запросов используйте FromSqlInterpolated:</b></para>
/// <code>
/// context.Entity.FromSqlInterpolated($"SELECT ... FOR UPDATE SKIP LOCKED")
/// </code>
/// </remarks>
public sealed class ForUpdateSkipLockedInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData _, InterceptionResult<DbDataReader> result)
    {
        ModifyCommand(command);
        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData _, InterceptionResult<DbDataReader> result, CancellationToken __ = default)
    {
        ModifyCommand(command);
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private static void ModifyCommand(DbCommand command)
    {
        if (!command.CommandText.Contains($"-- {Marker}"))
            return;

        var sql = command.CommandText
            .Replace($"-- {Marker}", string.Empty)
            .Trim();

        if (!sql.Contains("FOR UPDATE", StringComparison.OrdinalIgnoreCase))
        {
            sql = sql.TrimEnd(';');
            sql += "\nFOR UPDATE SKIP LOCKED";
        }

        command.CommandText = sql;
    }
    
    public const string Marker = "FORUPDATE_SKIPLOCKED";
}