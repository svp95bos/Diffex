// See https://aka.ms/new-console-template for more information

using Samples;
using Diffster;
var instance1 = new SampleClass1();
var instance2 = new SampleClass1();

instance1.NumberA = 1;
instance2.NumberA = 2;

var differences = instance1.Diff(instance2);
Console.WriteLine(string.Join(", ", differences));
Console.ReadLine();
