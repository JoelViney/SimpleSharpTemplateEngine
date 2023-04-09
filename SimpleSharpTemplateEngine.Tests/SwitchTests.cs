using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleSharpTemplateEngine
{
    public class SwitchModel
    {
        public string MyName { get; set; }
    }

    [TestClass]
    public class SwitchTests
    {
        [TestMethod]
        public void SimpleSwitch()
        {
            // Arrange
            var text = @"{{ switch: MyName }}{{ case:A }}Aye{{ end case }}{{ end switch }}";
            var model = new SwitchModel() { MyName = "A" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Aye", result);
        }


        [TestMethod]
        public void Switch()
        {
            // Arrange
            var text = @"{{ switch: MyName }}{{ case: A }}Aye{{ end case }}{{ case: B }}Bee{{ end case }}{{ end switch }}";
            var model = new SwitchModel() { MyName = "A" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Aye", result);
        }


        [TestMethod]
        public void SwitchSecondOption()
        {
            // Arrange
            var text = @"{{ switch: MyName }}{{ case: A }}Aye{{ end case }}{{ case: B }}Bee{{ end case }}{{ end switch }}";
            var model = new SwitchModel() { MyName = "B" };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("Bee", result);
        }
    }
}
