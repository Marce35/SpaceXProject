namespace SpaceXProject.api.Shared.Base.Error.SpaceXApiErrors;

public static class SpaceXApiErrors
{

    public static Error ApiError(string error) => new(
        code: "SpaceX_ApiError",
        messages: [$"SpaceX API Error: {error}"]
    );

    public static Error SerializationError() => new(
        code: "SpaceX_SerializationError",
        messages: ["Received empty response from SpaceX"]
    );
}
