using lrn.devgalop.projectcreator.app.Services;

/**
Input parameters:
1) Folder Path (string) => C:\\temp\\HelloWorld
2) Project name (string) => hello_world
3) Project type (string) => webapi, console, webapp, etc... (See more executing: dotnet new list)
**/

try
{
    string folderSelected, projectName, projectType = string.Empty;
    if (args.Length < 3)
    {
        Console.WriteLine("To ensure proper execution, you need to specify the folder path, project name, and project type.");
        Console.WriteLine("What's the folder path: ");
        folderSelected = Console.ReadLine() ?? throw new Exception("The folder path is invalid. Please provide a valid path.");
        Console.WriteLine("Write the project name: ");
        projectName = Console.ReadLine() ?? throw new Exception("The project name is invalid. Please provide a valid name.");
        Console.WriteLine("Write the project type: ");
        projectType = Console.ReadLine() ?? throw new Exception("The project type is invalid. Please provide a valid type. To see the project types, execute the command 'dotnet new list'.");
    }else
    {
        folderSelected = args[0];
        projectName = args[1];
        projectType = args[2];
    }
    
    Console.WriteLine($"Folder path selected: {folderSelected}");
    Console.WriteLine($"Project name: {projectName}");
    Console.WriteLine($"Project type: {projectType}");

    string projectTypeCap = char.ToUpper(projectType[0]) + projectType.Substring(1);

    CommandService commandService = new CommandService();

    if (!Directory.Exists(folderSelected))
    {
        Directory.CreateDirectory(folderSelected);
    }

    List<string> commands = new()
    {
        $"dotnet new sln -n {projectName}",
        $"dotnet new classlib -n {projectName}.Core",
        $"dotnet new classlib -n {projectName}.Infrastructure",
        $"dotnet new {projectType} -n {projectName}.{projectTypeCap}",
        $"dotnet new xunit -n {projectName}.Tests",
        $"dotnet sln add ./{projectName}.Core/{projectName}.Core.csproj",
        $"dotnet sln add ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
        $"dotnet sln add ./{projectName}.{projectTypeCap}/{projectName}.{projectTypeCap}.csproj",
        $"dotnet sln add ./{projectName}.Tests/{projectName}.Tests.csproj",
        $"dotnet add {projectName}.Core/{projectName}.Core.csproj reference ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
        $"dotnet add {projectName}.{projectTypeCap}/{projectName}.{projectTypeCap}.csproj reference ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
        $"dotnet add {projectName}.{projectTypeCap}/{projectName}.{projectTypeCap}.csproj reference ./{projectName}.Core/{projectName}.Core.csproj",
        $"dotnet add {projectName}.Tests/{projectName}.Tests.csproj reference ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
        $"dotnet add {projectName}.Tests/{projectName}.Tests.csproj reference ./{projectName}.Core/{projectName}.Core.csproj",
        $"dotnet add {projectName}.Tests/{projectName}.Tests.csproj reference ./{projectName}.{projectTypeCap}/{projectName}.{projectTypeCap}.csproj",
        $"mkdir {folderSelected}\\{projectName}.Core\\Extensions {folderSelected}\\{projectName}.Core\\Services {folderSelected}\\{projectName}.Core\\Interfaces {folderSelected}\\{projectName}.Core\\Models {folderSelected}\\Docker",
        $"cd > Dockerfile"

    };
    
    foreach (var command in commands)
    {
        var commandResult = commandService.ExecuteCommand(folderSelected, command);
        if (!commandResult.IsSucceed)
        {
            throw new Exception(commandResult.ErrorMessage);
        }
        Console.WriteLine($"Command executed successfully. [COMMAND]:{command}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Project cannot be created. Error: {ex}");
}


