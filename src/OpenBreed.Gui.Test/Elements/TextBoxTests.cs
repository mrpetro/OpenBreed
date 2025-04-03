using Moq;
using OpenBreed.Gui.Abstractions;
using OpenBreed.Gui.Abstractions.Builders;
using OpenBreed.Gui.Abstractions.Elements;
using OpenBreed.Gui.Builders;
using OpenBreed.Gui.Elements;
using OpenBreed.Rendering.Interface.Events;
using OpenTK.Mathematics;
using System;
using Xunit;

namespace OpenBreed.Gui.Test.Elements
{
    public class TextBoxTests
    {
        private MockRepository mockRepository;

        private Mock<TextBoxBuilder> mockTextBoxBuilder;

        public TextBoxTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockTextBoxBuilder = this.mockRepository.Create<TextBoxBuilder>();
        }

        private ITextBox CreateTextBox()
        {
            return new TextBox(
                this.mockTextBoxBuilder.Object);
        }

        [Fact]
        public void GetCharacter_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var textBox = this.CreateTextBox();
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
            var textBox = this.CreateTextBox();
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
            var textBox = this.CreateTextBox();
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
            var textBox = this.CreateTextBox();
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
            var textBox = this.CreateTextBox();

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
            var textBox = this.CreateTextBox();
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
            var textBox = this.CreateTextBox();
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
