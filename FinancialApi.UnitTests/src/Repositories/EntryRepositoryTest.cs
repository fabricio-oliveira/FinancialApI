using System;
using System.Diagnostics;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using NUnit.Framework;

namespace FinancialApi.UnitTests.repositories
{
    [TestFixture]
    public class EntryRepositoryTest
    {
        private EntryRepository _repository = null;

        [SetUp]
        public void Setup()
        {
            var context = DatabaseHelper.Connection();
            _repository = new EntryRepository(context);
        }

        [TearDown]
        public void Cleanup()
        {
            DatabaseHelper.CleanData();
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(12)]
        public void TestCount(int count)
        {
            for (int i = 0; i < count; i++)
                EntryFactory.Create();

            Assert.AreEqual(count, _repository.Count());
        }

        public void Update()
        {
            var created = EntryFactory.Create();
            created.Value = 300.00m;
            _repository.Save(created);

            var finded = _repository.Find(created.Id);

            Assert.AreEqual(300.00m, finded.Value);
        }

        [Test]
        public void TestSave()
        {
            var entry = EntryFactory.Build();

            _repository.Save(entry);
            Assert.IsNotNull(entry.Id);
        }

        [Test]
        public void TestUpdatedEntityd()
        {
            var created = EntryFactory.Create();

            //update value
            created.Value += 2;
            _repository.Update(created);

            //check
            var finded = _repository.Find(created.Id);
            Assert.AreEqual(created.Value, finded.Value);
        }

        [Test]
        public void TestFindEntityNotFound()
        {
            var entry = _repository.Find(1);
            Assert.IsNull(entry);
        }

        [Test]
        public void TestFindExistentEntity()
        {
            var created = EntryFactory.Create();
            var finded = _repository.Find(created.Id);
            Assert.AreEqual(created.Id, finded.Id);
        }

    }
}