using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using SQLite;

[Serializable]
public class Record : IComparable<Record>, IEquatable<Record>
{
    [PrimaryKey]
    public string id { get; set; }
    public int Score { get; set; }
    public string Name { get; set; }
    [JsonConverter(typeof(DateTimeJsonConverter))]
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonConverter(typeof(DateTimeJsonConverter))]
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
    
    public Record(){}

    public Record(int score, string name = null)
    {
        Ulid ulid = Ulid.NewUlid();
        id = ulid.ToString();
        Score = score;
        Name = name ?? string.Empty;
    }

    public bool IsRecordNameNull()
    {
        return string.IsNullOrEmpty(Name);
    }

    public int CompareTo(Record other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Score.CompareTo(other.Score);
    }

    public bool Equals(Record other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Score == other.Score;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Record)obj);
    }

    public override int GetHashCode()
    {
        return Score;
    }
}


public class ResponseRankedIn
{
    [JsonPropertyName("rankin")]
    public bool Rankin { get; set; }
}

[Serializable]
public class ResponseRanking
{
    [JsonPropertyName("records")]
    public List<Record> Records { get; set; }
}