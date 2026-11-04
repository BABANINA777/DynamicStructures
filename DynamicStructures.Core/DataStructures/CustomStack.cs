
namespace DynamicStructures.Core.DataStructures;

public class CustomStack<T>
{
    private readonly SinglyLinkedList<T> items = new();

    // Добавление элемента на вершину стека
    public void Push(T value)
    {
        items.AddFirst(value);
    }

    // Удаление и получение верхнего элемента
    public bool TryPop(out T value)
    {
        return items.TryRemoveFirst(out value);//В Push() добавляем в начало, поэтому первый элемент - это вернхий элемент.
    }

    // Получение верхнего элемента без удаления
    public bool TryTop(out T value)
    {
        return items.TryPeekFirst(out value);//В Push() добавляем в начало, поэтому первый элемент - это вернхий элемент.
    }

    // Проверка пустоты стека
    public bool IsEmpty()
    {
        return items.IsEmpty;
    }

    // Количество элементов
    public int Count => items.Count;
    /*
     * Это именно свойство, но оно read-only:
     * Лямбда-выражение в данном случае используется как сокращенная запись для геттера свойства.
     * Из-за отсутствия блока set свойство автоматически становится доступным только для чтения (read-only).
    */

    // Вывод содержимого стека
    public override string ToString()
    {
        return items.ToString();
    }
}
