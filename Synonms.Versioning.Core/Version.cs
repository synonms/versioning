namespace Synonms.Versioning.Core
{
    //public class Version
    //{
    //    public Version(uint majorVersion = 0u, uint minorVersion = 0u)
    //    {
    //        MajorVersion = majorVersion;
    //        MinorVersion = minorVersion;
    //    }

    //    public Version(string text)
    //    {
    //        var tokens = text.Trim(new[] { 'v' }).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

    //        uint.TryParse(tokens.Length > 0 ? tokens[0] : string.Empty, out var majorVersion);
    //        uint.TryParse(tokens.Length > 1 ? tokens[1] : string.Empty, out var minorVersion);

    //        MajorVersion = majorVersion;
    //        MinorVersion = minorVersion;
    //    }

    //    public uint MajorVersion { get; }
    //    public uint MinorVersion { get; }

    //    public bool IsUnspecified => MajorVersion == 0u && MinorVersion == 0u;

    //    public override bool Equals(object obj)
    //    {
    //        if (obj is Version version)
    //        {
    //            return Equals(version);
    //        }

    //        return false;
    //    }

    //    public override int GetHashCode()
    //    {
    //        unchecked
    //        {
    //            return ((int)MajorVersion * 397) ^ (int)MinorVersion;
    //        }
    //    }

    //    public override string ToString()
    //    {
    //        return $"v{MajorVersion}.{MinorVersion}";
    //    }

    //    public static bool operator <(Version lhs, Version rhs)
    //    {
    //        if (lhs.MajorVersion < rhs.MajorVersion) return true;
    //        if (lhs.MajorVersion > rhs.MajorVersion) return false;

    //        return lhs.MinorVersion < rhs.MinorVersion;
    //    }

    //    public static bool operator >(Version lhs, Version rhs)
    //    {
    //        if (lhs.MajorVersion < rhs.MajorVersion) return false;
    //        if (lhs.MajorVersion > rhs.MajorVersion) return true;

    //        return lhs.MinorVersion > rhs.MinorVersion;
    //    }

    //    public static bool operator <=(Version lhs, Version rhs)
    //    {
    //        if (lhs.MajorVersion < rhs.MajorVersion) return true;
    //        if (lhs.MajorVersion > rhs.MajorVersion) return false;

    //        return lhs.MinorVersion <= rhs.MinorVersion;
    //    }

    //    public static bool operator >=(Version lhs, Version rhs)
    //    {
    //        if (lhs.MajorVersion < rhs.MajorVersion) return false;
    //        if (lhs.MajorVersion > rhs.MajorVersion) return true;

    //        return lhs.MinorVersion >= rhs.MinorVersion;
    //    }

    //    public static bool TryParse(string text, out Version version)
    //    {
    //        version = new Version(text);

    //        return !version.IsUnspecified;
    //    }

    //    private bool Equals(Version other)
    //    {
    //        return MajorVersion == other.MajorVersion && MinorVersion == other.MinorVersion;
    //    }
    //}
}
