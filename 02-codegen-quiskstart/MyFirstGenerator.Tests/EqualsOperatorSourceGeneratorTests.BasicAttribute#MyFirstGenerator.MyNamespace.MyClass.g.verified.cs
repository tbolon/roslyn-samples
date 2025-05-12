//HintName: MyFirstGenerator.MyNamespace.MyClass.g.cs

namespace MyNamespace
{
    partial class MyClass
    {
        public static bool operator==(MyClass left, MyClass right) => left?.Equals(right) == true;
        public static bool operator!=(MyClass left, MyClass right) => !(left == right);
    }
}