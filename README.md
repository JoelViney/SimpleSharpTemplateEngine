# Simple Sharp Template Engine
 
This is a simple C# templating engine where you pass in an object and it will build a document for you. I shouldn't have written it, but it was fun.

To use it you pass in a template and an object with properties and the output is built from the model's values.

## Example


```
var text = "Hello {{ MyProperty }}.";
var model = new TestModel() 
{ 
    MyProperty = "World" 
};

var result = TemplateEngine.Execute(text, model);

// The result will be 'Hello World.'
```


## Syntax

###### Property Assignment:
```
{{ MyProperty }}
```

###### If Statements
```
{{ if: MyProperty1 }}
{{ else if: MyProperty2 }}
{{ else }}
{{ end if }}
```

###### If Not Statements
```
{{ if not: MyProperty }}
{{ end if }}
```

###### Loops
```
{{ loop: MyList }}
{{ end loop }}
```

###### Switch
```
{{ switch: MyProperty }}
{{ case: 1 }}
{{ end case }}
{{{ end switch }}
```


## Other Features


###### Nesting
```
{{ if: MyPoperty }}
{{ loop: MyList }}
{{ end loop }}
{{ end if }}
```

###### Kebab Case
```
{{ if: my-property }}
{{ end if }}
```

###### Case Insensitive
```
{{ IF: myproperty }}
{{ END IF }}
```

###### Not strict on spacing
```
{{IF:MyProperty}}
{{ENDIF}}
```

## How it works?

The engine builds up an object model representing the template. e.g.

```
{{ if: MyProperty }}
Hello World
{{ end if }}
```

would build an object model that looks like

```
ContainerObject
    .Items[0] =
        IfObject
            .Property = "MyProperty"
            .ContainerObject = 
                TextObject
                    .Text = "Hello World"               
    ]
```

once the object model is built it is processed with the process command in an object oriented approach.

```
var stringBuilder = templateObjectModel.Process(model);
```
