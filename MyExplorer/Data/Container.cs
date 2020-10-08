using MyExplorer.Enums;
using System.Collections.Generic;
using System.Windows.Controls;

namespace MyExplorer.Data
{
    public class Container
    {
        public Grid ContainerGrid { get; set; }
        public FrameName CurrentUI { get; set; } = FrameName.Null;
        public Stack<FrameName> PreviousUI { get; set; } = new Stack<FrameName>();
    }
}
