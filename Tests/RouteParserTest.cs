using Pyther.Core;

namespace Tests;

[TestClass]
public class RouteParserTest
{
    [TestMethod]
    public void TestRoutes()
    {
        RouteParser parser = new();

        // shoul all be true
        Assert.AreEqual(true, parser.Is("orders", "orders"));

        Assert.AreEqual(true, parser.Is("orders/{id}", "orders/123"));
        Assert.AreEqual("123", parser["id"]);

        Assert.AreEqual(true, parser.Is("orders/{id}/test/{a}-{b}", "orders/123/test/hello-world"));
        Assert.AreEqual("123", parser["id"]);
        Assert.AreEqual(123, parser.Get<int>("id"));
        Assert.AreEqual("hello", parser["a"]);
        Assert.AreEqual("world", parser["b"]);

        // should all be false
        Assert.AreEqual(false, parser.Is("orders", "order"));
        Assert.AreEqual(false, parser.Is("orders/{id}", "orders/123/test"));
        Assert.AreEqual(null, parser["id"]);
        Assert.AreEqual(false, parser.Is("orders/{id}", ""));
        Assert.AreEqual(false, parser.Is("orders/{id}", "products"));
    }
}