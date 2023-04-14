using SimpleSharpTemplateEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleSharpTemplateEngine.Models
{
    internal class SwitchStatement : ITemplateObject
    {
        public string PropertyName { get; }
        public List<ISwitchCaseObject> Cases { get; set; }

        public SwitchStatement(string propertyName, ContainerObject container)
        {
            this.PropertyName = propertyName;

            this.Cases = new List<ISwitchCaseObject>();
            foreach (var item in container.Items) 
            {
                if (!(item is SwitchCaseStatement switchCase))
                {
                    throw new TemplateEngineException("Invalid item in case statement.");
                }
                this.Cases.Add(switchCase);
            }
        }

        public StringBuilder Process(object model)
        {
            var property = PropertyHelper.GetReferencedProperty(model, this.PropertyName);

            foreach(var switchCase in this.Cases)
            { 
                if (switchCase.MatchExpression(property))
                {
                    return switchCase.Process(model);
                }
            }

            return new StringBuilder();
        }
    }
}
