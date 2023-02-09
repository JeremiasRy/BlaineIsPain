namespace Types;
public class Train
{
    readonly int _trackMaxPosition;
    int _position;
    readonly TrainDirection _direction;
    
    public char Piece;
    public int WaitTimer { get; set; }
    public bool IsExpress { get; init; } = false;
    public List<int> Positions { get
        {
            var result = new List<int>() { _position};
            foreach(var carriage in Carriage)
            {
                result.Add(carriage.Position(_position));
            }
            return result;
        } }
    public Carriage[] Carriage { get; set; }
    public void Move() 
    {
        _position = _direction == TrainDirection.CounterClockwise 
            ? _position - 1 < 0 
                ? _trackMaxPosition : _position - 1 
            : _position + 1 > _trackMaxPosition 
                ? 0 : _position + 1;
    }
    public Train(string train, int position, int trackMaxPosition)
    {
        _direction = char.IsUpper(train[0]) ? TrainDirection.CounterClockwise : TrainDirection.Clockwise; 
        _position = position;
        Carriage = new Carriage[train.Length - 1];
        _trackMaxPosition = trackMaxPosition;
        Piece = char.ToUpper(train[0]);
        IsExpress = Piece == 'X';
        for (int i = 0; i < train.Length - 1; i++)
        {
            Carriage[i] = new Carriage(trackMaxPosition, _direction == TrainDirection.CounterClockwise ? i + 1 : 0 - (i + 1)); 
        }
    }


}

