using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lrn.devgalop.projectcreator.app.Services
{
    public abstract class GeneratorService
    {
        protected readonly ICommandService _commandService;
        protected readonly ITechnologiesService _technologiesService;
        protected readonly ITemplateService _templateService;

        public GeneratorService(
            ICommandService commandService,
            ITechnologiesService technologiesService,
            ITemplateService templateService)
        {
            _commandService = commandService;
            _technologiesService = technologiesService;
            _templateService = templateService;
        }

        public abstract List<string> GetCommands(string projectName, string projectType, string projectTypeCap, string folderSelected, bool includeTests = true, bool includeDocker = true);
        public abstract void RunCommands(bool includeTests = true, bool includeDocker = true);
    }

}