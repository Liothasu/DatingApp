namespace API.Interfaces
{
    public class IUnitOfWork
    {
        IUserRepository UserRepository {get; }
        IMessageRepository MessageRepository{ get; }
        ILikesRepository LikesRepository {get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}