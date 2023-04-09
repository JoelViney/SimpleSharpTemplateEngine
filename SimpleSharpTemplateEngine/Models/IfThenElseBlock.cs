using System.Collections.Generic;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal interface IIfStatementObject
    {
        bool MatchesExpression(object model);
        StringBuilder Process(object model);
        ContainerObject Contents { get; }
    }

    internal interface IIfStatementChainingObject
    {
        // This is used to identify Else and ElseIf
    }

    /// <summary>
    /// Used to store a single logical if statement that can also contain else if and else statements.
    /// </summary>
    internal class IfThenElseBlock : ITemplateObject
    {
        public List<IIfStatementObject> Items { get; set; }

        public IfThenElseBlock()
        {
            this.Items = new List<IIfStatementObject>();
        }

        public StringBuilder Process(object model)
        {
            foreach (var statement in this.Items)
            {
                if (statement.MatchesExpression(model))
                {
                    return statement.Process(model);
                }
            }

            return new StringBuilder();
        }
    }
}
