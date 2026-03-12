using Moq;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Services;
using Xunit;

namespace OpenBreed.Wecs.Test.Systems
{
    public class DefaultSystemRequirementsProviderTests
    {
        public class AComponent : IEntityComponent { }
        public class BComponent : IEntityComponent { }
        public class CComponent : IEntityComponent { }
        public class DComponent : IEntityComponent { }

        private MockRepository mockRepository;
        private Mock<ITypeAttributesProvider> mockTypeAttributesProvider;
        private Mock<IEntityClassMan> mockEntityClassMan;

        public DefaultSystemRequirementsProviderTests
()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockTypeAttributesProvider = mockRepository.Create<ITypeAttributesProvider>();
            this.mockEntityClassMan = mockRepository.Create<IEntityClassMan>();
        }

        private void SetupForbiddenComponentTypes(Mock<ITypeAttributesProvider> mock, Type inputType, params Type[] types )
        {
            mock.Setup(item => item.GetAttributes(inputType)).Returns(new object[] { new RequireEntityWithoutAttribute(types) });
        }

        private void SetupAllowedComponentTypes(Mock<ITypeAttributesProvider> mock, Type inputType, params Type[] types)
        {
            mock.Setup(item => item.GetAttributes(inputType)).Returns(new object[] { new RequireEntityWithAttribute(types) });
        }

        private DefaultSystemRequirementsProvider CreateProvider()
        {
            return new DefaultSystemRequirementsProvider(mockTypeAttributesProvider.Object, mockEntityClassMan.Object);
        }

        [Fact]
        public void RegisterRequirements_NullArgument_ArgumentNullException()
        {
            // Arrange
            var provider = this.CreateProvider();
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => provider.RegisterRequirements(systemType: null));
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void RegisterRequirements_NonISystemType_ArgumentException()
        {
            // Arrange
            var provider = this.CreateProvider();
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => provider.RegisterRequirements(typeof(string)));
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void RegisterRequirements_ISystemType_DoesNotThrow()
        {
            // Arrange
            var provider = this.CreateProvider();
            var mockSystem = mockRepository.Create<ISystem>();
            var systemType = mockSystem.Object.GetType();
            mockTypeAttributesProvider.Setup(item => item.GetAttributes(systemType)).Returns(Array.Empty<object>);
            // Act
            // Assert
            var exception = Record.Exception(() => provider.RegisterRequirements(systemType));
            Assert.Null(exception);

            this.mockRepository.VerifyAll();
        }
    }
}
