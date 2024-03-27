using lrn.devgalop.projectcreator.app.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lrn.devgalop.projectcreator.app.Services
{
    public class CommandService : ICommandService
    {
        public GenericResponse<ConsoleResponse> ExecuteCommand(string workingDirectory, string command)
        {
			try
			{
                ConsoleResponse response = new();
                using (Process process = new Process())
                {
                    process.StartInfo.Verb = "runas";
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.WorkingDirectory = workingDirectory;
                    process.StartInfo.Arguments = @"/c" + command;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.Start();

                    // Synchronously read the standard output of the spawned process.
                    StreamReader reader = process.StandardOutput;
                    StreamReader readerError = process.StandardError;
                    response.Response = reader.ReadToEnd();
                    response.Error = readerError.ReadToEnd();
                    // Write the redirected output to this application's window.
                    process.WaitForExit();
                }
                return new()
				{
					IsSucceed = true,
					Result = response
				};
			}
			catch (Exception ex)
			{
				return new()
				{
					IsSucceed = false,
					ErrorMessage = ex.ToString(),
				};
			}
        }
    }

    public interface ICommandService
    {
        GenericResponse<ConsoleResponse> ExecuteCommand(string workingDirectory, string command);
    }
}
