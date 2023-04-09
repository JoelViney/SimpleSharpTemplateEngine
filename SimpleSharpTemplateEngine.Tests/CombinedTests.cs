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
        public string MyName { get; set; }
    }

    [TestClass]
    public class CombinedTests
    {
        [TestMethod]
        public void IfThenLoop()
        {
            // Arrange
            var text = ".{{ if :MyProperty1 }}.{{ loop: mylist }}|{{ myname }}|{{ end loop }}.{{ end if }}.";
            var model = new CombinedModel
            {
                MyProperty1 = true,
                MyList = new List<CombinedChildModel>(new[] { new CombinedChildModel() { MyName = "One" }, new CombinedChildModel() { MyName = "Two" } })
            };

            // Act
            var result = TemplateEngine.Execute(text, model);

            // Assert
            Assert.AreEqual("..|One||Two|..", result);
        }
    }
}
