using Moq;
using Newtonsoft.Json;
using OpenBreed.Physics.Generic.Managers;
using OpenBreed.Physics.Interface;
using OpenTK.Graphics.OpenGL;
using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace OpenBreed.Physics.Generic.Test.Managers
{
    public class CollisionCheckerTests
    {
        #region Private Fields

        private MockRepository mockRepository;

        #endregion Private Fields

        #region Public Constructors

        public CollisionCheckerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
        }

        #endregion Public Constructors

        #region Public Methods

        public static TheoryData<Vector2, Box2, Vector2, Box2, bool> CheckBoxVsBoxData()
        {
            return new TheoryData<Vector2, Box2, Vector2, Box2, bool>
            {
                { new Vector2(1,2), new Box2(-5,-10,20,4),new Vector2(1,2), new Box2(-5,-10,20,4), true },
                { new Vector2(-15,0), new Box2(-5,-10,20,4),new Vector2(1,2), new Box2(-5,-10,20,4), false }
            };
        }

        [Theory, MemberData(nameof(CheckBoxVsBoxData))]
        public void CheckBoxVsBox_TestInput_ExpectedResult(
            Vector2 posA,
            Box2 boxA,
            Vector2 posB,
            Box2 boxB,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
            var shapeA = MockBoxShape(boxA.Min, boxA.Size);
            var shapeB = MockBoxShape(boxB.Min, boxB.Size);
            Vector2 projection = default;

            // Act
            var result = collisionChecker.CheckBoxVsBox(
                posA,
                shapeA,
                posB,
                shapeB,
                out projection);

            // Assert
            Assert.True(result == expectedResult);
        }

        [Fact]
        public void CheckCircleVsBox_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
            Vector2 posA = default(global::OpenTK.Mathematics.Vector2);
            ICircleShape shapeA = null;
            Vector2 posB = default(global::OpenTK.Mathematics.Vector2);
            IBoxShape shapeB = null;
            Vector2 projection = default(global::OpenTK.Mathematics.Vector2);

            // Act
            var result = collisionChecker.CheckCircleVsBox(
                posA,
                shapeA,
                posB,
                shapeB,
                out projection);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void CheckCircleVsCircle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
            Vector2 posA = default(global::OpenTK.Mathematics.Vector2);
            ICircleShape shapeA = null;
            Vector2 posB = default(global::OpenTK.Mathematics.Vector2);
            ICircleShape shapeB = null;
            Vector2 projection = default(global::OpenTK.Mathematics.Vector2);

            // Act
            var result = collisionChecker.CheckCircleVsCircle(
                posA,
                shapeA,
                posB,
                shapeB,
                out projection);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        public static TheoryData<Vector2, Vector2, Vector2, Box2, bool> CheckPointVsBoxData()
        {
            return new TheoryData<Vector2, Vector2, Vector2, Box2, bool>
            {
                { new Vector2(1,2), new Vector2(2,1),new Vector2(1,2), new Box2(-5,-10,20,4), true },
                { new Vector2(-15,0), new Vector2(2,1),new Vector2(1,2), new Box2(-5,-10,20,4), false }
            };
        }

        [Theory, MemberData(nameof(CheckPointVsBoxData))]
        public void CheckPointVsBox_TestInput_ExpectedResult(
            Vector2 posA,
            Vector2 pointOffset,
            Vector2 posB,
            Box2 box,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
            var shapeA = MockPointShape(pointOffset);
            var shapeB = MockBoxShape(box.Min, box.Size);
            Vector2 projection = default;

            // Act
            var result = collisionChecker.CheckPointVsBox(
                posA,
                shapeA,
                posB,
                shapeB,
                out projection);

            // Assert
            Assert.True(result == expectedResult);
        }

        public static TheoryData<Vector2, Vector2, Vector2, Vector2, float, bool> CheckPointVsCircleData()
        {
            return new TheoryData<Vector2, Vector2, Vector2, Vector2, float, bool>
            {
                { new Vector2(1,2), new Vector2(2,1),new Vector2(1,2),new Vector2(1,0), 5.0f, true },
                { new Vector2(-15,0), new Vector2(2,1),new Vector2(1,2),new Vector2(0,0), 5.0f, false }
            };
        }

        [Theory, MemberData(nameof(CheckPointVsCircleData))]
        public void CheckPointVsCircle_TestInput_ExpectedResult(
            Vector2 posA,
            Vector2 pointOffset,
            Vector2 posB,
            Vector2 circleOffset,
            float circleRadius,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
            var shapeA = MockPointShape(pointOffset);
            var shapeB = MockCircleShape(circleOffset, circleRadius);
            Vector2 projection = default;

            // Act
            var result = collisionChecker.CheckPointVsCircle(
                posA,
                shapeA,
                posB,
                shapeB,
                out projection);

            // Assert
            Assert.True(result == expectedResult);
        }

        public static TheoryData<Vector2, Vector2, Vector2, Vector2, bool> CheckPointVsPointData()
        {
            return new TheoryData<Vector2, Vector2, Vector2, Vector2, bool>
            {
                { new Vector2(1,2), new Vector2(2,1),new Vector2(2,1),new Vector2(1,2), true },
                { new Vector2(2,2), new Vector2(2,2),new Vector2(2,2),new Vector2(2,2), true },
                { new Vector2(-15,0), new Vector2(2,1),new Vector2(1,2),new Vector2(0,0), false }
            };
        }

        [Theory, MemberData(nameof(CheckPointVsPointData))]
        public void CheckPointVsPoint_TestInput_ExpectedResult(
            Vector2 posA,
            Vector2 pointOffsetA,
            Vector2 posB,
            Vector2 pointOffsetB,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
            var shapeA = MockPointShape(pointOffsetA);
            var shapeB = MockPointShape(pointOffsetB);
            Vector2 projection = default;

            // Act
            var result = collisionChecker.CheckPointVsPoint(
                posA,
                shapeA,
                posB,
                shapeB,
                out projection);

            // Assert
            Assert.True(result == expectedResult);
        }

        #endregion Public Methods

        #region Private Methods

        private CollisionChecker CreateCollisionChecker()
        {
            return new CollisionChecker();
        }

        private IBoxShape MockBoxShape(Vector2 pos, Vector2 size)
        {
            var shapeMock = mockRepository.Create<IBoxShape>();
            shapeMock.Setup(item => item.X).Returns(pos.X);
            shapeMock.Setup(item => item.Y).Returns(pos.Y);
            shapeMock.Setup(item => item.Width).Returns(size.X);
            shapeMock.Setup(item => item.Height).Returns(size.Y);
            return shapeMock.Object;
        }

        private ICircleShape MockCircleShape(Vector2 center, float radius)
        {
            var shapeMock = mockRepository.Create<ICircleShape>();
            shapeMock.Setup(item => item.Center).Returns(center);
            shapeMock.Setup(item => item.Radius).Returns(radius);
            return shapeMock.Object;
        }

        private IPointShape MockPointShape(Vector2 pos)
        {
            var shapeMock = mockRepository.Create<IPointShape>();
            shapeMock.Setup(item => item.X).Returns(pos.X);
            shapeMock.Setup(item => item.Y).Returns(pos.Y);
            return shapeMock.Object;
        }

        #endregion Private Methods
    }
}