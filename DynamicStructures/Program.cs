using DynamicStructures;

Node node = new Node();
/*
Stack.Push(ref node,"Привет");
Stack.Push(ref node,"Привет1");
Stack.Print(node);
Stack.Push(ref node,"Привет2");
Stack.Push(ref node,"Привет3");
Stack.Print(node);
Console.WriteLine();
Console.WriteLine(Stack.Pop(ref node, 99));
Console.WriteLine();
Stack.Print(node);
 */
Console.WriteLine("Dynamic Structures Program");

Console.WriteLine("1. Работа с файлом");

int a = int.Parse(Console.ReadLine());
switch (a)
{
 case 1:
  Stack.LoadFile(ref node);
     break;
}




