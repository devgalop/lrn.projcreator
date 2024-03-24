using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lrn.devgalop.projectcreator.app.Models
{
    public class GenericResponse<T>
    {
        public bool IsSucceed { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Result { get; set; }
    }
}
