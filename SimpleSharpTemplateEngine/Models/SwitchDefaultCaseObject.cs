namespace SimpleSharpTemplateEngine.Models
{
    internal class SwitchDefaultCaseObject : ISwitchCaseObject
    {
        public ContainerObject Contents { get; }

        public SwitchDefaultCaseObject(ContainerObject contents)
        {
            this.Contents = contents;
        }

        public bool MatchExpression(object? value)
        {
            return true;
        }
    }
}
