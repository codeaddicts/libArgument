[![CC0](http://i.creativecommons.org/p/zero/1.0/88x31.png)](http://creativecommons.org/publicdomain/zero/1.0/)  
To the extent possible under law, [Codeaddicts](https://github.com/codeaddicts) has waived all copyright and related or neighboring rights to Codeaddicts.libArgument.

# libArgument
libArgument is a .NET library for handling command-line arguments like a boss.  
It allows you to validate arguments on-the-fly and saves you hours of work.

If you want to test the latest features, please download the source and compile it yourself.  
The release may not be up-to-date.

Features:

* The generic parsing method works with any class
* Boolean switches with the 'Switch' attribute
* Arguments with parameters using the 'Argument' attribute
* Automatically casts the argument to the type of the variable
* Automatically infers the argument name from the variable name if no argument name is given
* Allows you to use multiple argument or switch names for one variable

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
	[Argument ("input")]
	public string Input;
    
    // -o path/to/file
    // --output path/to/file
    [Argument ("o", "output")]
    public string Output;

    // --test "Test"
    // --woop "Test"
    [Argument ("test")]
    [Argument ("woop")]
    public string Test;

    // --num 123
    [Argument ("num")]
    public UInt64 ANumber;
    
    // --log
    [Switch]
    public bool log;

    // --enable-something
    [Switch ("enable-something")]
    public bool Something;

    // -a
    // --annoyme
    [Switch ("a", "annoyme")]
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
