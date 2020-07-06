using SimpleSharpTemplateEngine.Models;
using System;
using System.Linq;
using System.Text;

namespace SimpleSharpTemplateEngine
{
    /// <summary>
    /// This is a templating engine. See the unit tests for how it works.
    /// </summary>
    /// <remarks>
    /// I should never have written this, I should have used a 3rd party, but it was fun to write.
    /// </remarks>
    public class TemplateEngine
    {
        private enum State
        {
            None,
            Loop,
            If,
        }

        public string Execute(string text, object model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var output = ExecuteInternal(text, model);

            return output.ToString();
        }

        private StringBuilder ExecuteInternal(string text, object model)
        {

            var i = 0;
            var dom = BuildObjectModel(State.None, text, ref i);

            var builder = dom.Process(model);

            return builder;
        }

        private ContainerObject BuildObjectModel(State state, string text, ref int i)
        {
            var result = new ContainerObject();

            string command = "";
            bool parseCommand = false;
            for (; i < text.Length; i++)
            {
                char ch = text[i];

                if (ch != '#' || i == (text.Length - 1) || text[i + 1] != '#')
                {
                    // Process Text / Command
                    if (parseCommand)
                    {
                        command += ch; // Build the command name
                    }
                    else
                    {
                        if (result.Items.Count == 0 || !(result.Items.LastOrDefault() is TextObject txtObject))
                        {
                            txtObject = new TextObject();
                            result.Items.Add(txtObject);
                        }

                        txtObject.Text.Append(ch); // Build output
                    }
                    continue;
                }

                // Found a Command (##)
                i++; // Found the Start... skip processing the next #

                if (!parseCommand)
                {
                    parseCommand = true;
                    continue;
                }


                if (command.StartsWith("IF:"))
                {
                    command = command.Substring("IF:".Length);

                    i++;
                    var obj = new IfObject()
                    {
                        PropertyName = command,
                        Contents = this.BuildObjectModel(State.If, text, ref i)
                    };

                    result.Items.Add(obj);
                }
                else if (command == "ENDIF")
                {
                    if (state != State.If)
                        throw new Exception($"Unexpected IF at character {i}"); // This shouldn't happen, we have a malformed template.

                    return result;
                }
                else if (command.StartsWith("STARTLOOP:"))
                {
                    command = command.Substring("STARTLOOP:".Length);

                    i++;
                    var obj = new LoopObject()
                    {
                        PropertyName = command,
                        Contents = this.BuildObjectModel(State.Loop, text, ref i)
                    };

                    result.Items.Add(obj);
                }
                else if (command == "ENDLOOP")
                {
                    if (state != State.Loop)
                        throw new Exception($"Unexpected STARTLOOP at character {i}"); // This shouldn't happen, we have a malformed template.

                    return result;
                }
                else
                {
                    // Found a Property.
                    var property = new PropertyObject() { PropertyName = command };
                    result.Items.Add(property);
                }

                parseCommand = false;
                command = "";
            }

            return result;
        }
    }
}
