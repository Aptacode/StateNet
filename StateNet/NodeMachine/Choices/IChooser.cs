using System;

namespace Aptacode.StateNet.NodeMachine.Choices
{
    public interface IChooser<TChoice>
        where TChoice : System.Enum
    {
        TChoice GetChoice();
    }
}
