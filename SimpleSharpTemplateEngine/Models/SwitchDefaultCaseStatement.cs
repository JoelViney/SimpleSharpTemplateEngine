using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class SwitchDefaultCaseStatement : ISwitchCaseObject
    {
        public ContainerObject Contents { get; }

        public SwitchDefaultCaseStatement(ContainerObject contents)
        {
            this.Contents = contents;
        }

        public bool MatchExpression(object? value)
        {
            return true;
        }

        public StringBuilder Process(object model)
        {
            return this.Contents.Process(model);
        }
    }
}
