namespace Tests.Helpers;

/// <summary>
/// Stores base64-encoded samples for tests.
/// </summary>
public static class SampleBase64Data
{
    // Add your base64 samples here, keyed by a friendly name.
    private static readonly Dictionary<string, string> Samples = new(StringComparer.OrdinalIgnoreCase)
    {
        {
            "emoji1.png", "iVBORw0KGgoAAAANSUhEUgAAAEgAAABICAMAAABiM0N1AAAAh1BMVEVHcEz/zE3/zE3/zE3/zE3/zE3/zE3/zE3/zE3/zE3/zE3/zE3/zE3/zE1mRQDPojXZqjqziSd5Vgr1xEhwTQWfeB2zooCMZxPZ0b/su0OMdED18++DXg6DaDDisz/i3M+Wf1CWbxjs6N/////GmTC8kSupgCK8ro/GuZ/Pxa9wURCfi2B5XCBjMO3CAAAADXRSTlMA36/PgFCPIJ/vQL9g4e8akwAAAkFJREFUeF7tmNmu4jAMhmlLSwK1uy/sy1mXef/nmyH0RCImTfDM4Wq+GyQj/fJSy7EnI8zSZBHAQLBI0tmEQSwCIAQivk9llkiwIBN/v+YhjBLO/bwR4ER4eDWV4IGcutyJwJNo1KlYgjcyHgkL7sIanoA7ET46fCUBDIQrP/w8xcDEqN1MAhN53XoRsIlIgrhMSWDs4CyV538Dc/hL5oNQCCZZiVhm4GsPhwxRHVRkvna4ZCkBg6pGRV057JpECZGSFThQOOwaebs5Njiwcdk18c3ao2bETr+AYERo6bBrAlUzjke0bum/EUpJ8XnJhmSyAGCUn7BQuWZ8kDTboGC3iEYJ8ZuWCvHhC/0X2uXgpspJ2xrNX20QsTwW2RIsLLPiWCJiV123v9EiDWo23WvxkiuW6ueleO02qGmMFklIg/tiNG16FRlbKJ3MiNCpbw9btPLr0PYnJWQMpMAMbbt++sO+f2vblWK7utC2b/3+/N96awoF5uAvz0pfTw6+zjqlOf5jUjXlkwXtDzbk3SbpUP7s7Tr9Jx3cko7sEi8cTrdlTgdEJJEl9BHxjN+s3kmA6/cVfvN8XTNFaI4KrdV+7L9F9h/toEIHSXjrobVDL3bmQ0shaHB2aGDC8hht0Elje2lP4S6lxr5FRGSkWqCDNhpbIbIardQZXSHsS011RAtHY17HrjUr75CCXe5eRwUZA40RYN3suCtkXnQlKsquyH9uqRU/s67zDwgPOGkwjiwPOPswDlHs09hDj3X88+FvXybnvMjgRHsAAAAASUVORK5CYII="
        },
        {
            "emoji2.png", "iVBORw0KGgoAAAANSUhEUgAAAEgAAABICAMAAABiM0N1AAAAM1BMVEVHcEzdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkTdLkQVM82yAAAAEHRSTlMAIN+fzzBAEO+/j3Bgr1CAS8Xz9AAAAV5JREFUeF7l2M2OgzAMhdGbEOK4/N33f9rRMMISamgF9m7O1tKnNgmtAv6Vsdbx4dykSRt3bZlSb74cc7X5u7nwRFecrMqTMqOnZr7RClOVb7LNjbzY9To6V3PBiRReKPJ9bjA2Xipina42wkjmB0Ws05UFh8KPloUflaOz0WnDLtEt4ZfSTQFgZIARwMAAA4DGAA2oDFGxMcSGhSEWKEMoCkMUMAgaQ7SwNQoLxZ2jlSFWJIZIUQ9t5M/IzAAzAGEAAYCFbgt+rXRbsct0yvgzByz1ThpdGg4TXSaY7Fwhs3q3zKjzf9+kxodawsnkXWmj7i/mOkxN8KbygYqOjbdt6Bp404A+KbylCC5Idne+XyX6Vwd/yTrXJXfHSu6OlZwdI8WxXyei/Eh7nQdnfMANc+OFNuOWsbCrjLhJXhe38/tqvnpf4P1QL8FTSWk0waOqvSXxqgM5VEQQwb/2A80o0l7lU88FAAAAAElFTkSuQmCC"
        },
        {
            "emoji3.png", "iVBORw0KGgoAAAANSUhEUgAAAEgAAABICAMAAABiM0N1AAAAwFBMVEVHcExVrO6gBB5VrO6gBB5VrO6eCSNVrO5VrO7KVif/rDOgBB6gBB5VrO5VrO7/rDP/rDP/rDP/rDNVrO7/rDNVrO7/rDP/rDOgBB6gBB6gBB5VrO5VrO6gBB5VrO5/T3r/rDOSrKugBB5VrO7/rDObDytXouH/xUhUl9Jjjcf/zE2SJEV7WIZybJ8iRmD/uD01bJUFCw+qrJEQIC3KXDN/TnmMMVYAAABseK0VKzy1MCg9fa2EQ2yXGThogrrhjj6Wvyp4AAAAInRSTlMAz7+Y6t9ogBAQ7yhAt1hwpNggQDhwhb+fj88gMFDvj1jHfb8ZaQAAAvVJREFUeF7Vltly4jAQRVltSHDAZCMJmSSSvLDv2Zf//6uxosnFZVptE57mvFJ1qnVvS6Z0KDcnrleWUh4oqdal5iBRv6oHAb+d5dyMAsq/0px4Mou3v6WBYdK4e2vcP5Li5EANuDlMA+p7RbzNJgwzouoea4Om5i9PCS/zX0XUqP5o4tXTP1bx/hGd1uFZPIEFTOXC4wA9D1hhi4qOA96mbwnTn7F+cuoX8bhyl3BqRC/Fy294kmRuTEZ0XiBl6wqamYp2di6thAuIvGJtsSMx29i8FgmVBKcsOd502PaojxOPIRjIXNHcGvVxLd8DkVnIPw3CcwuPeJa5okVs22pHgLHMY7qKbQNdCTCiK/8MUxNZr1lPbKECit+F+JRpMFC2drAhPONACBFyXw/UDgKcAAxGIuE966k3iNq5pMdmJWL2K4TaQZxNZ2Sp0rPXTh1gEtiq7NO107sYvlurrJK1g4BIGclxSbdazlW6s8e05zmw7+gp+Xo4GGyS8mygD2LyYLSrlokixLGgJw5G0aokIngGgaVJHIxT1RDRRGwZkXeD504a0Dr5ynlFP4jhI71ZuPQFRYOPtGfDBMSIsD3UYhX+tDbvHxCz9VGp5lravlJLxGy7YuUcTbejNEt4LJs4cHjPhbbMouHa9qZgTStsNpfaEw2HERRE0KYHVuRrzzDhS6T5CIk3t5J3rkiL1oLcaDxOvOjsO5+h5pV5KzfCYBeZgCAiC4vH2FJ78cokpLEU9pxeCi4hiAjPYPKIYXiRT4nWy+WD67p3I5Hlmo06LXp9XX9FQ6Xa+tfajqfJdY+wQaRUV4taGU/tmO0M9YOZ8s3PlTwPUIoYKTIn2xnplt9GpASUOVk2Je7m3ysQpeZRHeI/xlXO+wFm36polniOungZ4OmVODqKpF0CvZwFQmkE+mDAwQKx+DYPaKJ4FkJzZJoH14yHE3W6xF/52zxPM2Px27tROPwCYR+B32l3SwQtLFCe6Ojy8qJ9Zm2l1Sv9f/wFVgV1FSZTuKYAAAAASUVORK5CYII="
        }
    };

    /// <summary>
    /// Gets a base64 sample by key or throws with a friendly message.
    /// </summary>
    public static string Get(string key)
    {
        if (TryGet(key, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"Base64 sample not found: {key}");
    }

    /// <summary>
    /// Attempts to get a base64 sample by key.
    /// </summary>
    public static bool TryGet(string key, out string value)
    {
        value = string.Empty;
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        if (Samples.TryGetValue(key, out var foundValue) && foundValue is not null)
        {
            value = foundValue;
            return true;
        }

        return false;
    }
}