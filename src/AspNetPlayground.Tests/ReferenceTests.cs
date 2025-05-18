namespace AspNetPlayground.Tests;

using AspNetPlayground.Data;


public class ReferenceTests
{
    [Fact]
    public void RefContainer()
    {
        var container = new RefContainer(6);

        container[0] = 42;

        ref int refToElement = ref container[1];
        refToElement = 99;

        var gottenNonRef = container.GetRef(2);
        gottenNonRef = 100;

        ref int gottenRef = ref container.GetRef(3);
        gottenRef = 101;

        container.GetRef(4)++;

        container.GetRef(5) = 37;

        Assert.Equal(42, container[0]);
        Assert.Equal(99, container[1]);
        Assert.Equal(3, container[2]);  // Didn't change
        Assert.Equal(101, container[3]);
        Assert.Equal(6, container[4]);
        Assert.Equal(37, container[5]);
    }

    [Fact]
    public void RefArray()
    {
        int[] list = { 1, 2, 3, 4, 5 };

        ref int refToElement = ref list[1];
        refToElement = 42;

        list[2]++;

        Assert.Equal(42, list[1]);
        Assert.Equal(4, list[2]);

    }

    [Fact]
    public void Thing()
    {
        var d = new SortedDictionary<int, string>
        {
            [3] = "foo",
            [2] = "bar",
            [9] = "cat",
            [7] = "dog"
        };

        var t = new List<(int, string)>{};
        foreach (var (k, v) in d) {
            Console.WriteLine($"{k}: {v}");
            t.Add((k, v));
        }
        var e = new List<(int, string)>{
            (2, "bar"), (3, "foo"), (7, "dog"), (9, "cat")
        };

        Assert.Equal(e, t);

    }
}