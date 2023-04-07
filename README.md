# SimpleSharpTemplateEngine
 
This is a simple C# templating engine where you pass in an object and it will build a document for you. I shouldn't have written it, but it was fun.

To use it you pass in a template and an object with properties and the output is built from the model's values.

```
var text = "Hello ##name##.";
var model = new TestModel() 
{ 
    Name = "World" 
};

var result = TemplateEngine.Execute(text, model);

// The result will be 'Hello World'
```

## Features
Property Assignment:

```
##myProperty##
```

If Statements
```
##IF:myProperty## 
##ENDIF##
```

If Not Statements
```
##IFNOT:myProperty## 
##ENDIF##
```

Loops
```
##STARTLOOP:myList##
##ENDLOOP##
```
