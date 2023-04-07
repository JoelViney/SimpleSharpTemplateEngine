using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class IfModel
    {
        public bool MyProperty1 { get; set; }
        public bool MyProperty2 { get; set; }
    }

    [TestClass]
    public class IfTests
    {
        [DataTestMethod]
        [DataRow("{{if:MyProperty1}}Hello World.{{endif}}", "Hello World.")]
        [DataRow("{{ if:MyProperty1 }}Hello World.{{ end if }}", "Hello World.")]
        [DataRow("{{ if: my-property1 }}Hello World.{{ end if }}", "Hello World.")]
        [DataRow("{{ IF: MyProperty1 }}Hello World.{{ ENDIF }}", "Hello World.")]
        [DataRow(">>>{{ if:MyProperty1 }}Hello World.{{ end if }}<<<", ">>>Hello World.<<<")]
        public void IfTrue(string text, string expected)
        {
            // Arrange
            var model = new IfModel() { MyProperty1 = true };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IfFalse()
        {
            // Arrange
            var text = "{{ if: MyProperty1 }}This text is inside the If{{ end if }}";
            var model = new IfModel() { MyProperty1 = false };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void NestedIf()
        {
            // Arrange
            var text = ".{{IF:MyProperty1}}.{{IF:MyProperty2}}|Test|{{ENDIF}}.{{ENDIF}}.";
            var model = new IfModel() { MyProperty1 = true, MyProperty2 = true };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("..|Test|..", result);
        }

        [TestMethod]
        public void NestedIfInnerFalse()
        {
            // Arrange
            var text = ".{{IF:MyProperty1}}.{{IF:MyProperty2}}|Test|{{ENDIF}}.{{ENDIF}}.";
            var model = new IfModel() { MyProperty1 = true, MyProperty2 = false };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("....", result);
        }
    }
}
