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

        public static TheoryData<Vector2, IBoxShape, Vector2, IBoxShape, bool> CheckBoxVsBoxData()
        {
            return new TheoryData<Vector2, IBoxShape, Vector2, IBoxShape, bool>
            {
                {
                    new Vector2(0,0),
                    MockBoxShape(-5,-10,20,4),
                    new Vector2(0,0),
                    MockBoxShape(-5,-10,20,4),
                    true
                },
                {
                    new Vector2(0,0),
                    MockBoxShape(-10,-10,-5,4),
                    new Vector2(0,0),
                    MockBoxShape(-5,-10,20,4),
                    false
                }
            };
        }

        [Theory, MemberData(nameof(CheckBoxVsBoxData))]
        public void CheckBoxVsBox_TestInput_ExpectedResult(
            Vector2 posA,
            IBoxShape shapeA,
            Vector2 posB,
            IBoxShape shapeB,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
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

        public static TheoryData<Vector2, ICircleShape, Vector2, IBoxShape, bool> CheckCircleVsBoxData()
        {
            return new TheoryData<Vector2, ICircleShape, Vector2, IBoxShape, bool>
            {
                {
                    new Vector2(0,0),
                    MockCircleShape(-10, 0, 5),
                    new Vector2(0,0),
                    MockBoxShape(-5, -10, 20, 10),
                    false
                },
                {
                    new Vector2(0,0),
                    MockCircleShape(-9, 0, 5),
                    new Vector2(0,0),
                    MockBoxShape(-5, -10, 20, 10),
                    true
                }
            };
        }

        [Theory, MemberData(nameof(CheckCircleVsBoxData))]
        public void CheckCircleVsBox_TestInput_ExpectedResult(
            Vector2 posA,
            ICircleShape shapeA,
            Vector2 posB,
            IBoxShape shapeB,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();

            Vector2 projection = default;

            // Act
            var result = collisionChecker.CheckCircleVsBox(
                posA,
                shapeA,
                posB,
                shapeB,
                out projection);

            // Assert
            Assert.True(result == expectedResult);
        }

        public static TheoryData<Vector2, ICircleShape, Vector2, ICircleShape, bool> CheckCircleVsCircleData()
        {
            return new TheoryData<Vector2, ICircleShape, Vector2, ICircleShape, bool>
            {
                {
                    new Vector2(0,0),
                    MockCircleShape(new Vector2(0,0), 2.5f),
                    new Vector2(0,0),
                    MockCircleShape(new Vector2(5,0), 2.5f),
                    false
                },
                {
                    new Vector2(0,0),
                    MockCircleShape(new Vector2(0,0), 2.501f),
                    new Vector2(0,0),
                    MockCircleShape(new Vector2(5,0), 2.5f),
                    true
                }
            };
        }

        [Theory, MemberData(nameof(CheckCircleVsCircleData))]
        public void CheckCircleVsCircle_TestInput_ExpectedResult(
            Vector2 posA,
            ICircleShape shapeA,
            Vector2 posB,
            ICircleShape shapeB,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();

            Vector2 projection = default;

            // Act
            var result = collisionChecker.CheckCircleVsCircle(
                posA,
                shapeA,
                posB,
                shapeB,
                out projection);

            // Assert
            Assert.True(result == expectedResult);
        }

        public static TheoryData<Vector2, IPointShape, Vector2, IBoxShape, bool> CheckPointVsBoxData()
        {
            return new TheoryData<Vector2, IPointShape, Vector2, IBoxShape, bool>
            {
                {
                    new Vector2(1,2),
                    MockPointShape(2,1),
                    new Vector2(1,2),
                    MockBoxShape(-5,-10,20,4),
                    true
                },
                {
                    new Vector2(-15,0),
                    MockPointShape(2,1),
                    new Vector2(1,2),
                    MockBoxShape(-5,-10,20,4),
                    false
                }
            };
        }

        [Theory, MemberData(nameof(CheckPointVsBoxData))]
        public void CheckPointVsBox_TestInput_ExpectedResult(
            Vector2 posA,
            IPointShape shapeA,
            Vector2 posB,
            IBoxShape shapeB,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
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

        public static TheoryData<Vector2, IPointShape, Vector2, ICircleShape, bool> CheckPointVsCircleData()
        {
            return new TheoryData<Vector2, IPointShape, Vector2, ICircleShape, bool>
            {
                {
                    new Vector2(1,2),
                    MockPointShape(2,1),
                    new Vector2(1,2),
                    MockCircleShape( new Vector2(1,0), 5.0f),
                    true
                },
                {
                    new Vector2(-15,0),
                    MockPointShape(2,1),
                    new Vector2(1,2),
                    MockCircleShape( new Vector2(0,0), 5.0f),
                    false
                }
            };
        }

        [Theory, MemberData(nameof(CheckPointVsCircleData))]
        public void CheckPointVsCircle_TestInput_ExpectedResult(
            Vector2 posA,
            IPointShape shapeA,
            Vector2 posB,
            ICircleShape shapeB,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
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

        public static TheoryData<Vector2, IPointShape, Vector2, IPointShape, bool> CheckPointVsPointData()
        {
            return new TheoryData<Vector2, IPointShape, Vector2, IPointShape, bool>
            {
                {
                    new Vector2(1,2),
                    MockPointShape(2,1),
                    new Vector2(2,1),
                    MockPointShape(1,2),
                    true
                },
                {
                    new Vector2(2,2),
                    MockPointShape(2,2),
                    new Vector2(2,2),
                    MockPointShape(2,2),
                    true
                },
                {
                    new Vector2(-15,0),
                    MockPointShape(2,1),
                    new Vector2(1,2),
                    MockPointShape(0,0),
                    false
                }
            };
        }

        [Theory, MemberData(nameof(CheckPointVsPointData))]
        public void CheckPointVsPoint_TestInput_ExpectedResult(
            Vector2 posA,
            IPointShape shapeA,
            Vector2 posB,
            IPointShape shapeB,
            bool expectedResult)
        {
            // Arrange
            var collisionChecker = this.CreateCollisionChecker();
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

        private static IBoxShape MockBoxShape(float minX, float minY, float maxX, float maxY)
        {
            var shapeMock = new Mock<IBoxShape>(MockBehavior.Loose);
            shapeMock.Setup(item => item.X).Returns(minX);
            shapeMock.Setup(item => item.Y).Returns(minY);
            shapeMock.Setup(item => item.Width).Returns(maxX - minX);
            shapeMock.Setup(item => item.Height).Returns(maxY - minY);
            return shapeMock.Object;
        }

        private static IBoxShape MockBoxShape(Vector2 pos, Vector2 size) => MockBoxShape(pos.X, pos.Y, size.X, size.Y);

        private static ICircleShape MockCircleShape(float posX, float posY, float radius) => MockCircleShape(new Vector2(posX, posY), radius);

        private static ICircleShape MockCircleShape(Vector2 center, float radius)
        {
            var shapeMock = new Mock<ICircleShape>(MockBehavior.Loose);
            shapeMock.Setup(item => item.Center).Returns(center);
            shapeMock.Setup(item => item.Radius).Returns(radius);
            return shapeMock.Object;
        }

        private static IPointShape MockPointShape(Vector2 pos) => MockPointShape(pos.X, pos.Y);

        private static IPointShape MockPointShape(float x, float y)
        {
            var shapeMock = new Mock<IPointShape>(MockBehavior.Loose);
            shapeMock.Setup(item => item.X).Returns(x);
            shapeMock.Setup(item => item.Y).Returns(y);
            return shapeMock.Object;
        }

        #endregion Private Methods
    }
}