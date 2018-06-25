using System.Collections.Generic;

namespace Smartiks.Teydeb.Surgicalogic.ConsoleApp.OutputModel
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Operation> Operations { get; set; }
    }
}
