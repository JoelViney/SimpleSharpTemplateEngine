using SimpleSharpTemplateEngine.Models;
using System.Text;

namespace SimpleSharpTemplateEngine
{
    /// <summary>
    /// This is a templating engine. See the unit tests for how it works.
    /// </summary>
    public static class TemplateEngine
    {
        public static char CommandDelimiterStart = '#'; // It uses two of these to work out where commands start and end.
        public static char CommandDelimiterEnd = '#';

        private enum State
        {
            None,
            Loop,
            If,
            IfNot
        }

        public static string Execute(string text, object model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var output = ExecuteInternal(text, model);

            return output.ToString();
        }

        private static StringBuilder ExecuteInternal(string text, object model)
        {

            var i = 0;
            var dom = BuildObjectModel(State.None, text, ref i);

            var builder = dom.Process(model);

            return builder;
        }

        private static ContainerObject BuildObjectModel(State state, string text, ref int i)
        {
            var result = new ContainerObject();

            string command = "";
            bool parseCommand = false;
            for (; i < text.Length; i++)
            {
                char ch = text[i];

                if (
                    (!parseCommand && (ch != CommandDelimiterStart || i == (text.Length - 1) || text[i + 1] != CommandDelimiterStart))
                    ||
                    (parseCommand && (ch != CommandDelimiterEnd || i == (text.Length - 1) || text[i + 1] != CommandDelimiterEnd))
                    )
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
                    var contents = TemplateEngine.BuildObjectModel(State.If, text, ref i);
                    var obj = new IfObject(command, contents);

                    result.Items.Add(obj);
                }
                else if(command.StartsWith("IFNOT:"))
                {
                    command = command.Substring("IFNOT:".Length);

                    i++;
                    var contents = TemplateEngine.BuildObjectModel(State.IfNot, text, ref i);
                    var obj = new IfNotObject(command, contents);

                    result.Items.Add(obj);
                }
                else if (command == "ENDIF")
                {
                    if (state != State.If && state != State.IfNot)
                    {
                        throw new Exception($"Unexpected ENDIF at character {i}");
                    }

                    return result;
                }
                else if (command.StartsWith("STARTLOOP:"))
                {
                    command = command.Substring("STARTLOOP:".Length);

                    i++;
                    var contents = TemplateEngine.BuildObjectModel(State.Loop, text, ref i);
                    var obj = new LoopObject(command, contents);

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
                    var property = new PropertyObject(command);
                    result.Items.Add(property);
                }

                parseCommand = false;
                command = "";
            }

            return result;
        }
    }
}
