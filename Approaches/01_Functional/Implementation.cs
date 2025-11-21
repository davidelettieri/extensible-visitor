namespace ExtensibleVisitor._01_Functional;

public interface IShape;

public sealed record Point(double X, double Y);

public sealed record Square(double Length) : IShape;

public sealed record Circle(double Radius) : IShape;

/// <summary>
/// This type is added later
/// </summary>
/// <param name="Shape"></param>
/// <param name="Point"></param>
public sealed record TranslatedShape(IShape Shape, Point Point) : IShape;

public static class Tools
{
    public static bool ContainsPoint(Point point, IShape shape) =>
        shape switch
        {
            Square s => point.X >= 0 && point.X <= s.Length &&
                        point.Y >= 0 && point.Y <= s.Length,
            Circle c => point.X * point.X + point.Y * point.Y <= c.Radius * c.Radius,
            TranslatedShape ts => ContainsPoint(
                new Point(point.X - ts.Point.X, point.Y - ts.Point.Y),
                ts.Shape),
            _ => throw new NotSupportedException($"Shape of type {shape.GetType().Name} is not supported")
        };

    // Adding a new tool Shrink is easy - just add a new function
    public static IShape Shrink(double num, IShape shape) =>
        shape switch
        {
            Square s => new Square(s.Length / num),
            Circle c => new Circle(c.Radius * num),
            TranslatedShape ts => ts with { Shape = Shrink(num, ts.Shape) },
            _ => throw new NotSupportedException($"Shape of type {shape.GetType().Name} is not supported")
        };

    // We cannot add any new shape without modifying existing code
}
