# Pyther.Core

Library that provides logging functionality and extensions methods for strings and collections.

## Logging

include namespace
```csharp
using Pyther.Core.Logging;
```

add two example loggers
```csharp
// a) Register a new console logger (with options)
Log.Register(new ConsoleLogger()
{
    MinLevel = LogLevel.Warning   // log Warning Level and above
});

// b) Register a new file logger (with options)
Log.Register(new FileLogger("path/to/file.log")
{
    AppendMode = true,            // append to exisiting file
    UseTimestamp = true,          // log timestamps
    MinLevel = LogLevel.Debug,    // log Debug Level and above
    Lock = true                   // make thread safe
});
```

log something
```csharp
// Errors should inform about critical problems that can't be resolved by the system. 
Log.Error("This is a critical error!");

// Warnings should inform that something went wrong but can be handled by the system.
Log.Warning("This is a warning!");

// Infos should be enabled to display optional, relevant informations.
Log.Info("This is an information.");

// Process logging should be used to notify about important processes/steps.
Log.Process("This is a process.");

// Debug logging can be part of the final release, but should only be enabled when required to find problems.
Log.Debug("This is a debug message.");

/// Temporary logging should only be used during development and should completely be removed on final releases.
/// This kind of logging will always be executed. No matter what Log Level was chosen.
Log.Temp("Just log something temporary ...");
```

that's it :o)

Of course, you can use interpolated strings to log content of variables.
```csharp
Log.Warning($"Sorry, I can`t divide {a} by {b}!");
```

This library comes with the following loggers:
- `ConsoleLogger` ... log to the console using *Console.Write*
- `ConsoleColoredLogger` ... log colored text to the console using *Console.Write*
- `DebugLogger` ... Debug logger using *Debug.Write*
- `NullLogger` ... simple no operation logger

It is easy to write your own logger ...
```csharp
/// <summary>
/// A logger that show log messages using windows MessageBox.
/// </summary>
public class MessageBoxLogger : BaseLogger
{
    public override void Log(LogLevel level, string message)
    {
        System.Windows.MessageBox.Show(message);
    }
}
```

... and let it only show error messages:
```csharp
Log.Register(new MessageBoxLogger()
{
    MinLevel = LogLevel.Error
});
```

## String extensions

```csharp
"abc".Repeat(3);  // result: "abcabcabc"
"abc".Reverse();  // result: "cba"
"Hello World".Clip(5);  // result: "Hello"
"abc".Base64Encode();  // result: "YWJj";
"YWJj".Base64Decode();  // result: "abc"

int? number = "123".Parse<int>();  // result: 123
FileMode? mode = "Append".Parse<FileMode>();  // result: FileMode.Append

FileMode? mode = "Append".ToEnum<FileMode>();  // result: FileMode.Append

DateTime? dateTime = "04/02/1981 10:51:17".ToDateTime();  // result: DateTime object

TimeSpan? timeSpan = "1:23:45".ToTimeSpan();  // result: TimeSpan object
 
```