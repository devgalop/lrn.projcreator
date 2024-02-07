﻿// See https://aka.ms/new-console-template for more information



using lrn.devgalop.projectcreator.app.Services;
/**
Parametros de entrada:
1) NombreCarpeta (string) => D:\\Projects\\MiPrimerProyecto
2) NombreProyecto (string) => lrn.mi_primer_proyecto
3) TipoProyecto (string) => webapi, console, webapp, etc... (Consultar tipos de proyectos con el comando dotnet new list)
**/

try
{
    args = new[] { "D:\\Projects\\MiPrimerProyecto", "lrn.mi_primer_proyecto", "console" };
    if (args.Length < 3)
    {
        throw new ArgumentNullException("No se han declarado todas las variables necesarias para el proceso");
    }
    string folderSelected = args[0];
    Console.WriteLine($"Carpeta de proyecto: {folderSelected}");
    string projectName = args[1];
    Console.WriteLine($"Nombre del prooyecto: {projectName}");
    string projectType = args[2];
    Console.WriteLine($"Tipo de proyecto: {projectType}");

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
        $"dotnet new {projectType} -n {projectName}.App",
        $"dotnet sln add ./{projectName}.Core/{projectName}.Core.csproj",
        $"dotnet sln add ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
        $"dotnet sln add ./{projectName}.App/{projectName}.App.csproj",
        $"dotnet add {projectName}.Core/{projectName}.Core.csproj reference ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
        $"dotnet add {projectName}.App/{projectName}.App.csproj reference ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
        $"dotnet add {projectName}.App/{projectName}.App.csproj reference ./{projectName}.Core/{projectName}.Core.csproj",
    };
    
    foreach (var command in commands)
    {
        var commandResult = commandService.ExecuteCommand(folderSelected, command);
        if (!commandResult.IsSucessfully)
        {
            throw new Exception(commandResult.ErrorMessage);
        }
        Console.WriteLine($"Comando ejecutado con éxito. [COMANDO]:{command}");
    }
}
catch (Exception)
{

	throw;
}


