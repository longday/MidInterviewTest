using Xunit;
using Xunit.Abstractions;

namespace MidInterviewTest;

/*
    The task is to remove from the object tree all nodes
    where the Alive property is false. However, if a node
    has an Alive property that is true, all of its parents
    and children should remain in the tree.

    Extra:
    Perform the task using recursive and iterative algorithms,
    explain in which cases each one should be used, as well as
    the advantages and disadvantages of each approach.
*/

public record Node(int Id, int? ParentId, bool Alive, List<Node> Children);

public static class NodesCleaner
{
    public static void Clean(Node root)
    {
        throw new NotImplementedException();
    }
}

public class Tests
{
    [Fact]
    public void Test1()
    {
        var root = new Node(1, null, true, new List<Node>());
        var l11 = root.F();
        var l12 = root.T();
        l11.F().T();
        l12.F().T();
        root.T().F();
        root.F().T();
        //visualize tree
        //Print(root);
        NodesCleaner.Clean(root);
        var expected = """
            T1
            -F11
            --F111
            ---T1111
            -T12
            --F121
            ---T1211
            -T13
            --F131
            -F14
            --T141
            """.Trim();
        Assert.Equal(expected, Stringify(root).Trim());
    }

    [Fact]
    public void Test2()
    {
        var root = new Node(1, null, false, new List<Node>());
        root.F().F().F();
        root.F().T().F();
        //visualize tree
        //Print(root);
        NodesCleaner.Clean(root);
        var expected = """
            F1
            -F12
            --T121
            ---F1211
            """.Trim();
        Assert.Equal(expected, Stringify(root).Trim());
    }

    [Fact]
    public void Test3()
    {
        var root = new Node(1, null, false, new List<Node>());
        root.F().F().T();
        root.F().T().T();
        root.T().T().T();
        root.F().T().T();
        root.F().F().T();
        root.F().F().F();
        var l11 = root.F();
        l11.F();
        l11.T();
        l11.F().F();
        l11.F().T();
        l11.T().F();
        l11.T().T();

        var l12 = root.T();
        l12.F();
        l12.T();
        l12.F().F();
        l12.F().T();
        l12.T().F();
        l12.T().T();

        //visualize tree
        //Print(root);
        NodesCleaner.Clean(root);
        var expected = """
            F1
            -F11
            --F111
            ---T1111
            -F12
            --T121
            ---T1211
            -T13
            --T131
            ---T1311
            -F14
            --T141
            ---T1411
            -F15
            --F151
            ---T1511
            -F17
            --T172
            --F174
            ---T1741
            --T175
            ---F1751
            --T176
            ---T1761
            -T18
            --F181
            --T182
            --F183
            ---F1831
            --F184
            ---T1841
            --T185
            ---F1851
            --T186
            ---T1861
            """.Trim();

        Assert.Equal(expected, Stringify(root).Trim());
    }

    private readonly ITestOutputHelper _output;

    public Tests(ITestOutputHelper output)
    {
        _output = output;
    }

    private string Stringify(Node node, int level = 0)
    {
        return $"{new string('-', level)}{node.Alive.ToString()[0]}{node.Id}"
            + Environment.NewLine
            + string.Join("", node.Children.Select(child => Stringify(child, level + 1)));
        /*return $"{new string(' ', level * 2)}Id:{node.Id} pId{(node.ParentId.HasValue ? node.ParentId : "R")}:{node.Alive}"
            + Environment.NewLine
            + string.Join("", node.Children.Select(child => Stringify(child, level + 1)));*/
    }

    private void Print(Node node)
    {
        _output.WriteLine(Stringify(node));
    }
}

public static class NodeUtils
{
    public static Node T(this Node parent) => CreateChild(parent, true);

    public static Node F(this Node parent) => CreateChild(parent, false);

    public static Node CreateChild(this Node parent, bool alive)
    {
        var child = new Node(
            (parent.Children.Count > 0 ? parent.Children[^1].Id : parent.Id * 10) + 1,
            parent.Id,
            alive,
            new List<Node>()
        );

        parent.Children.Add(child);
        return child;
    }
}
