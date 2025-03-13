using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser]
public class ValueObjectBenchmarks
{
    private readonly UserIdRecord _userIdRecord;
    private readonly UserIdStruct _userIdStruct;
    private readonly AgeRecord _ageRecord;
    private readonly AgeStruct _ageStruct;

    public ValueObjectBenchmarks()
    {
        _userIdRecord = new UserIdRecord(Guid.CreateVersion7());
        _userIdStruct = new UserIdStruct(Guid.CreateVersion7());
        _ageRecord = new AgeRecord(42);
        _ageStruct = new AgeStruct(42);
    }

    [Benchmark]
    public UserIdRecord CreateUserIdRecord()
    {
        return new UserIdRecord(Guid.CreateVersion7());
    }

    [Benchmark]
    public UserIdStruct CreateUserIdStruct()
    {
        return new UserIdStruct(Guid.CreateVersion7());
    }

    [Benchmark]
    public string SerializeUserIdRecord()
    {
        return JsonSerializer.Serialize(_userIdRecord);
    }

    [Benchmark]
    public string SerializeUserIdStruct()
    {
        return JsonSerializer.Serialize(_userIdStruct);
    }

    [Benchmark]
    public string SourceGeneratedSerializeUserIdRecord()
    {
        return JsonSerializer.Serialize(_userIdRecord, UserIdJsonContext.Default.UserIdRecord);
    }

    [Benchmark]
    public string SourceGeneratedSerializeUserIdStruct()
    {
        return JsonSerializer.Serialize(_userIdStruct, UserIdJsonContext.Default.UserIdStruct);
    }

    [Benchmark]
    public AgeRecord CreateAgeRecord()
    {
        return new AgeRecord(42);
    }

    [Benchmark]
    public AgeStruct CreateAgeStruct()
    {
        return new AgeStruct(42);
    }

    [Benchmark]
    public string SerializeAgeRecord()
    {
        return JsonSerializer.Serialize(_ageRecord);
    }

    [Benchmark]
    public string SerializeAgeStruct()
    {
        return JsonSerializer.Serialize(_ageStruct);
    }

    [Benchmark]
    public string SourceGeneratedSerializeAgeRecord()
    {
        return JsonSerializer.Serialize(_ageRecord, AgeJsonContext.Default.AgeRecord);
    }

    [Benchmark]
    public string SourceGeneratedSerializeAgeStruct()
    {
        return JsonSerializer.Serialize(_ageStruct, AgeJsonContext.Default.AgeStruct);
    }
}


public sealed record UserIdRecord
{
    public Guid Value { get; }

    public UserIdRecord(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("UserIdRecord cannot be empty", nameof(value));
        }
        Value = value;
    }

    public static implicit operator Guid(UserIdRecord identityId) => identityId.Value;

    public static bool operator ==(UserIdRecord left, string right) => left.Equals(right);

    public static bool operator !=(UserIdRecord left, string right) => !left.Equals(right);

    public static bool operator ==(string left, UserIdRecord right) => right.Equals(left);

    public static bool operator !=(string left, UserIdRecord right) => !right.Equals(left);
}


public readonly struct UserIdStruct : IEquatable<UserIdStruct>
{
    public UserIdStruct(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty", nameof(value));
        }

        Value = value;
    }

    public readonly Guid Value { get; }

    public static implicit operator Guid(UserIdStruct identityId) => identityId.Value;

    public static bool operator ==(UserIdStruct left, UserIdStruct right) => left.Equals(right);

    public static bool operator !=(UserIdStruct left, UserIdStruct right) => !left.Equals(right);

    public static bool operator ==(UserIdStruct left, string right) => left.Equals(right);

    public static bool operator !=(UserIdStruct left, string right) => !left.Equals(right);

    public static bool operator ==(string left, UserIdStruct right) => right.Equals(left);

    public static bool operator !=(string left, UserIdStruct right) => !right.Equals(left);

    public readonly bool Equals(UserIdStruct other)
    {
        return Value == other.Value;
    }

    public readonly bool Equals(Guid primitive) => Value == primitive;

    public override readonly bool Equals(object? obj)
        => obj is UserIdStruct other && Equals(other);

    public override readonly int GetHashCode() => Value.GetHashCode();

    public override readonly string ToString() => Value.ToString();
}


public sealed record AgeRecord
{
    public int Value { get; }

    public AgeRecord(int value)
    {
        if (value is not >= 1 and <= 100)
        {
            throw new ArgumentException("Age must be between 1 and 100", nameof(value));
        }
        Value = value;
    }

    public static implicit operator int(AgeRecord age) => age.Value;

    public static bool operator ==(AgeRecord left, string right) => left.Equals(right);

    public static bool operator !=(AgeRecord left, string right) => !left.Equals(right);

    public static bool operator ==(string left, AgeRecord right) => right.Equals(left);

    public static bool operator !=(string left, AgeRecord right) => !right.Equals(left);
}


public readonly struct AgeStruct : IEquatable<AgeStruct>
{
    public AgeStruct(int value)
    {
        if (value is not >= 1 and <= 100)
        {
            throw new ArgumentException("Age must be between 1 and 100", nameof(value));
        }

        Value = value;
    }

    public readonly int Value { get; }

    public static implicit operator int(AgeStruct identityId) => identityId.Value;

    public static bool operator ==(AgeStruct left, AgeStruct right) => left.Equals(right);

    public static bool operator !=(AgeStruct left, AgeStruct right) => !left.Equals(right);

    public static bool operator ==(AgeStruct left, string right) => left.Equals(right);

    public static bool operator !=(AgeStruct left, string right) => !left.Equals(right);

    public static bool operator ==(string left, AgeStruct right) => right.Equals(left);

    public static bool operator !=(string left, AgeStruct right) => !right.Equals(left);

    public readonly bool Equals(AgeStruct other)
    {
        return Value == other.Value;
    }

    public readonly bool Equals(int primitive) => Value == primitive;

    public override readonly bool Equals(object? obj) => obj is AgeStruct other && Equals(other);

    public override readonly int GetHashCode() => Value.GetHashCode();

    public override readonly string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}



[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(UserIdRecord))]
[JsonSerializable(typeof(UserIdStruct))]
internal partial class UserIdJsonContext : JsonSerializerContext
{
}


[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(AgeRecord))]
[JsonSerializable(typeof(AgeStruct))]
internal partial class AgeJsonContext : JsonSerializerContext
{
}
