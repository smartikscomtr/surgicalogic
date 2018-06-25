using Smartiks.Teydeb.Surgicalogic.ConsoleApp.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartiks.Teydeb.Surgicalogic.ConsoleApp.Model
{
    public class OperationInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Doctor Doctor { get; set; }
        public int Period { get; set; }
        public List<int> UnavailableRooms { get; set; }
    }
}
