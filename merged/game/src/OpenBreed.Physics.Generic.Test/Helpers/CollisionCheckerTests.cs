//using Moq;
//using OpenBreed.Physics.Generic.Managers;
//using OpenBreed.Physics.Interface;
//using OpenTK.Mathematics;
//using OpenTK.Windowing.Common;
//using System;
//using Xunit;

//namespace OpenBreed.Physics.Generic.Test.Helpers
//{
//    public class CollisionCheckerTests
//    {
//        private MockRepository mockRepository;

//        [SetUp]
//        public void SetUp()
//        {
//            this.mockRepository = new MockRepository(MockBehavior.Strict);


//        }

//        private CollisionChecker CreateCollisionChecker()
//        {
//            return new CollisionChecker();
//        }

//        [Fact]
//        public void CheckBoxVsBox_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var collisionChecker = this.CreateCollisionChecker();
//            var posA = default(Vector2);
//            IBoxShape shapeA = null;
//            var posB = default(Vector2);
//            IBoxShape shapeB = null;
//            var projection = default(Vector2);

//            // Act
//            var result = collisionChecker.CheckBoxVsBox(
//                posA,
//                shapeA,
//                posB,
//                shapeB,
//                out projection);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }

//        //[Test]
//        //public void CheckCircleVsBox_StateUnderTest_ExpectedBehavior()
//        //{
//        //    // Arrange
//        //    var collisionChecker = this.CreateCollisionChecker();
//        //    var posA = default(Vector2);
//        //    ICircleShape shapeA = null;
//        //    var posB = default(Vector2);
//        //    IBoxShape shapeB = null;
//        //    var projection = default(Vector2);

//        //    // Act
//        //    var result = collisionChecker.CheckCircleVsBox(
//        //        posA,
//        //        shapeA,
//        //        posB,
//        //        shapeB,
//        //        out projection);

//        //    // Assert
//        //    Assert.Fail();
//        //    this.mockRepository.VerifyAll();
//        //}

//        [Test]
//        public void CheckCircleVsCircle_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var collisionChecker = this.CreateCollisionChecker();
//            var posA = default(Vector2);
//            ICircleShape shapeA = null;
//            var posB = default(Vector2);
//            ICircleShape shapeB = null;
//            var projection = default(Vector2);

//            // Act
//            var result = collisionChecker.CheckCircleVsCircle(
//                posA,
//                shapeA,
//                posB,
//                shapeB,
//                out projection);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }

//        [Test]
//        public void CheckPointVsBox_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var collisionChecker = this.CreateCollisionChecker();
//            var posA = default(Vector2);
//            IPointShape pointShape = null;
//            var posB = default(Vector2);
//            IBoxShape shapeB = null;
//            var projection = default(Vector2);

//            // Act
//            var result = collisionChecker.CheckPointVsBox(
//                posA,
//                pointShape,
//                posB,
//                shapeB,
//                out projection);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }

//        static IEnumerable<TestCaseData> CheckPointVsCircleData()
//        {
//            yield return new TestCaseData(new Vector2(0, 0), new Vector2(0, 0)).SetName("A");
//        }

//        [Test, TestCaseSource(nameof(CheckPointVsCircleData))]
//        public void CheckPointVsCircle_StateUnderTest_ExpectedBehavior(Vector2 posA, Vector2 posB)
//        {
//            // Arrange
//            var collisionChecker = this.CreateCollisionChecker();
//            IPointShape shapeA = null;
//            ICircleShape shapeB = null;
//            var projection = default(Vector2);

//            // Act
//            var result = collisionChecker.CheckPointVsCircle(
//                posA,
//                shapeA,
//                posB,
//                shapeB,
//                out projection);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }

//        [Test]
//        public void CheckPointVsPoint_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var collisionChecker = this.CreateCollisionChecker();
//            var posA = default(Vector2);
//            IPointShape pointShapeA = null;
//            var posB = default(Vector2);
//            IPointShape pointShapeB = null;
//            var projection = default(Vector2);

//            // Act
//            var result = collisionChecker.CheckPointVsPoint(
//                posA,
//                pointShapeA,
//                posB,
//                pointShapeB,
//                out projection);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }
//    }
//}
