using System;
using SQLite;

public class Record : IComparable<Record>
{
    [PrimaryKey]
    public string Id { get; set; }
    public int Score { get; set; }
    public string Name { get; set; }
    
    public Record(){}

    public Record(int score, string name = null)
    {
        Ulid ulid = Ulid.NewUlid();
        Id = ulid.ToString();
        Score = score;
        Name = name;
    }

    public int CompareTo(Record other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Score.CompareTo(other.Score);
    }
}
