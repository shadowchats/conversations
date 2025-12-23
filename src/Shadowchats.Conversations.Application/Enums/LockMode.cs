namespace Shadowchats.Conversations.Application.Enums;

/// <summary>
/// Режимы блокировки строк в базе данных.
/// </summary>
public enum LockMode : byte
{
    /// <summary>
    /// Без блокировки. Обычное чтение данных.
    /// </summary>
    None = 0,
    /// <summary>
    /// Пессимистическая блокировка с пропуском уже заблокированных строк (PostgreSQL: FOR UPDATE SKIP LOCKED).
    /// </summary>
    /// <remarks>
    /// <para><b>Поведение:</b></para>
    /// <list type="bullet">
    ///   <item>Блокирует выбранные строки для текущей транзакции.</item>
    ///   <item>Если строка уже заблокирована другой транзакцией, она пропускается (не ожидается освобождение).</item>
    ///   <item>Другие транзакции не смогут изменить заблокированные строки до завершения текущей транзакции.</item>
    ///   <item>Блокируются только строки основной таблицы, связанные сущности (Include) НЕ блокируются.</item>
    /// </list>
    /// 
    /// <para><b>Использование:</b></para>
    /// <list type="bullet">
    ///   <item>Конкурентная обработка очередей задач/событий несколькими воркерами.</item>
    ///   <item>Распределённая обработка данных без дублирования работы.</item>
    ///   <item>Паттерны Outbox, Job Queue, Task Queue.</item>
    /// </list>
    /// 
    /// <para><b>Ограничения:</b></para>
    /// <list type="bullet">
    ///   <item>Работает только с PostgreSQL.</item>
    ///   <item>Не применяется к запросам с GROUP BY, HAVING, DISTINCT, UNION, агрегацией.</item>
    ///   <item>Требует активной транзакции (явной в обработчике или неявной в декораторе).</item>
    /// </list>
    /// </remarks>
    ForUpdateSkipLocked = 1
}