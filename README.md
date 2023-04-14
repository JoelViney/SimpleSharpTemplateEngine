# Simple Sharp Template Engine
 
This is a simple C# templating engine where you pass in an object and it will build a document for you. I shouldn't have written it, but it was fun.

To use it you pass in a template and a strongly typed object with properties and the output is built from the model's values.

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


###### Chaining Property Assignment:
```
{{ MyChild.MyChildProperty }}
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
{{if:MyProperty}}
{{endif}}
```

## TODO

1. Add a way to reference the root object or the parent object:
    {{ Root.MyProperty }}
    {{ Parent.MyProperty }}

1. Add Format?
    {{ MyProperty: YYYY-MM-DD }}

1. Add Conditionals
    {{ if: x == 1 }}