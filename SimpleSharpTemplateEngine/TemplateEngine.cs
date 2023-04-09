using SimpleSharpTemplateEngine.Models;
using System.Diagnostics;
using System.Text;

namespace SimpleSharpTemplateEngine
{
    /// <summary>
    /// This is a templating engine. See the unit tests for how it works.
    /// </summary>
    public static class TemplateEngine
    {
        private static readonly char CommandDelimiterStart = '{'; // It uses two of these to work out where commands start and end.
        private static readonly char CommandDelimiterEnd = '}';

        private enum State
        {
            None,
            Loop,
            If,
            IfNot,
            ElseIf,
            Else,
            Switch,
            SwitchCase
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="TemplateEngineException">If the template is invalid because of miss-matched start and end tags this excecption will be thrown.</exception>
        public static string Execute(string template, object model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var output = ExecuteInternal(template, model);

            return output.ToString();
        }


        private static StringBuilder ExecuteInternal(string template, object model)
        {
            // Convert the template string into an object model containing commands and text.
            var i = 0;

            var templateObjectModel = BuildObjectModel(State.None, template, ref i);

            if (Debugger.IsAttached)
            {
                DebugObjectModel(templateObjectModel);
            }
            // Process the object model into text.
            var stringBuilder = templateObjectModel.Process(model);

            return stringBuilder;
        }

        private static void DebugObjectModel(ContainerObject model, int indent = 0)
        {
            if (indent == 0)
            {
                Debug.WriteLine($"========================================");
            }

            foreach (var item in model.Items)
            {
                if (item is IfChainObject chain)
                {
                    Debug.WriteLine($"{new String(' ', (indent * 2))}{item.GetType().Name}");
                    foreach (var chainItem in chain.Items)
                    {
                        DebugObjectModel(chainItem.Contents, indent + 1);
                    }
                }
                else
                {
                    Debug.WriteLine($"{new String(' ', (indent * 2))}{item.GetType().Name}");
                }
            }

            if (indent == 0)
            {
                Debug.WriteLine($"========================================");
            }
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
                        if (result.Items.Count == 0 || result.Items.LastOrDefault() is not TextObject txtObject)
                        {
                            txtObject = new TextObject();
                            result.Items.Add(txtObject);
                        }

                        txtObject.Text.Append(ch); // Build output
                    }
                    continue;
                }

                // Found a Command {{
                i++; // Found the Start... skip processing the next squiggly bracket

                if (!parseCommand)
                {
                    parseCommand = true;
                    continue;
                }

                command = CleanCommandText(command);
                var commandLowerCase = command.ToLower();
                
                // pull out the args.
                string? args = null;
                if (command.Contains(':'))
                {
                    var pos = command.IndexOf(':');
                    args = command.Substring(pos + 1);
                    args = CleanPropertyName(args);
                }

