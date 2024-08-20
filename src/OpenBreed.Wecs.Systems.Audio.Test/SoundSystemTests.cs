using Moq;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Audio;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Audio.Events;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Audio.Test
{
    [TestFixture]
    public class SoundSystemTests
    {
        #region Private Fields

        private Mock<IUpdateContext> mockContext;
        private Mock<IEntity> mockEntity;
        private Mock<IEventsMan> mockEventsMan;
        private MockRepository mockRepository;

        private Mock<ISoundMan> mockSoundMan;
        private Mock<IWorld> mockWorld;

        #endregion Private Fields

        #region Public Methods

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockSoundMan = this.mockRepository.Create<ISoundMan>(MockBehavior.Loose);
            this.mockEventsMan = this.mockRepository.Create<IEventsMan>(MockBehavior.Loose);
            this.mockWorld = this.mockRepository.Create<IWorld>();
            this.mockEntity = this.mockRepository.Create<IEntity>();
            this.mockContext = this.mockRepository.Create<IUpdateContext>();
        }

        [Test]
        public void Update_NoSoundPlayerComponent_NoPlaySample()
        {
            // Arrange
            var soundSystem = this.CreateSoundSystem();
            SetupMockEntity(mockEntity, null);
            SetupWorldContext(mockContext, paused: false);

            // Act
            soundSystem.Update(mockContext.Object);

            // Assert
            mockSoundMan.Verify(mock => mock.PlaySample(It.IsAny<int>()), Times.Never());
        }

        [Test]
        public void Update_ToPlayIsEmpty_NoPlaySample()
        {
            // Arrange
            var soundSystem = this.CreateSoundSystem();
            SetupMockEntity(mockEntity, new SoundPlayerComponent());
            SetupWorldContext(mockContext, paused: false);

            // Act
            soundSystem.Update(mockContext.Object);

            // Assert
            mockSoundMan.Verify(mock => mock.PlaySample(It.IsAny<int>()), Times.Never());
            mockEventsMan.Verify(mock => mock.Raise(It.IsAny<SoundPlayEvent>()), Times.Never());
        }
        [TestCase(new object[] {new int[] { 1 } })]
        [TestCase(new object[] { new int[]{ 2, 5, 6, 7 } })]
        public void Update_ToPlayHasIds_PlaySamples(int[] sampleIds)
        {
            // Arrange
            var soundSystem = this.CreateSoundSystem();
            var component = new SoundPlayerComponent();
            component.ToPlay.AddRange(sampleIds);
            SetupMockEntity(mockEntity, component);
            SetupWorldContext(mockContext, paused: false);

            // Act
            soundSystem.Update(mockContext.Object);

            // Assert
            foreach (var sampleId in sampleIds)
                mockSoundMan.Verify(mock => mock.PlaySample(sampleId), Times.Once());

            mockEventsMan.Verify(mock => mock.Raise(It.IsAny<SoundPlayEvent>()), Times.Exactly(sampleIds.Length));
        }

        #endregion Public Methods

        #region Private Methods

        private static void SetupMockEntity(Mock<IEntity> mock, SoundPlayerComponent soundPlayerComponent)
        {
            mock.Setup(item => item.TryGet<SoundPlayerComponent>()).Returns(soundPlayerComponent);
            mock.Setup(item => item.Id).Returns(0);
        }

        private static void SetupWorldContext(Mock<IUpdateContext> mock, bool paused)
        {
            mock.Setup(item => item.Paused).Returns(paused);
        }

        private SoundSystem CreateSoundSystem()
        {
            return new SoundSystem(
                this.mockSoundMan.Object,
                this.mockEventsMan.Object);
        }

        #endregion Private Methods
    }
}