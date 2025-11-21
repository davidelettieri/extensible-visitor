namespace ExtensibleVisitor._02_ObjectOriented;

// Initially we have 3 shapes: Square, Circle, TranslatedShape and one "tool": ContainsPoint
public interface IShape
{
    bool ContainsPoint(Point point);
}

public sealed record Point(double X, double Y);

public record Square(double Length) : IShape
{
    public bool ContainsPoint(Point point) =>
        point.X >= 0 && point.X <= Length &&
        point.Y >= 0 && point.Y <= Length;
}

public record Circle(double Radius) : IShape
{
    public bool ContainsPoint(Point point) =>
        point.X * point.X + point.Y * point.Y <= Radius * Radius;
}

public record TranslatedShape(IShape Shape, Point Point) : IShape
{
    public bool ContainsPoint(Point point) =>
        Shape.ContainsPoint(
            new Point(point.X - Point.X, point.Y - Point.Y));
}

// Now we want to add a new share UnionShape and a new tool Shrink
// without modifying existing code (think OCP - Open/Closed Principle)

public sealed record UnionShape(IShape Shape1, IShape Shape2) : IShape
{
    public bool ContainsPoint(Point point) =>
        Shape1.ContainsPoint(point) || Shape2.ContainsPoint(point);
}

public interface IShrinkableShape : IShape
{
    IShrinkableShape Shrink(double num);
}

public record ShrinkableSquare(double Length) : Square(Length), IShrinkableShape
{
    public IShrinkableShape Shrink(double num) => new ShrinkableSquare(Length / num);
}

public record ShrinkableCircle(double Radius) : Circle(Radius), IShrinkableShape
{
    public IShrinkableShape Shrink(double num) => new ShrinkableCircle(Radius * num);
}

public record ShrinkableTranslatedShape(IShrinkableShape ShrinkableShape, Point Point)
    : TranslatedShape(ShrinkableShape, Point), IShrinkableShape
{
    public IShrinkableShape Shrink(double num) =>
        new ShrinkableTranslatedShape(
            ShrinkableShape.Shrink(num),
            Point);
}
