
using System;

namespace Models
{
    public class CommandModel
    {
        public CommandModel()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string CommandName { get; set; }
        public string Command { get; set; }
        public bool RunAsAdmin { get; set; } = false;
    }
}
