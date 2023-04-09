using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class ElseStatement : IIfStatementObject, ITemplateObject, IIfStatementChainingObject
    {
        public ContainerObject Contents { get; }

        public ElseStatement(ContainerObject contents)
        {
            this.Contents = contents;
        }

        public bool MatchesExpression(object model)
        {
            return true; // If the else gets called, we always want to use it.
        }

        public StringBuilder Process(object model)
        {
            return this.Contents.Process(model);
        }
    }
}
