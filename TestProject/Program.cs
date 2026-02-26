string[] names = ["B123", "C234", "A345", "C15", "B177", "G3003", "C235", "B179"];
int bin = 0;
foreach (string name in names)
{
  if (name.StartsWith("B"))
  {
    Console.WriteLine($"names[{bin}]='{name}' starts with 'B'");
  }
  bin++;
}