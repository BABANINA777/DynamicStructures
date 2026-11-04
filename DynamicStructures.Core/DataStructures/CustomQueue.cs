
namespace DynamicStructures.Core.DataStructures;

public class CustomQueue<T>
{
    private readonly SinglyLinkedList<T> items = new();

    // Добавление элемента в конец очереди
    public void Enqueue(T value)
    {
        items.AddLast(value);
    }

    // Удаление и получение первого элемента
    public bool TryDequeue(out T value)
    {
        return items.TryRemoveFirst(out value);
    }

    // Получение первого элемента без удаления
    public bool TryPeek(out T value)
    {
        return items.TryPeekFirst(out value);
    }

    // Проверка пустоты очереди
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

    // Представление очереди в виде строки
    public override string ToString()
    {
        return items.ToString();
    }
}
