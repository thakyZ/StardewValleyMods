namespace ClearFarmDebris.Framework;

internal class Command {
  public string Name { get; private set; }
  public string CLI { get; private set; }
  public string Description { get; private set; }
  public string[] Sets { get; private set; }
  public Action<string, string[], string[]> CommandAction { get; private set; }

  public Command(string name, string cli, string description, string[] sets, Action<string, string[], string[]> action) {
    this.Name = name;
    this.CLI = cli;
    this.Description = description;
    this.Sets = sets;
    this.CommandAction = action;
  }

  public void Init(ICommandHelper commands) {
    commands.Add(this.CLI, this.Description, (string command, string[] args) => this.CommandAction(command, args, this.Sets));
  }

  public void Run(string command, string[]? args = null) {
    this.CommandAction(command, args ?? [], this.Sets);
  }
}
