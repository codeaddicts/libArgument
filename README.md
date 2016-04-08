[![](https://img.shields.io/nuget/v/Codeaddicts.libArgument.svg)](https://www.nuget.org/packages/Codeaddicts.libArgument)
[![](https://img.shields.io/github/license/codeaddicts/libargument.svg)](https://creativecommons.org/publicdomain/zero/1.0/)
[![](https://img.shields.io/github/issues/codeaddicts/libargument.svg)](https://github.com/codeaddicts/libArgument/issues)

To the extent possible under law, [Codeaddicts](https://github.com/codeaddicts) has waived all copyright and related or neighboring rights to Codeaddicts.libArgument.

# libArgument
libArgument is a .NET library for handling command-line arguments like a boss.  
It allows you to validate arguments on-the-fly and saves you hours of work.

If you want to test the latest features, please download the source and compile it yourself.  
The release may not be up-to-date.

You can always get the latest stable release from nuget.

Features:

* The generic parsing method works with any class
* Supports primitive type conversions
* Supports enum type conversions
* Supports one-dimensional array type conversions
* Supports Windows-style arguments: `/arg value`
* Supports POSIX style switches and arguments: `-a -b value`
* Supports POSIX style merged switches: `-abc` equals `-a -b -c`
* Supports GNU long-style arguments: `--arg=value`
* Supports arbitrarily many argument names for any variable
* Styles can be mixed: `-a --arg1 value /arg2 value --arg3=value`
* Automatically casts the argument to the correct type
* Automatically infers the argument name from the variable name if no argument name is given

## How does it work?
It's really easy.  
Here's a small example:

```cs
using Codeaddicts.libArgument;
using Codeaddicts.libArgument.Attributes;

// A simple class with some command-line options
public class MyOptions {
	[Argument] public string input;
	[Argument] public string output;
	[Switch] public bool verbose;
}

public class Program {
	public static void Main (string[] args) {
		// Let the magic happen
		var options = ArgumentParser.Parse<MyOptions> (args);

		// That's it! Now you can use the variables
		Console.WriteLine ("Input: {0}", options.input);
		Console.WriteLine ("Output: {0}", options.output);
		Console.WriteLine ("Verbose: {0}", options.verbose ? "yes" : "no");
	}
}
```

Call the program like that:  
$ MyApp.exe --input "path/to/input" --output "path/to/output" --verbose

## Can I do more advanced stuff?
Sure! You can do pretty much everything :P  
Here's some more advanced code for you.

```cs
using Codeaddicts.libArgument;
using Codeaddicts.libArgument.Attributes;

// A simple class with some command-line options
public class MyOptions
{
	// --str "Hello, World!"
	[Argument]
	public string str;

    // --input path/to/file
	[Argument ("--input")]
	public string Input;
    
    // -o path/to/file
    // --output path/to/file
    [Argument ("-o", "--output")]
    public string Output;

    // --test "Test"
    // --woop "Test"
    [Argument ("--test")]
    [Argument ("--woop")]
    public string Test;

    // --num 123
    [Argument ("--num")]
    public UInt64 ANumber;
    
    // --log
    [Switch]
    public bool log;

    // --enable-something
    [Switch ("--enable-something")]
    public bool Something;

    // -a
    // --annoyme
    [Switch ("-a", "--annoyme")]
    public bool AnotherSwitch;
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
        ...
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
