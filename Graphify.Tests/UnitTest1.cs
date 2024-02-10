namespace Graphify.Tests;

public class TestExample
{
    private IList<int> _list = null!;

    [SetUp]
    public void Setup()
    {
        _list = new List<int>();
    }

    [Test]
    public void GIVEN_Empty_list_WHEN_no_items_added_THEN_empty_list_expected()
    {
        Assert.That(_list, Is.Empty);
    }

    [TestCase(1, 1)]
    [TestCase(-124412, -124412)]
    [TestCase(0, 0)]
    public void GIVEN_Empty_list_WHEN_one_item_added_THEN_same_item_at_zero_index_expected(int item, int expected)
    {
        _list.Add(item);

        Assert.That(_list[0], Is.EqualTo(expected));
    }
}