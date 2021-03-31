using System;

namespace RKSoftware.DAL.EntityFramework.Domain
{
    public class TestEntity
    {
        public string TestStringProperty { get; set; }

        public long TestLongProperty { get; set; }

        public DateTime TestDateProperty { get; set; }

        public bool CompareTo(TestEntity other)
        {
            return TestStringProperty == other.TestStringProperty
                && TestLongProperty == other.TestLongProperty
                && TestDateProperty == other.TestDateProperty;
        }
    }
}
