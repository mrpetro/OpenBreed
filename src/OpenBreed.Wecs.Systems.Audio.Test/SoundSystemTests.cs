using Moq;
using NUnit.Framework;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Audio;
using OpenBreed.Wecs.Systems.Audio;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Audio.Test
{
    [TestFixture]
    public class SoundSystemTests
    {
        private MockRepository mockRepository;

        private Mock<ISoundMan> mockSoundMan;
        private Mock<IEventsMan> mockEventsMan;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockSoundMan = this.mockRepository.Create<ISoundMan>();
            this.mockEventsMan = this.mockRepository.Create<IEventsMan>();
        }

        private SoundSystem CreateSoundSystem()
        {
            return new SoundSystem(null,
                this.mockSoundMan.Object,
                this.mockEventsMan.Object);
        }

        [Test]
        public void TestMethod1()
        {
            // Arrange
            var soundSystem = this.CreateSoundSystem();

            //soundSystem.
            // Act


            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
