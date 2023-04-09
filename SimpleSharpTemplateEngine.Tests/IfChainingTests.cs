using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class IfChaningModel
    {
        public bool MyProperty1 { get; set; }
        public bool MyProperty2 { get; set; }
    }

    [TestClass]
    public class IfChainingTests
    {
        [TestMethod]
        public void IfChaining()
        {
            // Arrange
            var text = "{{ if: MyProperty1 }}Text inside the if{{ else }}Text inside the else{{ end if }}";
            var model = new IfChaningModel() { MyProperty1 = true };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Text inside the if", result);
        }

        [TestMethod]
        public void ElseChaining()
        {
            // Arrange
            var text = "{{ if: MyProperty1 }}Text inside the If{{ else }}Text inside the else{{ end if }}";
            var model = new IfChaningModel() { MyProperty1 = false };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Text inside the else", result);
        }

        [TestMethod]
        public void IfElseChaining()
        {
            // Arrange
            var text = "{{ if: MyProperty1 }}Text inside the if{{ else if: MyProperty2 }}Text inside the second if{{ end if }}";
            var model = new IfChaningModel() { MyProperty1 = false, MyProperty2 = true };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Text inside the second if", result);
        }
    }
}
