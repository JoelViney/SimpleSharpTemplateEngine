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
        [TestMethod]
        public void IfTrue()
        {
            // Arrange
            var text = ".##IF:MyProperty1##abc##ENDIF##.";
            var model = new IfModel() { MyProperty1 = true };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual(".abc.", result);
        }

        [TestMethod]
        public void IfFalse()
        {
            // Arrange
            var text = ".##IF:MyProperty1##This text is inside the If##ENDIF##.";
            var model = new IfModel() { MyProperty1 = false };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("..", result);
        }

        [TestMethod]
        public void NestedIf()
        {
            // Arrange
            var text = ".##IF:MyProperty1##.##IF:MyProperty2##|Test|##ENDIF##.##ENDIF##.";
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
            var text = ".##IF:MyProperty1##.##IF:MyProperty2##|Test|##ENDIF##.##ENDIF##.";
            var model = new IfModel() { MyProperty1 = true, MyProperty2 = false };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("....", result);
        }
    }
}
