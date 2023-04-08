# Simple Sharp Template Engine
 
This is a simple C# templating engine where you pass in an object and it will build a document for you. I shouldn't have written it, but it was fun.

To use it you pass in a template and an object with properties and the output is built from the model's values.

## Example


```
var text = "Hello {{myname}}.";
var model = new TestModel() 
{ 
    MyName = "World" 
};

var result = TemplateEngine.Execute(text, model);

// The result will be 'Hello World'
```


## Syntax

###### Property Assignment:
```
{{ myProperty }}
```

###### If Statements
```
{{ if: myProperty }}
{{ end if }}
```

###### If Not Statements
```
{{ if not: myProperty }}
{{ end if }}
```

###### Loops
```
{{ loop:myList }}
{{ end }}
```

###### Switch
```
{{ switch: myProperty }}
{{ case: 1 }}
{{ end case }}
{{{ end switch }}
```


## Other Features

###### Kebab Case
```
{{ if: my-property }}
{{ end if }}
```

###### Case Insensitive
```
{{ IF: my-property }}
{{ END IF }}
```

###### Flexable spacing
```
{{IF:my-property}}
{{END IF}}
```
