namespace CommandSystem
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}