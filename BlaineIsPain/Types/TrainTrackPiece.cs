namespace Types;
using BlaineIsPain;
public class TrainTrackPiece
{
    public TrainTrackPiece(int y, int x, char piece)
    {
        X = x;
        Y = y;
        _originalPiece = piece;
    }
    private readonly char _originalPiece;
    private char _piece;
    public int TrackPosition { get; set; }
    public int Y { get; set; }
    public int X { get; set; }
    public bool HasTrainCarriage { get; set; } = false;
    public char Piece { get => HasTrainCarriage ? _piece : _originalPiece; private set => _piece = value; }
    public void Occupy(char carriage)
    {
        HasTrainCarriage = true;
        Piece = carriage;
    }
    public void UnOccupy() => HasTrainCarriage = false;
    public void Draw()
    {
        ScreenBuffer.Draw(Piece, Y, X);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public static bool operator == (TrainTrackPiece left, TrainTrackPiece right)
    {
        return left.Equals(right);
    }
    public static bool operator != (TrainTrackPiece left, TrainTrackPiece right)
    {
        return left.Equals(right);
    }
    public override bool Equals(object? obj)
    {
        if (obj is TrainTrackPiece compareTo)
        {
            return compareTo.X == this.X && compareTo.Y == this.Y;
        }
        return false;
    }
}

