using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lrn.devgalop.projectcreator.app.Services
{
    public class CSharpGeneratorService : GeneratorService
    {
        public CSharpGeneratorService(
            ICommandService commandService,
            ITechnologiesService technologiesService,
            ITemplateService templateService) : base(commandService, technologiesService, templateService)
        {
        }

        public override List<string> GetCommands(string projectName, string projectType, string projectTypeCap, string folderSelected, bool includeTests = true, bool includeDocker = true)
        {
            var templateSelected = _templateService.GetTemplate();
            if(string.IsNullOrEmpty(templateSelected))templateSelected = "onion";
            var folders = _templateService.GetTemplateFolders(templateSelected);

            List<string> commands = new()
            {
                $"dotnet new sln -n {projectName}",
                $"dotnet add {projectName}.Core/{projectName}.Core.csproj reference ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
                $"dotnet add {projectName}.{projectTypeCap}/{projectName}.{projectTypeCap}.csproj reference ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
                $"dotnet add {projectName}.{projectTypeCap}/{projectName}.{projectTypeCap}.csproj reference ./{projectName}.Core/{projectName}.Core.csproj",
                $"dotnet add {projectName}.Tests/{projectName}.Tests.csproj reference ./{projectName}.Infrastructure/{projectName}.Infrastructure.csproj",
                $"dotnet add {projectName}.Tests/{projectName}.Tests.csproj reference ./{projectName}.Core/{projectName}.Core.csproj",
                $"dotnet add {projectName}.Tests/{projectName}.Tests.csproj reference ./{projectName}.{projectTypeCap}/{projectName}.{projectTypeCap}.csproj",
            };

            // Create projects and assign them to solutions
            foreach (var folder in folders)
            {
                if(folder == "ProjectType")
                {
                    commands.Add($"dotnet new {projectType} -n {projectName}.{projectTypeCap}");
                    commands.Add($"dotnet sln add ./{projectName}.{projectTypeCap}/{projectName}.{projectTypeCap}.csproj");
                    continue;
                } 
                commands.Add($"dotnet new classlib -n {projectName}.{folder}");
                commands.Add($"dotnet sln add ./{projectName}.{folder}/{projectName}.{folder}.csproj");
            }

            // Add reference to projects
            

            // Create test project
            if(includeTests)
            {
                commands.Add($"dotnet new xunit -n {projectName}.Tests");
                commands.Add($"dotnet sln add ./{projectName}.Tests/{projectName}.Tests.csproj");
            }

            //Create empty dockerfile
            if(includeDocker)
            {
                commands.Add($"mkdir {folderSelected}\\Docker");
                commands.Add($"cd > Dockerfile");
            }

            return commands;
        }

        public override void RunCommands(bool includeTests = true, bool includeDocker = true)
        {
            string folderSelected, projectName, projectType = string.Empty;

            Console.WriteLine("To ensure proper execution, you need to specify the folder path, project name, and project type.");
            Console.WriteLine("What's the folder path: ");
            folderSelected = Console.ReadLine() ?? throw new Exception("The folder path is invalid. Please provide a valid path.");
            Console.WriteLine("Write the project name: ");
            projectName = Console.ReadLine() ?? throw new Exception("The project name is invalid. Please provide a valid name.");
            Console.WriteLine("Write the project type: ");
            projectType = Console.ReadLine() ?? throw new Exception("The project type is invalid. Please provide a valid type. To see the project types, execute the command 'dotnet new list'.");

            Console.WriteLine($"Folder path selected: {folderSelected}");
            Console.WriteLine($"Project name: {projectName}");
            Console.WriteLine($"Project type: {projectType}");

            string projectTypeCap = char.ToUpper(projectType[0]) + projectType.Substring(1);

            CommandService commandService = new CommandService();

            if (!Directory.Exists(folderSelected))
            {
                Directory.CreateDirectory(folderSelected);
            }

            List<string> commands = GetCommands(projectName, projectType, projectTypeCap, folderSelected, includeTests, includeDocker);

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
    }
}