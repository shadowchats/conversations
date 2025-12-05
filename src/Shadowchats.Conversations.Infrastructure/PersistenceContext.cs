using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure;

public sealed class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        // Если бы в бизнес-домене были инварианты, которые возможно проконтролировать в БД (например, инвариант "У
        // юзеров должны быть уникальные логины" можно проконтролировать с помощью уникального индекса), то такие
        // инварианты прокидывались бы из БД в код именно таким способом с помощью исключений слоя Application. Слой
        // Application эти исключения может поймать и преобразовать в исключения слоя Domain, которые поймаются
        // Middleware и преобразуются в Bad Request. Если же до Middleware дойдет исключение слоя Application, то оно
        // преобразуется в Internal Server Error, как и любые другие неожиданные исключения (BugException и все прочие
        // исключения, не наследованные от BaseException слоя Domain).
        /*try
        {
            await _unitOfWork.DbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException
                                           {
                                               SqlState: "23505", ConstraintName: "IX_Accounts_Login"
                                           })
        {
            throw new EntityAlreadyExistsException<Account, string>(a => a.Credentials.Login);
        }*/
        
        await _unitOfWork.DbContext.SaveChangesAsync(cancellationToken);
    }
    
    private readonly UnitOfWork _unitOfWork;
}