using Pyther.Core;

namespace Tests
{
    [TestClass]
    public class RouteParserTest
    {
        [TestMethod]
        public void TestRoutes()
        {
            RouteParser parser = new();

            // shoul all be true
            Assert.AreEqual(parser.Is("orders", "orders"), true);

            Assert.AreEqual(parser.Is("orders/{id}", "orders/123"), true);
            Assert.AreEqual(parser["id"], "123");

            Assert.AreEqual(parser.Is("orders/{id}/test/{a}-{b}", "orders/123/test/hello-world"), true);
            Assert.AreEqual(parser["id"], "123");
            Assert.AreEqual(parser.Get<int>("id"), 123);
            Assert.AreEqual(parser["a"], "hello");
            Assert.AreEqual(parser["b"], "world");

            // should all be false
            Assert.AreEqual(parser.Is("orders", "order"), false);
            Assert.AreEqual(parser.Is("orders/{id}", "orders/123/test"), false);
            Assert.AreEqual(parser["id"], null);
            Assert.AreEqual(parser.Is("orders/{id}", ""), false);
            Assert.AreEqual(parser.Is("orders/{id}", "products"), false);
        }
    }
}