using BlaineIsPain;

string track = "    /---------------------\\               /-\\ /-\\  \n   //---------------------\\\\              | | | |  \n  //  /-------------------\\\\\\             | / | /  \n  ||  |/------------------\\\\\\\\            |/  |/   \n  ||  ||                   \\\\\\\\           ||  ||   \n  \\\\  ||                   | \\\\\\          ||  ||   \n   \\\\-//                   | || \\---------/\\--/|   \n/-\\ \\-/                    \\-/|                |   \n|  \\--------------------------/                |   \n\\----------------------------------------------/   \n";
string track2 = "                                /------------\\             \n/-------------\\                /             |             \n|             |               /              S             \n|             |              /               |             \n|        /----+--------------+------\\        |\n\\       /     |              |      |        |             \n \\      |     \\              |      |        |             \n |      |      \\-------------+------+--------+---\\         \n |      |                    |      |        |   |         \n \\------+--------------------+------/        /   |         \n        |                    |              /    |         \n        \\------S-------------+-------------/     |         \n                             |                   |         \n/-------------\\              |                   |         \n|             |              |             /-----+----\\    \n|             |              |             |     |     \\   \n\\-------------+--------------+-----S-------+-----/      \\  \n              |              |             |             \\ \n              |              |             |             | \n              |              \\-------------+-------------/ \n              |                            |               \n              \\----------------------------/               ";
static int TrainCrash(string track, string aTrain, int aTrainPos, string bTrain, int bTrainPos, int limit)
{
    var mappedTrack = MapTrack(track);
    var maxPos = mappedTrack.Count - 1;
    var trainTrack = new TrainTrack(mappedTrack, new Train(aTrain, aTrainPos, maxPos), new Train(bTrain, bTrainPos, maxPos));
    int rounds = 0;

    Console.CursorVisible = false;

    ScreenBuffer.Initialize();
    while (rounds < limit)
    {
        Thread.Sleep(100);
        trainTrack.ClearTrack();
        foreach (var train in trainTrack.TrainsOnTrack)
        {
            train.Move();
            foreach (int position in train.Positions)
            {
                trainTrack.Track[position].Occupy(train._piece);
            }
        }
        trainTrack.Draw();
        ScreenBuffer.DrawScreen();
        rounds++;
    }
    return 0;
}
static Dictionary<int, TrainTrackPiece> MapTrack(string track)
{
    var result = new Dictionary<int, TrainTrackPiece>();

    string[] rows = track.Split('\n');
    int trackPosition = 0;
    int startY = 0;
    int startX = rows[0].IndexOf(item => item != ' ');
    int y = 0;
    int x = startX + 1;
    
    result.Add(trackPosition++, new TrainTrackPiece(startY, startX, rows[startY][startX])); //Initialize zero position
    Direction direction = Direction.Right;
    Direction edgeOfTrack = Direction.None;

    bool complete = startY == y && startX == x;

    while (!complete)
    {
        var newPiece = new TrainTrackPiece(y, x, rows[y][x]);
        TrainTrackPiece? oldpiece = null;
        foreach (var keyValue in result)
        {
            if (keyValue.Value == newPiece)
            {
                oldpiece = keyValue.Value;
                break;
            }
        }
        result.Add(trackPosition++, oldpiece is not null ? oldpiece : newPiece);
        oldpiece = null;

        edgeOfTrack = x + 1 > rows[y].Length - 1 ? edgeOfTrack | Direction.Right : edgeOfTrack &= ~Direction.Right;
        edgeOfTrack = x - 1 < 0 ? edgeOfTrack | Direction.Left : edgeOfTrack &= ~Direction.Left;
        edgeOfTrack = y + 1 > rows.Length - 1 ? edgeOfTrack | Direction.Down : edgeOfTrack &= ~Direction.Down;
        edgeOfTrack = y - 1 < 0 ? edgeOfTrack | Direction.Up : edgeOfTrack &= ~Direction.Up;

        switch (rows[y][x])
        {
            case '\\': 
                {
                    switch (direction)
                    {
                        case Direction.Right:
                            {
                                if ((edgeOfTrack & Direction.Right) == Direction.Right || rows[y + 1][x] == '|')
                                {
                                    direction = Direction.Down;
                                } else if (rows[y + 1][x + 1] == '\\' || rows[y + 1][x + 1] == 'X')
                                {
                                    direction |= Direction.Down;
                                }
                            } break;
                        case Direction.Left:
                            {
                                if ((edgeOfTrack & Direction.Left) == Direction.Left || rows[y - 1][x] == '|')
                                {
                                    direction = Direction.Up;
                                } else if (rows[y - 1][x - 1] == '\\' || rows[y - 1][x - 1] == 'X')
                                {
                                    direction |= Direction.Up;
                                }
                            } break;
                        case Direction.Up:
                            {
                                if ((edgeOfTrack & Direction.Up) == Direction.Up || rows[y][x - 1] == '-') 
                                {
                                    direction = Direction.Left;
                                } else if (rows[y - 1][x - 1] == '\\' || rows[y - 1][x - 1] == 'X')
                                {
                                    direction |= Direction.Left;
                                }
                            } break;
                        case Direction.Down:
                            {
                                if ((edgeOfTrack & Direction.Down) == Direction.Down || rows[y][x + 1] == '-')
                                {
                                    direction = Direction.Right;
                                } else if (rows[y + 1][x + 1] == '\\' || rows[y + 1][x + 1] == 'X')
                                {
                                    direction |= Direction.Right;
                                }
                            } break;
                        case Direction.Right | Direction.Down:
                            {
                                if ((edgeOfTrack & Direction.Right) == Direction.Right || rows[y + 1][x] == '|' || rows[y + 1][x] == '+')
                                {
                                    direction = Direction.Down;
                                }
                                else if ((edgeOfTrack & Direction.Down) == Direction.Down || rows[y][x + 1] == '-' || rows[y][x + 1] == '+')
                                {
                                    direction = Direction.Right;
                                }
                            }break;
                        case Direction.Left | Direction.Up:
                            {
                                if ((edgeOfTrack & Direction.Left) == Direction.Left || rows[y - 1][x] == '|' || rows[y - 1][x] == '+')
                                {
                                    direction = Direction.Up;
                                }
                                else if ((edgeOfTrack & Direction.Up) == Direction.Up || rows[y][x - 1] == '-' || rows[y][x - 1] == '+')
                                {
                                    direction = Direction.Left;
                                }
                            } break;
                    }
                } break;
            case '/':
                {
                    switch (direction)
                    {
                        case Direction.Right:
                            {
                                if ((edgeOfTrack & Direction.Right) == Direction.Right || rows[y - 1][x] == '|')
                                {
                                    direction = Direction.Up;
                                } else if (rows[y - 1][x + 1] == '/' || rows[y - 1][x + 1] == 'X')
                                {
                                    direction |= Direction.Up;
                                }
                            } break;
                        case Direction.Left:
                            {
                                if ((edgeOfTrack & Direction.Left) == Direction.Left || rows[y + 1][x] == '|')
                                {
                                    direction = Direction.Down;
                                } else if (rows[y + 1][x - 1] == '/' || rows[y + 1][x - 1] == 'X')
                                {
                                    direction |= Direction.Down;
                                }
                            } break;
                        case Direction.Up:
                            {
                                if ((edgeOfTrack & Direction.Up) == Direction.Up || rows[y][x + 1] == '-')
                                {
                                    direction = Direction.Right;
                                } else if (rows[y - 1][x + 1] == '/' || rows[y - 1][x + 1] == 'X')
                                {
                                    direction |= Direction.Right;
                                } 
                            } break;
                        case Direction.Down:
                            {
                                if ((edgeOfTrack & Direction.Down) == Direction.Down || rows[y][x - 1] == '-')
                                {
                                    direction = Direction.Left;
                                } else if (rows[y + 1][x - 1] == '/' || rows[y + 1][x - 1] == 'X')
                                {
                                    direction |= Direction.Left;
                                }
                            } break;
                        case Direction.Right | Direction.Up: 
                            {
                                if ((edgeOfTrack & Direction.Up) == Direction.Up || rows[y][x + 1] == '-' || rows[y][x + 1] == '+')
                                {
                                    direction = Direction.Right;
                                } else if ((edgeOfTrack & Direction.Right) == Direction.Right || rows[y - 1][x] == '|' || rows[y - 1][x] == '+')
                                {
                                    direction = Direction.Up;
                                }
                            } break;
                        case Direction.Left | Direction.Down:
                            {
                                if ((edgeOfTrack & Direction.Left) == Direction.Left || rows[y + 1][x] == '|' || rows[y + 1][x] == '+')
                                {
                                    direction = Direction.Down;
                                } else if ((edgeOfTrack & Direction.Down) == Direction.Down || rows[y][x - 1] == '-' || rows[y][x - 1] == '+')
                                {
                                    direction = Direction.Left;
                                }
                            } break;
                    } 
                } break;
        }
        switch (direction)
        {
            case Direction.Right:
                {
                    x++;
                } break;
            case Direction.Left:
                {
                    x--;
                } break;
            case Direction.Up: 
                {
                    y--;
                } break;
            case Direction.Down:
                {
                    y++;
                } break;
            case Direction.Up | Direction.Right:
                {
                    y--;
                    x++;
                } break;
            case Direction.Up | Direction.Left:
                {
                    y--;
                    x--;
                } break;
            case Direction.Down | Direction.Right:
                {
                    y++;
                    x++;
                } break;
            case Direction.Down | Direction.Left:
                {
                    y++;
                    x--;
                } break;
        }
        complete = startY == y && startX == x;
    }
    return result;
}