                if (commandLowerCase.StartsWith("if:"))
                {
                    if (args == null)
                    {
                        throw new TemplateEngineException($"No arguments provided to the 'if' at character {i}");
                    }

                    i++;
                    // This will return the contents of the if statement and all chaining if, else if and elses
                    var contents = TemplateEngine.BuildObjectModel(State.If, text, ref i);

                    var obj = new IfChainObject();
                    var ifObject = new IfObject(args);
                    obj.Items.Add(ifObject);

                    // Keep adding it until we hit an if chaining object.
                    foreach (var item in contents.Items)
                    {
                        if (item is IIfStatementChainingObject && item is IIfStatementObject childObject)
                        {
                            obj.Items.Add(childObject);
                        }
                        else
                        {
                            ifObject.Contents.Items.Add(item);
                        }
                    }

                    result.Items.Add(obj);
                }
                else if(commandLowerCase.StartsWith("ifnot:"))
                {

                    if (args == null)
                    {
                        throw new TemplateEngineException($"No arguments provided to the 'ifnot' at character {i}");
                    }

                    i++;
                    var contents = TemplateEngine.BuildObjectModel(State.IfNot, text, ref i);

                    var obj = new IfChainObject();
                    var ifObject = new IfNotObject(args);
                    obj.Items.Add(ifObject);

                    // Keep adding it until we hit an if chaining object.
                    foreach (var item in contents.Items)
                    {
                        if (item is IIfStatementChainingObject && item is IIfStatementObject childObject)
                        {
                            obj.Items.Add(childObject);
                        }
                        else
                        {
                            ifObject.Contents.Items.Add(item);
                        }
                    }

                    result.Items.Add(obj);
                }
                else if (commandLowerCase.StartsWith("elseif:"))
                {
                    if (state != State.If && state != State.IfNot && state != State.ElseIf)
                    {
                        throw new TemplateEngineException($"Unexpected 'else' at character {i}");
                    }

                    if (args == null)
                    {
                        throw new TemplateEngineException($"No arguments provided to the 'else' at character {i}");
                    }

                    i++;
                    var contents = TemplateEngine.BuildObjectModel(State.ElseIf, text, ref i);
                    var obj = new ElseIfObject(args, contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase.StartsWith("else"))
                {
                    if (state != State.If && state != State.IfNot && state != State.ElseIf)
                        throw new TemplateEngineException($"Unexpected 'else' at character {i}");

                    i++;
                    var contents = TemplateEngine.BuildObjectModel(State.Else, text, ref i);
                    var obj = new ElseObject(contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase == "endif")
                {
                    if (state != State.If && state != State.IfNot && state != State.ElseIf && state != State.Else)
                        throw new TemplateEngineException($"Unexpected 'endif' at character {i}");

                    return result;
                }
                else if (commandLowerCase.StartsWith("switch:"))
                {
                    if (args == null)
                        throw new TemplateEngineException($"No arguments provided to the 'switch' at character {i}");
                    

                    i++;

                    TemplateEngine.SkipNewLine(text, ref i);

                    var contents = TemplateEngine.BuildObjectModel(State.Switch, text, ref i);

                    var obj = new SwitchObject(args, contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase == "endswitch")
                {
                    if (state != State.Switch)
                    {
                        throw new TemplateEngineException($"Unexpected 'endswitch' at character {i}");
                    }

                    return result;
                }
                else if (commandLowerCase.StartsWith("case:"))
                {
                    if (args == null)
                        throw new TemplateEngineException($"No arguments provided to the 'case' at character {i}");

                    i++;
                    TemplateEngine.SkipNewLine(text, ref i);
                    var contents = TemplateEngine.BuildObjectModel(State.SwitchCase, text, ref i);
                    var obj = new SwitchCaseObject(args, contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase == "endcase")
                {
                    if (state != State.SwitchCase)
                    {
                        throw new TemplateEngineException($"Unexpected 'endcase' at character {i}");
                    }

                    return result;
                }
                else if (commandLowerCase.StartsWith("loop:"))
                {
                    if (args == null)
                        throw new TemplateEngineException($"No arguments provided to the 'loop' at character {i}");

                    i++;
                    var contents = TemplateEngine.BuildObjectModel(State.Loop, text, ref i);
                    var obj = new LoopObject(args, contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase == "endloop")
                {
                    if (state != State.Loop)
                    {
                        throw new TemplateEngineException($"Unexpected 'endloop' at character {i}"); // This shouldn't happen, we have a malformed template.
                    }

                    return result;
                }
                else
                {
                    // Found a Property.
                    var propertyName = CleanPropertyName(command); // Use the original command to save the correct case.
                    var property = new PropertyObject(propertyName);
                    result.Items.Add(property);
                }

                parseCommand = false;
                command = "";
            }

            if (parseCommand)
            {
                // We are currently parsing a command and ran out of text.
                // This means we have an invalid template
                var pos = text.Length - command.Length - "{{".Length;
                throw new TemplateEngineException($"Unexpected end of template, failed to find a closing command bracket for the command at character {pos}.");
            }
            return result;
        }

        private static string CleanCommandText(string command)
        {
            return command.Replace(" ", "").Trim();
        }

        private static string CleanPropertyName(string text)
        {
            return text.Replace("-", "");
        }

        private static void SkipNewLine(string text, ref int i)
        {
            if (i >= text.Length)
            {
                return; // End of template
            }
            if (text[i] == '\r') 
            {
                i++;
            }

            if (text[i] == '\n')
            {
                i++;
            }
        }
    }
}
