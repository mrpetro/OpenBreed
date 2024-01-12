using Moq;
using NUnit.Framework;
using OpenBreed.Common.Interface;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Systems;
using System;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace OpenBreed.Wecs.Test.Systems
{
    [TestFixture]
    public class DefaultSystemRequirementsProviderTests
    {
        public class AComponent : IEntityComponent { }
        public class BComponent : IEntityComponent { }
        public class CComponent : IEntityComponent { }
        public class DComponent : IEntityComponent { }

        private MockRepository mockRepository;
        private Mock<ITypeAttributesProvider> mockTypeAttributesProvider;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockTypeAttributesProvider = mockRepository.Create<ITypeAttributesProvider>();
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
            return new DefaultSystemRequirementsProvider(mockTypeAttributesProvider.Object);
        }

        [Test]
        public void RegisterRequirements_NullArgument_ArgumentNullException()
        {
            // Arrange
            var provider = this.CreateProvider();
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => provider.RegisterRequirements(systemType: null));
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void RegisterRequirements_NonISystemType_ArgumentException()
        {
            // Arrange
            var provider = this.CreateProvider();
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => provider.RegisterRequirements(typeof(string)));
            this.mockRepository.VerifyAll();
        }

        [Test]
        public void RegisterRequirements_ISystemType_DoesNotThrow()
        {
            // Arrange
            var provider = this.CreateProvider();
            var mockSystem = mockRepository.Create<IMatchingSystem>();
            var systemType = mockSystem.Object.GetType();
            mockTypeAttributesProvider.Setup(item => item.GetAttributes(systemType)).Returns(Array.Empty<object>);
            // Act
            // Assert
            Assert.DoesNotThrow(() => provider.RegisterRequirements(systemType));
            this.mockRepository.VerifyAll();
        }
    }
}