TrainCrash(track, "xX", 188, "Dd", 113, 2000);
TrainCrash(track2, "Aaaa", 147, "Bbbbbbbbbbb", 288, 1000);

[Flags]
public enum Direction
{
    None = 0,
    Up = 1,
    Down = 1 << 1,
    Left = 1 << 2,
    Right = 1 << 3,
}
public enum TrainDirection
{
    Clockwise,
    CounterClockwise
}
public static class Extension
{
    public static int IndexOf<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
    {
        int i = 0;
        foreach (TSource item in source)
        {
            if (predicate(item))
            {
                return i;
            }
            i++;
        }
        return -1;
    }
}

public class TrainTrack
{
    public Dictionary<int, TrainTrackPiece> Track { get; init; }
    public Train[] TrainsOnTrack { get; init; }
    public void Draw()
    {
        foreach (var KVpair in Track)
        {
            KVpair.Value.Draw();
        }
    }
    public void ClearTrack()
    {
        foreach (var KVpair in Track)
        {
            KVpair.Value.UnOccupy();
        }
    }
    public TrainTrack(Dictionary<int, TrainTrackPiece> track, Train trainA, Train trainB)
    {
        Track = track;
        TrainsOnTrack = new Train[] {trainA, trainB};
    }
}
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

public class Train
{
    readonly int _trackMaxPosition;
    public char _piece;
    int _position;
    readonly TrainDirection _direction;
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
        _piece = char.ToUpper(train[0]);
        for (int i = 0; i < train.Length - 1; i++)
        {
            Carriage[i] = new Carriage(trackMaxPosition, _direction == TrainDirection.CounterClockwise ? i + 1 : 0 - (i + 1)); 
        }
    }


}
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

