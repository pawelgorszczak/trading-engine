namespace Trading_Engine.Application
{
    public interface ICommandHandler
    {

    }
    public interface ICommandHandler<in T> : ICommandHandler where T : ICommand
    {
        string Handle(T command);
    }
}