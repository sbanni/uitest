namespace UITest.Core
{
    public interface ICommandExecutionGroup
    {
        bool IsCommandSupported(string commandName);
        void Execute(string commandName, IDictionary<string, object> parameters);
    }
}