namespace Types;
public class Carriage
{
    readonly int _trackMaxPosition;
    readonly int _nthCarriage;
    public int Position(int enginePosition)
    {
        if (enginePosition + _nthCarriage < 0)
        {
            return _trackMaxPosition - (Math.Abs(enginePosition + _nthCarriage) - 1);
        } else if (enginePosition + _nthCarriage - _trackMaxPosition == 1)
        {
            return 0;
        } else if (enginePosition + _nthCarriage >_trackMaxPosition)
        {
            return (enginePosition + _nthCarriage) % _trackMaxPosition - 1; 
        } else
        {
            return enginePosition + _nthCarriage;
        }
    }
    public Carriage(int trackMaxPosition, int nthCarriage)
    {
        _trackMaxPosition = trackMaxPosition;
        _nthCarriage = nthCarriage;

    }

}

