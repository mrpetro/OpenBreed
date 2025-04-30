using Moq;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Abstractions.Events;
using OpenTK.Mathematics;
using System;
using Xunit;

namespace OpenBreed.Gui.Test.Elements
{
    public class TextFieldTests
    {
        private MockRepository mockRepository;

        private Mock<TextFieldBuilder> mockTextFieldBuilder;

        public TextFieldTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockTextFieldBuilder = this.mockRepository.Create<TextFieldBuilder>();
        }

        private ITextField CreateTextField()
        {
            return new TextField(
                this.mockTextFieldBuilder.Object);
        }

        [Fact]
        public void GetCharacter_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textBox = this.CreateTextField();
            int columnIndex = 0;
            int lineIndex = 0;

            // Act
            var result = textBox.GetCharacter(
                columnIndex,
                lineIndex);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetCharacterIndex_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textBox = this.CreateTextField();
            int columnIndex = 0;
            int lineIndex = 0;

            // Act
            var result = textBox.GetCharacterIndex(
                columnIndex,
                lineIndex);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetLineLength_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textBox = this.CreateTextField();
            int lineIndex = 0;

            // Act
            var result = textBox.GetLineLength(
                lineIndex);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetLineCharacters_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textBox = this.CreateTextField();
            int lineIndex = 0;

            // Act
            var result = textBox.GetCharacters(
                lineIndex);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetPointerXPosition_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textBox = this.CreateTextField();

            // Act
            var result = textBox.Pointer.GetXPosition();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void Input_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textBox = this.CreateTextField();
            string text = null;

            // Act
            textBox.Input(
                text);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetIndexPosition_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textBox = this.CreateTextField();
            Vector2 position = default(global::OpenTK.Mathematics.Vector2);

            // Act
            var result = textBox.GetIndexPosition(
                position);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
