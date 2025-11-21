using ExtensibleVisitor._01_Functional;

namespace Approaches.Tests._01_Functional;

public sealed class ImplementationTests
{
    [Fact]
    public void TestNestedTranslatedShapes()
    {
        // Arrange
        var circle = new Circle(10);
        var t1 = new TranslatedShape(circle, new Point(10, 10));
        var t2 = new TranslatedShape(t1, new Point(5, 5));

        // Act
        var contains = Tools.ContainsPointV2(new Point(0, 0), t2);

        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void TestUnionShapeContainsPoint()
    {
        // Arrange
        var square = new Square(5);
        var circle = new Circle(3);
        var unionShape = new UnionShape(square, circle);

        // Act
        var containsInSquare = Tools.ContainsPointV2(new Point(4, 4), unionShape);

        // Assert
        Assert.True(containsInSquare);
    }
}
