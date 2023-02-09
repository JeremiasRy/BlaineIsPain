namespace Types;
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
    public bool CollisionCheck()
    {
        foreach (var train in TrainsOnTrack)
        {
            if (train.Positions.Select(pos => Track[pos]).GroupBy(coordinates => new { coordinates.X, coordinates.Y }).Count() < train.Positions.Count)
            {
                return true;
            }
        }
        return TrainsOnTrack[0].Positions.Any(aPos => TrainsOnTrack[1].Positions.Any(bPos => Track[aPos] == Track[bPos]));
    }
    public TrainTrack(Dictionary<int, TrainTrackPiece> track, Train trainA, Train trainB)
    {
        Track = track;
        TrainsOnTrack = new Train[] {trainA, trainB};
    }
}

