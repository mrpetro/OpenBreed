//using Moq;
//using NUnit.Framework;
//using OpenBreed.Common.Tools.Collections;
//using System;

//namespace OpenBreed.Common.Tools.Test.Collections
//{
//    [TestFixture]
//    public class IdMapTests
//    {
//        private MockRepository mockRepository;



//        [SetUp]
//        public void SetUp()
//        {
//            this.mockRepository = new MockRepository(MockBehavior.Strict);
//        }

//        private IdMap<string> CreateIdMap()
//        {
//            return new IdMap<string>();
//        }

//        [Test]
//        public void TryGetValue_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var idMap = this.CreateIdMap();
//            int id = 0;
//            string value = default(string);

//            // Act
//            var result = idMap.TryGetValue(
//                id,
//                out value);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }

//        [Test]
//        public void NewId_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var idMap = this.CreateIdMap();

//            // Act
//            var result = idMap.NewId();

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }

//        [Test]
//        public void Insert_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var idMap = this.CreateIdMap();
//            int id = 0;
//            string item = default(string);

//            // Act
//            var result = idMap.Insert(
//                id,
//                item);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }

//        [Test]
//        public void Add_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var idMap = this.CreateIdMap();
//            string item = default(string);

//            // Act
//            var result = idMap.Add(
//                item);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }

//        [Test]
//        public void RemoveById_StateUnderTest_ExpectedBehavior()
//        {
//            // Arrange
//            var idMap = this.CreateIdMap();
//            int id = 0;

//            // Act
//            idMap.RemoveById(
//                id);

//            // Assert
//            Assert.Fail();
//            this.mockRepository.VerifyAll();
//        }
//    }
//}
