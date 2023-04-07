using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SimpleSharpTemplateEngine
{
    public class CombinedModel
    {
        public bool MyProperty1 { get; set; }
        public List<CombinedChildModel> MyList { get; set; }
    }

    public class CombinedChildModel
    {
        public string Name { get; set; }
    }

    [TestClass]
    public class CombinedTests
    {
        [TestMethod]
        public void IfThenLoop()
        {
            // Arrange
            var text = ".##IF:MyProperty1##.##STARTLOOP:mylist##|##name##|##ENDLOOP##.##ENDIF##.";
            var model = new CombinedModel
            {
                MyProperty1 = true,
                MyList = new List<CombinedChildModel>(new[] { new CombinedChildModel() { Name = "One" }, new CombinedChildModel() { Name = "Two" } })
            };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("..|One||Two|..", result);
        }
    }
}
