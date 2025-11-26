namespace ExtensibleVisitor._04_ExtensibleVisitor;

public interface IShape
{
    T Process<T>(IShapeProcessor<T> processor);
}

public interface IShapeProcessor<out T>
{
    T ForSquare(Square square);
    T ForCircle(Circle circle);
    T ForTranslatedShape(TranslatedShape translatedShape);
}

public sealed record Point(double X, double Y);

public sealed record Square(double Length) : IShape
{
    public T Process<T>(IShapeProcessor<T> processor) => processor.ForSquare(this);
}

public sealed record Circle(double Radius) : IShape
{
    public T Process<T>(IShapeProcessor<T> processor) => processor.ForCircle(this);
}

public sealed record TranslatedShape(IShape Shape, Point Point) : IShape
{
    public T Process<T>(IShapeProcessor<T> processor) => processor.ForTranslatedShape(this);
}

public class ContainsPoint(Point point) : IShapeProcessor<bool>
{
    protected virtual ContainsPoint MakeContainsPoint(Point p ) => new(p);

    public bool ForSquare(Square square) =>
        point.X >= 0 && point.X <= square.Length &&
        point.Y >= 0 && point.Y <= square.Length;

    public bool ForCircle(Circle circle) =>
        point.X * point.X + point.Y * point.Y <= circle.Radius * circle.Radius;

    public bool ForTranslatedShape(TranslatedShape translatedShape) =>
        translatedShape.Shape.Process(MakeContainsPoint(
            new Point(point.X - translatedShape.Point.X, point.Y - translatedShape.Point.Y)));
}

public class UnionContainsPoint(Point point) : ContainsPoint(point), IUnionShapeProcessor<bool>
{
    protected override ContainsPoint MakeContainsPoint(Point p) => new UnionContainsPoint(p);

    public bool ForUnionShape(UnionShape unionShape) =>
        unionShape.Shape1.Process(this) || unionShape.Shape2.Process(this);
}

// Now we want to add a new tool Shrink and a new shape UnionShape

public sealed class Shrink(double num) : IShapeProcessor<IShape>
{
    public IShape ForSquare(Square square) => new Square(square.Length / num);

    public IShape ForCircle(Circle circle) => new Circle(circle.Radius * num);

    public IShape ForTranslatedShape(TranslatedShape translatedShape) =>
        translatedShape with { Shape = translatedShape.Shape.Process(this) };
}

public interface IUnionShapeProcessor<out T> : IShapeProcessor<T>
{
    T ForUnionShape(UnionShape unionShape);
}

public sealed record UnionShape(IShape Shape1, IShape Shape2) : IShape
{
    public T Process<T>(IShapeProcessor<T> processor)
    {
        if (processor is IUnionShapeProcessor<T> unionProcessor)
        {
            return unionProcessor.ForUnionShape(this);
        }

        throw new NotSupportedException($"Processor of type {processor.GetType().Name} does not support UnionShape");
    }
}
