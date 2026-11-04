
using System.Text;

namespace DynamicStructures.Core.DataStructures;

public class SinglyLinkedList<T>
{
    private class Node
    {
        public T Data;
        public Node? Next;

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    private Node? head;
    private Node? tail;

    public int Count { get; private set; }

    public bool IsEmpty => head == null;

    // Добавление элемента в начало списка
    public void AddFirst(T value)
    {
        Node newNode = new Node(value);

        newNode.Next = head;
        head = newNode;

        if (tail == null)
            tail = newNode;//Если не было вообще элементов, значит tail пуст (равен null).

        Count++;
    }

    // Добавление элемента в конец списка
    public void AddLast(T value)
    {
        Node newNode = new Node(value);

        if (head == null)
        {
            head = newNode;
            tail = newNode;
        }
        else
        {
            tail!.Next = newNode;
            tail = newNode;
        }

        Count++;
    }

    // Удаление первого элемента (и получение)
    public bool TryRemoveFirst(out T value)
    {
        if (head == null)
        {
            value = default!;
            return false;
        }

        value = head.Data;
        head = head.Next;//Список просто "забывает" про прошлый head. Но раз объект стал «недостижимым», GC удалит его сам.

        if (head == null)
            tail = null;

        Count--;
        return true;
    }

    // Получение первого элемента без удаления
    public bool TryPeekFirst(out T value)
    {
        if (head == null)
        {
            value = default!;
            return false;
        }

        value = head.Data;
        return true;
    }

    // Преобразование списка в строку
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();

        Node? current = head;

        while (current != null)
        {
            result.Append(current.Data);

            if (current.Next != null)
                result.Append(" -> ");

            current = current.Next;
        }

        return result.Length == 0 ? "Коллекция пуста" : result.ToString();
    }
}
