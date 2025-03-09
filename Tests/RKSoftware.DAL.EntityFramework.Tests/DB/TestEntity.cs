namespace RKSoftware.DAL.EntityFramework.Tests.DB;

public class TestEntity
{
    public string? TestStringProperty { get; set; }

    public long TestLongProperty { get; set; }

    public DateTime TestDateProperty { get; set; }

    public bool CompareTo(TestEntity other)
    {
        ArgumentNullException.ThrowIfNull(other, nameof(other));

        return TestStringProperty == other.TestStringProperty
            && TestLongProperty == other.TestLongProperty
            && TestDateProperty == other.TestDateProperty;
    }
}
