using NUnit.Framework;

namespace Quilt4Net.Tests
{
    [TestFixture]
    public class Getting_Session_key
    {
        [Test]
        public void When_two_threads_are_getting_session_key_at_the_same_tine()
        {
            //Arrange

            //Act
            
            //Assert
            //TODO: All threads should get the same session key without any exception thrown
            Assert.Fail("Implement.");
        }
    }
}