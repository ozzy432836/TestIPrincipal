using System.Security.Claims;
using Moq;
using NUnit.Framework;
using TestIPrincipal.Controllers;
using ClaimTypes = TestIPrincipal.Controllers.ClaimTypes;

namespace TestIPrincipal.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {

        public class AdminHasAccessToResourceTests : HomeControllerTest
        {
            private HomeController _controller;
            private Mock<ClaimsPrincipal> _mockUser;
            private ClaimsPrincipal _user;
            private readonly string _adminsResourceId = "3.14";
            private string _aDifferentAdminsResourceId = "3.141";
            private bool _verdict;

            [SetUp]
            public void SetUp()
            {
                _controller = new HomeController();

                _mockUser = new Mock<ClaimsPrincipal>();

                _mockUser
                    .Setup(m => m.HasClaim(ClaimTypes.Resource, _adminsResourceId))
                    .Returns(true);

                _user = _mockUser.Object;
                _controller.User = _user;
            }

            [Test]
            public void AdminCanAccessResourcesForTheirOwnResourceId()
            {
                _verdict = _controller.AdminHasAccessToResource(_adminsResourceId);
                Assert.AreEqual(true, _verdict);
            }

            [Test]
            public void AdminCannotAccessResourcesForSomeoneElsesResouceId()
            {
                _verdict = _controller.AdminHasAccessToResource(_aDifferentAdminsResourceId);
                Assert.AreEqual(false, _verdict);
            }

            [TestCase("3.14")]
            [TestCase("3.141")]
            [TestCase("3.1415")]
            [TestCase("3.14159")]
            [TestCase("3.141593")]
            public void SuperAdminCanAccessResoucesForAnyResouceId(string resourceId)
            {
                _mockUser
                    .Setup(m => m.HasClaim(ClaimTypes.SuperAdmin, "1"))
                    .Returns(true);

                _verdict = _controller.AdminHasAccessToResource(resourceId);
                Assert.AreEqual(true, _verdict);
            }
        }
    }
}
