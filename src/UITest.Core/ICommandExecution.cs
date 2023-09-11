namespace UITest.Core
{
    public interface ICommandExecution
    {
        void AddCommandGroup(ICommandExecutionGroup commandExecuteGroup);
        void Execute(string commandName, IDictionary<string, object> parameters);
    }
}