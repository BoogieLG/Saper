public enum FieldDirection
{
    West,
    WestNorth,
    North,
    EastNorth,
    East,
    EastSouth,
    South,
    WestSouth
};

public static class FieldDirectionExtensions
{
    public static FieldDirection Opposite (this FieldDirection direction)
    {
        return (int) direction < 4 ? (direction + 4) : (direction - 4);
    }
}
