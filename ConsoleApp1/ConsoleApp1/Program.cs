

var a1 = new A("iama");
var a2 = new A("iama");
var a3 = new A("iamothera");

var b1 = new B("iama");

Console.WriteLine(a1.Equals(a2));
Console.WriteLine(a1.Equals(a3));
Console.WriteLine(a1.Equals(b1));


record A(string name);
record B(string name);