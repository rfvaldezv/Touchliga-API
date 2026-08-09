namespace Touchliga.ModuleGenerator.Commands;

public interface ICommand
{
    Task<int> ExecuteAsync(string[] args);
}
