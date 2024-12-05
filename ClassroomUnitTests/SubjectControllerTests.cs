using Classroom.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace ClassroomTests
{
    public class SubjectControllerTests
    {
        private Mock<ILogger<SubjectController>> _loggerMock;
        private SubjectController _subjectController;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<SubjectController>>();
            _subjectController = new SubjectController(_loggerMock.Object);
        }

        [Test]
        public void Test1()
        {
            var expected = new[]
                { "Nyelvtan", "Irodalom", "Matematika", "Környezetismeret", "Angol", "Ének", "Rajz", "Testnevelés" };

            var result = _subjectController.GetAll();
            
            
            Assert.That(result, Is.Not.Null, "Result should not be null.");
            Assert.That(result, Is.InstanceOf<OkObjectResult>(), "Result should be of type OkObjectResult.");

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null, "Result cast to OkObjectResult should not be null.");
            
            var actual = okResult.Value as IEnumerable<string>;
            Assert.That(actual, Is.Not.Null, "OkObjectResult value should not be null.");
            Assert.That(actual, Is.EquivalentTo(expected), "Result array elements should match the expected elements.");
            
            Assert.That(actual, Is.EquivalentTo(expected));
            
            Assert.That(actual.First(), Is.EqualTo("Nyelvtan"));
            
            Assert.That(actual, Is.Not.Empty);
            
        }
    }
}