# XzaarScript [![Build Status](https://travis-ci.org/zerratar/XzaarScript.svg?branch=master)](https://travis-ci.org/zerratar/XzaarScript)

## Introduction
If Rust and TypeScript had a baby, this might be it. It's ugly, it's incomplete, it's buggy. But I love it! Originally created for a game I was making in Unity. But it took too long to develop that I didn't have time to finish it. Now here it is, free for everyone to use!

XzaarScript is written in C#, .NET Framework v4.6.1 using Visual Studio 2017

You can test out the demo website [here](http://xzaarscript.shinobytes.com)

## License
XzaarScript is licensed under the [GPL, version 3.0](https://www.gnu.org/licenses/gpl-3.0.en.html). See [gpl-licence.txt](https://github.com/zerratar/PapyrusDotNet/blob/master/gpl-license.txt) for details.

## Build
Open up the `XzaarScript.sln` in visual studio, press build. Voilà!

## Documentation
### Code samples

You can test these out on the demo website [here](http://xzaarscript.shinobytes.com)

#### Well, hello world!
```rust
let console = $console

fn print_hello_world() -> void {
   console.log("Hello World")
}

print_hello_world()
```

#### Looping that magical array
```rust
let console = $console

foreach (let item in ["hello",", ", "world!"]) {
    console.log(item);
}

// or just loop
loop {
    console.log("helloooo!")
    break; 
    // yeah, you can mix with and without semicolons, 
    // although I would recommend if you use it as it will make it easier for the 
    // compiler to know when the line ends. See it as an performance optimization
}

// or do while
do {
    console.log("wooooorld!!!");
    break; // don't continue.. 
} while(true);

// or just while
while (true) {
    console.log("I'm totally spamming");
}

// and why not the typical for loop?
for (let i = 0; i < 100; i++) {
    console.log("A hundred times");
}
```

#### using structs
```rust
let console = $console;

// structs can only hold fields, no default values yet either
// classes are not available yet either. 

struct myStruct {
    myFieldA : str,
    myFieldB : i32
}

/* instantiate the struct, no need for a 'new' keyword here */
let theStruct = myStruct {
    myFieldA = "hello",
    myFieldB = 123
};

console.log(theStruct.myFieldA + theStruct.myFieldB);
```

To be updated with more soon!

## History
To be updated