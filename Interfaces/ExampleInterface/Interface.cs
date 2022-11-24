using System;

/// <summary>
/// This shows how an interface should work
/// Checkout https://github.com/Loki-Hornsby/UME.Broforce for a more in depth example of usage
/// </summary>

public static class ExampleInterface {
    /// <summary> <Required>
    /// Returns the info for our interface
    /// </summary>
    public static List<string> GetInfo(){
        return new List<string>() {
            "Example",
            "Cheese.dll",
            "UniversalModEngine\Engine\Interfaces\ExampleInterface" // Todo: find a better way to grab this instead of user having to upload it
        };
    }

    /// <summary> <Optional>
    /// Shows extra messages in the console
    /// </summary>
    public static List<CustomMessage> GetMessages(){
        return new List<CustomMessage>() {
            new CustomMessage(1, "Hi!"),
            new CustomMessage(3, "This can only be seen in high verbosity!"),
            new CustomMessage(1, "This is an example interface!"),
        };
    }
}