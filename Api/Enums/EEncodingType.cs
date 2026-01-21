namespace Api.Enums;

public enum EEncodingType
{
    PNG,
    JPG
}

public static class EncodingTypeExtension
{
    public static string[] PossibleContentTypes(this EEncodingType encodingType) => encodingType switch
    {
        EEncodingType.PNG => ["image/png"],
        EEncodingType.JPG => ["image/jpg", "image/jpeg"],
        _ => throw new NotImplementedException()
    };
}