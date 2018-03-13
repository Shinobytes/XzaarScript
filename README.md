# XzaarScript [![Build Status](https://travis-ci.org/zerratar/XzaarScript.svg?branch=master)](https://travis-ci.org/zerratar/XzaarScript) [![codecov](https://codecov.io/gh/zerratar/XzaarScript/branch/master/graph/badge.svg)](https://codecov.io/gh/zerratar/XzaarScript)

## Introduction
If Rust and TypeScript had a baby, this might be it. It's ugly, it's incomplete, it's buggy. But I love it! Originally created for a game I was making in Unity. But it took too long to develop that I didn't have time to finish it. Now here it is, free for everyone to use!

XzaarScript is written in C#, .NET Framework v4.6.1 using Visual Studio 2017

You can test out the demo website [here](http://xzaarscript.shinobytes.com)

## License
XzaarScript is licensed under the [GPL, version 3.0](https://www.gnu.org/licenses/gpl-3.0.en.html). See [gpl-licence.txt](https://github.com/zerratar/PapyrusDotNet/blob/master/gpl-license.txt) for details.

## Build
You can either: open up the `XzaarScript.sln` in visual studio, press build. Voilà!

Or you can open the root directory (same one that has the `XzaarScript.sln` file) and use the following commandline

    dotnet build

You can run all tests with

    dotnet test

## Documentation
This documentation totally sucks. I should feel bad. But I don't.
It will be updated along the development of this project.

### Types
All types are named the same as in `Rust` but they also have an alias matching `TypeScript`.
For example, the primitive type `i32` can also be referred to as `number` and `str` can be referred to as `string`.

See below for the full list of primitive types and their corresponding aliases.

Please note: The value **null** does not exist in the current state of XzaarScript.

#### Primitive Value Types

For number you can always just use `number`. It will refer to any numeric type.
But if you feel like being more specific you can explicitly tell XzaarScript the 
size of the number.
 
 * **boolean:** `bool` or `boolean`
 * **byte:** `i8`, `u8`, `sbyte`, `byte` or `number`
 * **short (word signed/unsiged):** `i16`, `u16`, `short`, `ushort` or `number`
 * **int (dword signed/unsigned):** `i32`, `u32`, `int`, `uint` or `number`
 * **long (qword signed/unsigned):** `i64`, `u64`, `long`, `ulong` or `number`
 * **float:** `f32`, `float` or `number`
 * **double:** `f64`, `double` or `number`
 * **string:** `str` or `string`
 * **char:** `str` or `string`, char doesn't really exist. It is handled as a string.

#### Primitive Reference Types

There exists only one primitive reference type, its the `any` type. It can be used in place of... Anything. Function references, lambdas, and all the value types. 

`any` is also the fallback-type for whenever the compiler is unable to determine a type.

The `any` type is somewhat similar to the C# `dynamic` or just TypeScript `any`, as it will not validate on compile-time and only on runtime.

Performance gains/loss using `any` ? Shit, the VM is so slow right now that theres no noticable difference anyway.

#### Constant/Literal Values
`Number` or `i32` or `int` or ... omg just stop! I should have just picked one and gone with it.
Anyway, you can also use `binary` and `hexadecimal` forms when supplying the values.

```typescript
// hexadecimal
let hex = 0xff;

// binary
let bin = 0b0001;

// as for strings you can use both single-quoute ' and double-quoute "
let strA = "hello!";
let strB = 'hello!';
```

#### Arrays

All types are supported as arrays! And can be created just as easy as in `TypeScript` or `JavaScript`.
Currently, array's have support for following functions `add`, `push`, `insert`, `remove`, `pop`
and the property: `length` that returns the size of the array.

```rust 
let arrayA = [];
let arrayB = ["hello world!", 1235, "yeah we can mix stuff here"];
let arrayC : i32[] = [123, 421];

$console.log(arrayB.length) // prints: 3

arrayB.add("test")       // add/push item 'test' to the end of the array
arrayB.push("test")      // add/push item 'test' to the end of the array
arrayB.remove(0)         // remove item at index 0
arrayB.insert(0, "test") // insert item 'test' at index 0
arrayB.pop()             // remove last item from the list, does not currently return the item
```

#### Functions

XzaarScript currently supports `global`/`static` and `external` functions.
Instanced functions will be implemented in the future along with classes.

A global function is just a function defined in the script while the external one
comes from injected .NET code. (More about that in the future. It's even more experimental than XzaarScript itself)

```rust
// global/static function, not explicitly defining return type
fn main() {

}

// with a defined returntype
fn main() -> void {

}

extern fn main(); // cannot have a body
```

##### Function Parameters

What would XzaarScript be if you couldn't pass along arguments to your functions?
Luckily, we don't have to try and imagine it as we totally can use it!

XzaarScript is an abomination when it comes to defining parameters since
you can define them in two different ways.

*C/C++/C#/Java/.. Way:*
```rust
fn test(str a, int b, any c, number d) {

}
```

*Rust/TypeScript/.. Way:*
```rust
fn test(a:str, b:int, c:any, d:number) {

}
```

Use whichever you feel like for now. But once settled I will probably only keep one way of defining parameters. Let's hope you pick the right one!

##### Function Return Types

You can use any type as return type of a function. even `any`. The only addition is the `void` type. 
It is used to explicitly tell XzaarScript that the function will not return anything. `void` cannot be used as a variable type.

> Even though you do not have to specify a return type for a function. It is recommended that you do as
it will make it easier for the compiler to know what type to return (*it will even compile faster!*). There are cases when it is impossible
for the compiler to determine the return type and it will return `any`. This may or may not be wanted. So keep that in mind!

##### Function References

You can reference to functions by using their names and then invoke those references as if you were calling the function.

```rust 
let console = $console
fn test() {
    console.log("oh hi, Mark!");
}

let a = test;
a(); // prints: oh hi, Mark!

// or 
fn test2(func) {
    func();
}

test2(a);     // prints: oh hi, Mark!
test2(test);  // prints: oh hi, Mark!
```

#### Defining Variables
Much like `TypeScript` or `JavaScript` you can use the keyword `let` and `var` to define your variables.
However, unlike `TypeScript` / `JavaScript` those keywords does not change the accessibility throughout the different scopes of the script.

So using `let` is exactly the same as `var` right now.

```typescript
let a = "";
var b = "";
```

Another thing is that you can explicitly setting the desired type,
doing so will speed up the compiler as it does not have to guess what type
you are referring to.

```typescript
let a:string = "";
var b:string = "";
```


Last thing. Throughout the document you will see variables starting with `$`
Most commonly `$console`. The dollar `$` sign on start of a variable name tells
XzaarScript that this variable is imported or registered from an external source.

[On the demo website](http://xzaarscript.shinobytes.com) we have a Console Wrapper Class in C# that we register as `$console` so we can have some sort of logging. 

### More Code Samples

You can test these out on the demo website [here](http://xzaarscript.shinobytes.com)

#### Replacing Functions

Wait what?

You can replace the function after its been declared by reassigning the function name.
But by doing so you will lose the original function forever!

Oh and the new function must have same sequence of parameters (names not important) and
have the same returntype.

Assigning a variable with the existing function before reassigning the actual function does not work.
As the variable will only hold a reference to the function while reassigning the function will replace
the actual function body and parameters.

```rust 
fn test() {
    console.log("forever lost");
}
fn test2() {
    console.log("This is what you will see");
}

// gets a function reference to 'test'
let a = test;

// reassign function body and parameters of 'test'
test = test2;

a();        // prints: This is what you will see
test();     // prints: This is what you will see
test2();    // prints: This is what you will see
```

```rust 
let console = $console
fn test() {
    console.log("oh hi, Mark!");
}

test();     // prints: oh hi, Mark!

test = () => {
    console.log("Oh damn!");
}

test();     // prints: Oh damn!

fn test2() {
    console.log("Bleh");
}

test = test2;

test();     // prints: Bleh
test2();    // prints: Bleh
```


#### Lambda / Arrow Functions
Syntax looks very much like `C#` and `TypeScript`
You may explicitly provide the type of each parameter to the lambda/arrow-function. however its not necessary. And only possible if you encapsulate the argument list with parenthesis.

```rust
let d = x:i32 => 0; // compile error
```
But following is okay!
```rust
let a = () => console.log("hello world"); // calls console.log, returns null
let b = () => 123;      // does nothing, returns 123
let c = () => "asd";    // does nothing, returns "asd"
let d = x => "asd" + x; // does nothing, returns "asd" + the value from x.
d("blah");              // "asdblah"

let e = (o:i32, k, n:string) => {
    if (o == 0) {
        return "It was 0. But K is " + k + " and n is " + n;
    }
    return n + " " + k + " (" + o + ")";
};

e(0, "any", "str"); // prints: It was 0. But K is any and n is str

let f = () => {
    // you don't have to return anything. This is just an empty one.
}

fn test(callback) {
    // ... logic
    callback();
}

test(() => {
    console.log("yay! callbacks....");
});

```
#### Well, hello world!
By now you might already have grasped some of XzaarScript! But here are some more of them lovely basic stuff.

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

// a single line comment!

/*
    a multiline
    comment!
*/

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
let j = 0;
while (j < 100) {
    console.log("I'm totally spamming");
    j++;
}

// and why not the typical for loop?
for (let i = 0; i < 100; i++) {
    console.log("A hundred times");
}
```

#### using structs
Structs are a data blob or 'data structure', which only supports fields. But! You may assign a field a reference to a function or lambda. So it is possible to have functions inside the struct. Just make sure those fields are of type `any`

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
    myFieldA: "hello",
    myFieldB: 123
};

/*
    This is also okay:

    let theStruct = myStruct {
        myFieldA = "hello",
        myFieldB = 123
    };  
*/

struct structWithFunc {
    myLambda: any,
    myFn: any
}

fn weee() {
    console.log("Weee!");
}

let func = structWithFunc {
    myLambda: () => { console.log("Oh you!"); }
    myFn: weee
}

func.myLambda(); // prints: Oh you!
func.myFn();     // prints: Weee!
console.log(theStruct.myFieldA + theStruct.myFieldB); // prints: hello123
```

To be updated with more soon!

## Known Issues
The current state of the Virtual Machine is slow and the tests only cover about 70%.
Therefor, there may be tons of bugs that I don't know of yet.

## History

To be updated with relevant information as soon as I have some sort of history/changelog.