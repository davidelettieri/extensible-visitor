using ExtensibleVisitor._03_Visitor;

namespace Approaches.Tests._03_Visitor;

public sealed class ImplementationTests
{
    [Fact]
    public void TestNestedTranslatedShapes()
    {
        // Arrange
        var circle = new Circle(10);
        var square = new Square(10);
        var t1 = new UnionShape(square, circle);
        var t2 = new TranslatedShape(t1, new Point(5, 5));

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => t2.Process(new UnionContainsPoint(new Point(0, 0))));
    }
}
