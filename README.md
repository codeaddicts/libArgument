# libArgument
libArgument is a .NET library for handling command-line arguments like a boss.  
It allows you to validate arguments on-the-fly and saves you hours of work.

Features:

* The generic parsing method works with any class
* Create boolean switches with the 'Switch' attribute
* Create arguments with parameters using the 'Argument' attribute
* Cast argument texts to int32, int64, bool, float etc. using the 'CastAs' attribute

And here's some code!

```cs
using Codeaddicts.libArgument;
using Codeaddicts.libArgument.Attributes;

// A simple class with some command-line options
public class MyOptions
{
	// -i path/to/file
    // --input path/to/file
	[Argument ("i", "input")]
	public string Input;
    
    // -o path/to/file
    // --output path/to/file
    [Argument ("o", "output")]
    public string Output;
    
    // --log
    [Switch ("", "log")]
    public bool EnableLog;
    
    // --num 123
    [Argument ("", "num")]
    [CastAs (CastingType.UInt64)]
    public UInt64 ANumber;
}

public class Program
{
	public static void Main (string[] args) {
    	
        // This is where the magic happens
        var options = ArgumentParser.Parse<MyOptions> (args);
        
        // Now you can access the fields!
        Console.WriteLine ("Input file: " + options.Input);
        Console.WriteLine ("Output file: " + options.Output);
        Console.WriteLine ("Your number: " + options.ANumber);
    }
}
```

Now call the application like that:  
$ MyApp.exe -i test -o test --log --num 123 or  
$ MyApp.exe --input test --output test --log --num 123

## How about optional arguments?
Absolutely no problem!

Just add a parameterless constructor to your Options class and initialize  
your optional variables there. It's a piece of cake!