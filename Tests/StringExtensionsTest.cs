using Pyther.Core.Extensions;
using System.Globalization;
using System.Runtime.Versioning;

namespace Tests;

[TestClass]
public class StringExtensionsTest
{
    [TestMethod]
    public void TestManipulation()
    {
        Assert.AreEqual("abcabcabc", "abc".Repeat(3));
        Assert.AreEqual("cba", "abc".Reverse());
        Assert.AreEqual("Hello", "Hello World!".Clip(5));

        Assert.AreEqual("This is a test.", "This <b>is a</b> test.".RemoveTags());
        Assert.AreEqual("This is a test.", "This is </br>a test.".RemoveTags());
        Assert.AreEqual("This test.", "This <!-- is a -->test.".RemoveHtmlComments());
    }

    [TestMethod]
    public void TestTransform()
    {
        Assert.AreEqual("YWJj", "abc".Base64Encode());
        Assert.AreEqual("abc", "YWJj".Base64Decode());
        Assert.AreEqual("name", "Name".LowerFirst());
        Assert.AreEqual("Name", "name".UpperFirst());
    }

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

    [TestMethod]
    public void TestParse()
    {
        Assert.AreEqual(123, "123".Parse<int>());
        Assert.AreEqual(123.45, "123.45".Parse<double>());
        Assert.AreEqual(123.45f, "123.45".Parse<float>());
        Assert.AreEqual(true, "true".Parse<bool>());
        Assert.AreEqual(false, "false".Parse<bool>());
        Assert.AreEqual(FileMode.Append, "Append".Parse<FileMode>());
    }

    [TestMethod]
    public void TestCheck()
    {
        string? nullString = null;
        Assert.AreEqual(true, nullString.IsEmpty());
        Assert.AreEqual(true, "".IsEmpty());
        Assert.AreEqual(true, " ".IsEmpty());
        Assert.AreEqual(false, " 123 ".IsEmpty());
        Assert.AreEqual(true, "123".IsInteger());
        Assert.AreEqual(false, "123.12".IsInteger());
        Assert.AreEqual(false, "abc".IsInteger());

        Assert.AreEqual(true, "me@you.de".IsEmail());
        Assert.AreEqual(false, "me@you.de.".IsEmail());
        Assert.AreEqual(false, "hello".IsEmail());

        Assert.AreEqual(false, "abcde".Contains("bCd"));
        Assert.AreEqual(true, "abcde".Contains("bCd", StringComparison.OrdinalIgnoreCase));
        Assert.AreEqual(true, "abcde".ContainsIgnoreCase("bCd"));
    }

    [TestMethod]    
    public void TestUnsorted()
    {
        Assert.AreEqual(3, "1,,2,3,4".SplitToType<int>(',')?.ToArray()[2]);
        Assert.AreEqual(3, "1,,2;3,4;".SplitToType<int>(StringSplitOptions.RemoveEmptyEntries, ',', ';')?.ToArray()[2]);
    }

    [SupportedOSPlatform("windows")]
    [TestMethod]
    public void TestCryptWindows()
    {
        Assert.AreEqual("Hello World!", "Hello World!".EncryptRSA("MyKey")?.DecryptRSA("MyKey"));
    }
}
