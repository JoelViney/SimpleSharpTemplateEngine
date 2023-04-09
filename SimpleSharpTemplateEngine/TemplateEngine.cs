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
        /// Applies the model to the supplied template.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the model is null this exception will be thrown.</exception>
        /// <exception cref="TemplateEngineException">If the template is invalid because of miss-matched start and end tags this excecption will be thrown.</exception>
        public static string Execute(string template, object model)
        {
            if (model == null) 
                throw new ArgumentNullException(nameof(model));

            var output = ExecuteInternal(template, model);

            return output.ToString();
        }


        private static StringBuilder ExecuteInternal(string template, object model)
        {
            // Convert the template string into an object model containing commands and text.
            var i = -1; // We increment in the BuildObjectModel

            var templateObjectModel = BuildObjectModel(State.None, template, ref i);

            if (Debugger.IsAttached)
            {
                DebugObjectModel(templateObjectModel);
            }
            // Process the object model into text.
            var stringBuilder = templateObjectModel.Process(model);

            return stringBuilder;
        }

        // Recursive function to build the template object model.
        private static ContainerObject BuildObjectModel(State state, string text, ref int i)
        {
            var result = new ContainerObject();
            string command = "";
            bool parseCommand = false;

            i++;

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
                        // Build the command
                        command += ch; 
                    }
                    else
                    {
                        // Build a text object
                        if (result.Items.Count == 0 || result.Items.LastOrDefault() is not TextContainer txtObject)
                        {
                            txtObject = new TextContainer();
                            result.Items.Add(txtObject);
                        }

                        txtObject.Text.Append(ch); // Build output
                    }

                    continue; // Continue until we find a command or we are done with the template.
                }

                // Found a Command {{
                i++; // Found the Start... skip processing the next squiggly bracket

                if (!parseCommand)
                {
                    parseCommand = true;
                    continue;
                }

                command = command.Replace(" ", "").Trim();
                var commandLowerCase = command.ToLower();
                
                // Pull out the args.
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
                        throw new TemplateEngineException($"No arguments provided to the 'if' at character {i}");

                    // This will return the contents of the if statement and all chaining if, else if and elses
                    var contents = BuildObjectModel(State.If, text, ref i);

                    var obj = new IfThenElseBlock();
                    var ifObject = new IfStatement(args);
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
                        throw new TemplateEngineException($"No arguments provided to the 'ifnot' at character {i}");
                    
                    var contents = BuildObjectModel(State.IfNot, text, ref i);

                    var obj = new IfThenElseBlock();
                    var ifObject = new IfNotStatement(args);
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
                        throw new TemplateEngineException($"Unexpected 'else' at character {i}");
                    
                    if (args == null) 
                        throw new TemplateEngineException($"No arguments provided to the 'else' at character {i}");

                    var contents = BuildObjectModel(State.ElseIf, text, ref i);

                    var obj = new ElseIfStatement(args, contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase.StartsWith("else"))
                {
                    if (state != State.If && state != State.IfNot && state != State.ElseIf) 
                        throw new TemplateEngineException($"Unexpected 'else' at character {i}");

                    var contents = BuildObjectModel(State.Else, text, ref i);

                    var obj = new ElseStatement(contents);

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
                    
                    var contents = BuildObjectModel(State.Switch, text, ref i);

                    var obj = new SwitchStatement(args, contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase == "endswitch")
                {
                    if (state != State.Switch) 
                        throw new TemplateEngineException($"Unexpected 'endswitch' at character {i}");

                    return result;
                }
                else if (commandLowerCase.StartsWith("case:"))
                {
                    if (args == null) 
                        throw new TemplateEngineException($"No arguments provided to the 'case' at character {i}");

                    var contents = BuildObjectModel(State.SwitchCase, text, ref i);

                    var obj = new SwitchCaseStatement(args, contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase == "endcase")
                {
                    if (state != State.SwitchCase) 
                        throw new TemplateEngineException($"Unexpected 'endcase' at character {i}");

                    return result;
                }
                else if (commandLowerCase.StartsWith("loop:"))
                {
                    if (args == null) 
                        throw new TemplateEngineException($"No arguments provided to the 'loop' at character {i}");
                    
                    var contents = BuildObjectModel(State.Loop, text, ref i);

                    var obj = new LoopStatement(args, contents);

                    result.Items.Add(obj);
                }
                else if (commandLowerCase == "endloop")
                {
                    if (state != State.Loop) 
                        throw new TemplateEngineException($"Unexpected 'endloop' at character {i}"); // This shouldn't happen, we have a malformed template.

                    return result;
                }
                else
                {
                    // Found a Property.
                    var propertyName = CleanPropertyName(command); // Use the original command to save the correct case.
                    var property = new PropertyContainer(propertyName);
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


        private static string CleanPropertyName(string text)
        {
            return text.Replace("-", "");
        }


        private static void DebugObjectModel(ContainerObject model, int indent = 0)
        {
            if (indent == 0)
            {
                Debug.WriteLine($"========================================");
            }

            foreach (var item in model.Items)
            {
                if (item is IfThenElseBlock chain)
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
    }
}
