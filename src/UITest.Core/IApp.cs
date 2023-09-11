namespace UITest.Core
{
    public interface IApp : IDisposable
    {
        IConfig Config { get; }
        IElementQueryable Query { get; }
        ApplicationState AppState { get; }
        IElement FindElement(string id);
        IElement FindElement(IQuery query);
        ICommandExecution CommandExecutor { get; }
        void Click(float x, float y);
        FileInfo Screenshot(string fileName);
        string ElementTree { get; }
    }
}
