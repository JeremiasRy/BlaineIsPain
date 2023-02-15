using static BlaineIsPain.ScreenBuffer;
using Types;

string track = "    /------S--------------\\               /-\\ /-\\  \n   //---------------------\\\\              | | | |  \n  //  /-------------------\\\\\\             | / | /  \n  ||  |/------------------\\\\\\\\            |/  |/   \n  ||  ||                   \\\\\\\\           ||  ||   \n  \\\\  ||                   | \\\\\\          ||  ||   \n   \\\\-//                   | || \\---------/\\--/|   \n/-\\ \\-/                    \\-/|                |   \n|  \\--------------------------/                |   \n\\----------------------------------------------/   \n";
string track2 = "                                /------------\\             \n/-------------\\                /             |             \n|             |               /              S             \n|             |              /               |             \n|        /----+--------------+------\\        |\n\\       /     |              |      |        |             \n \\      |     \\              |      |        |             \n |      |      \\-------------+------+--------+---\\         \n |      |                    |      |        |   |         \n \\------+--------------------+------/        /   |         \n        |                    |              /    |         \n        \\------S-------------+-------------/     |         \n                             |                   |         \n/-------------\\              |                   |         \n|             |              |             /-----+----\\    \n|             |              |             |     |     \\   \n\\-------------+--------------+-----S-------+-----/      \\  \n              |              |             |             \\ \n              |              |             |             | \n              |              \\-------------+-------------/ \n              |                            |               \n              \\----------------------------/               ";
string track3 = "/----\\     /----\\ \n|     \\   /     | \n|      \\ /      | \n|       S       | \n|      / \\      | \n|     /   \\     | \n\\----/     \\----/";
string track4 = "/-------\\ \n|       | \n|       | \n|       | \n\\-------+--------\\\n        |        |\n        S        |\n        |        |\n        \\--------/";
string track5 = "/------S----------\\\n|                 |\n|                 |\n|                 |\n|                 |\n\\----------S------/";

static int TrainCrash(string track, string aTrain, int aTrainPos, string bTrain, int bTrainPos, int limit)
{
    var mappedTrack = MapTrack(track);
    var maxPos = mappedTrack.Count - 1;
    var trainTrack = new TrainTrack(mappedTrack, new Train(aTrain, aTrainPos, maxPos), new Train(bTrain, bTrainPos, maxPos));
    int rounds = 0;

    if (trainTrack.Track[aTrainPos].GetOriginalPiece() == 'S')
    {
        trainTrack.TrainsOnTrack[0].WaitTimer = 1;
    }
    if (trainTrack.Track[bTrainPos].GetOriginalPiece() == 'S')
    {
        trainTrack.TrainsOnTrack[1].WaitTimer = 1;
    }
    foreach (var train in trainTrack.TrainsOnTrack)
    {
        foreach (int position in train.Positions)
        {
            trainTrack.Track[position].Occupy(train.Piece);
        }
    }
    Initialize();
    trainTrack.Draw();
    DrawScreen();
    if (trainTrack.CollisionCheck())
    {
        return 0;
    }

    Console.CursorVisible = false;
    Offset = 5;
    while (rounds < limit)
    {
        Thread.Sleep(60);
        DrawText($"Round: {rounds}, Limit: {limit}", 0, 0);
        trainTrack.ClearTrack();
        foreach (var train in trainTrack.TrainsOnTrack)
        {
            if (train.WaitTimer != 0)
            {
                train.WaitTimer--;
                foreach (int position in train.Positions)
                {
                    trainTrack.Track[position].Occupy(train.Piece);
                }
                continue;
            }
            train.Move();
            if (trainTrack.CollisionCheck())
            {
                return rounds;
            }
            if (trainTrack.Track[train.Positions[0]].Piece == 'S' && !train.IsExpress)
            {
                train.WaitTimer = train.Positions.Count - 1;
            }
            foreach (int position in train.Positions)
            {
                trainTrack.Track[position].Occupy(train.Piece);
            }
        }
        trainTrack.Draw();
        DrawScreen();
        rounds++;
        if (trainTrack.CollisionCheck())
        {
            return rounds;
        }
    }
    return rounds > limit ? -1 : rounds;
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
                                if ((edgeOfTrack & Direction.Down) == Direction.Down)
                                {
                                    direction = Direction.Right;
                                } else if ((edgeOfTrack & Direction.Right) == Direction.Right) 
                                {
                                    direction = Direction.Down;
                                } else if (rows[y + 1][x] == '|' || rows[y + 1][x] == '+')
                                {
                                    direction = Direction.Down;
                                } else if (rows[y][x + 1] == '-' || rows[y][x + 1] == '+')
                                {
                                    direction = Direction.Right;
                                }
                            }break;
                        case Direction.Left | Direction.Up:
                            {
                                if ((edgeOfTrack & Direction.Up) == Direction.Up)
                                {
                                    direction = Direction.Left;
                                } else if ((edgeOfTrack & Direction.Left) == Direction.Left)
                                {
                                    direction = Direction.Up;
                                }
                                else if (rows[y - 1][x] == '|' || rows[y - 1][x] == '+')
                                {
                                    direction = Direction.Up;
                                }
                                else if (rows[y][x - 1] == '-' || rows[y][x - 1] == '+')
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
                                if ((edgeOfTrack & Direction.Up) == Direction.Up)
                                {
                                    direction = Direction.Right;
                                } 
                                else if ((edgeOfTrack & Direction.Right) == Direction.Right)
                                {
                                    direction = Direction.Up;
                                }
                                else if (rows[y - 1][x] == '|' || rows[y - 1][x] == '+')
                                {
                                    direction = Direction.Up;
                                }
                                else if (rows[y][x + 1] == '-' || rows[y][x + 1] == '+')
                                {
                                    direction = Direction.Right;
                                }
                            } break;
                        case Direction.Left | Direction.Down:
                            {
                                if ((edgeOfTrack & Direction.Down) == Direction.Down)
                                {
                                    direction = Direction.Left;
                                } 
                                else if ((edgeOfTrack & Direction.Left) == Direction.Left)
                                {
                                    direction = Direction.Down;
                                }
                                else if (rows[y + 1][x] == '|' || rows[y + 1][x] == '+')
                                {
                                    direction = Direction.Down;
                                }
                                else if (rows[y][x - 1] == '-' || rows[y][x - 1] == '+')
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
TrainCrash(track5, "ooO", 2, "Uuu", 10, 100);
TrainCrash(track4, "Aaa", 6, "Bbb", 1, 100);
TrainCrash(track3, "Aa", 12, "bB", 16, 100);
TrainCrash(track, "xX", 188, "Dd", 113, 2000);
TrainCrash(track2, "Aaaa", 147, "Bbbbbbbbbbbbbb", 288, 1000);



