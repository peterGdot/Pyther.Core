using Pyther.Core.Extensions;

namespace Tests;

[TestClass]
public class StringExtensionsTest
{
    [TestMethod]
    public void TestNamingPolicy()
    {
        // [snake_case -> ...]
        Assert.AreEqual("MyClassName", "my_class_name".SnakeToTitleCase());
        Assert.AreEqual("myClassName", "my_class_name".SnakeToCamelCase());
        Assert.AreEqual("my-class-name", "my_class_name".SnakeToKebabCase());

        // [TitleCase -> ...]
        Assert.AreEqual("my_class_name", "MyClassName".TitleToSnakeCase());
        Assert.AreEqual("my-class-name", "MyClassName".TitleToKebabCase());
        Assert.AreEqual("myClassName", "MyClassName".TitleToCamelCase());

        // [kebabCase -> ...]
        Assert.AreEqual("MyClassName", "my-class-name".KebabToTitleCase());
        Assert.AreEqual("my_class_name", "my-class-name".KebabToSnakeCase());
        Assert.AreEqual("myClassName", "my-class-name".KebabToCamelCase());

        // [camelCase -> ...]
        Assert.AreEqual("my_class_name", "myClassName".CamelToSnakeCase());
        Assert.AreEqual("my-class-name", "myClassName".CamelToKebabCase());
        Assert.AreEqual("MyClassName", "myClassName".CamelToTitleCase());
    }
}
