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
        [DataRow("{{if MyProperty1}}Hello World.{{end if}}", "Hello World.")]
        [DataRow("{{ if MyProperty1 }}Hello World.{{ end if }}", "Hello World.")]
        [DataRow("{{ if my-property1 }}Hello World.{{ end if }}", "Hello World.")]
        [DataRow(">>>{{ if MyProperty1 }}Hello World.{{ end if }}<<<", ">>>Hello World.<<<")]
        public void IfTrue(string template, string expected)
        {
            // Arrange
            var model = new IfModel() { MyProperty1 = true };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IfFalse()
        {
            // Arrange
            var template = "{{ if MyProperty1 }}Hello World.{{ end if }}";
            var model = new IfModel() { MyProperty1 = false };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("", result);
        }

        [TestMethod]
        public void NestedIf()
        {
            // Arrange
            var template = "{{ if MyProperty1 }}{{ if MyProperty2 }}Hello World.{{ end if }}{{ end if }}";
            var model = new IfModel() { MyProperty1 = true, MyProperty2 = true };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("Hello World.", result);
        }


        [TestMethod]
        public void NestedIfInSitu()
        {
            // Arrange
            var template = "This {{ if MyProperty1 }}is {{ if MyProperty2 }}Hello World {{ end if }}right {{ end if }}here.";
            var model = new IfModel() { MyProperty1 = true, MyProperty2 = true };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("This is Hello World right here.", result);
        }


        [TestMethod]
        public void NestedIfInnerFalse()
        {
            // Arrange
            var template = "{{ if MyProperty1 }}{{ if MyProperty2 }}Hello World.{{ end if }}{{ end if }}";
            var model = new IfModel() { MyProperty1 = true, MyProperty2 = false };

            // Act
            var result = TemplateEngine.Execute(template, model);

            // Assert
            Assert.AreEqual("", result);
        }
    }
}
