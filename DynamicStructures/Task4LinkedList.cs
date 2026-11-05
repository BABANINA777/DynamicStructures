namespace DynamicStructures;

/// <summary>
/// Узел односвязного списка целых чисел
/// </summary>
public class ListNode
{
    public int Value;
    public ListNode? Next;

    public ListNode(int value, ListNode? next = null)
    {
        Value = value;
        Next = next;
    }
}

/// <summary>
/// Задание 4: Реализация 12 алгоритмов работы со СВОИМИ связными списками.
/// </summary>
public static class Task4LinkedList
{
    // =========================================================================
    // Вспомогательные методы: создание, вывод, клонирование
    // =========================================================================

    public static ListNode? Create(params int[] values)
    {
        if (values == null || values.Length == 0) return null;
        var head = new ListNode(values[0]);
        var curr = head;
        for (int i = 1; i < values.Length; i++)
        {
            curr.Next = new ListNode(values[i]);
            curr = curr.Next;
        }
        return head;
    }

    public static ListNode? Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        var parts = input.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        var nums = new List<int>();
        foreach (var p in parts)
        {
            if (int.TryParse(p, out int val))
                nums.Add(val);
        }
        return Create(nums.ToArray());
    }

    public static ListNode? Clone(ListNode? head)
    {
        if (head == null) return null;
        var newHead = new ListNode(head.Value);
        var currNew = newHead;
        var currOld = head.Next;
        while (currOld != null)
        {
            currNew.Next = new ListNode(currOld.Value);
            currNew = currNew.Next;
            currOld = currOld.Next;
        }
        return newHead;
    }

    public static string ToDisplayString(ListNode? head)
    {
        if (head == null) return "[пустой список]";
        var list = new List<string>();
        var curr = head;
        int count = 0;
        while (curr != null)
        {
            list.Add(curr.Value.ToString());
            curr = curr.Next;
            count++;
            if (count > 200) // Защита от бесконечного цикла при случайном зацикливании
            {
                list.Add("... (обнаружено зацикливание)");
                break;
            }
        }
        return string.Join(" -> ", list);
    }

    public static void Print(string prefix, ListNode? head)
    {
        Console.WriteLine($"{prefix}: {ToDisplayString(head)}");
    }


    // =========================================================================
    // 1. Переворот списка L (изменение ссылок)
    // =========================================================================
    /// <summary>
    /// Функция 1: Переворачивает список L (разворачивает ссылки в обратном порядке).
    /// Сложность: O(N) по времени, O(1) по памяти.
    /// </summary>
    public static void Reverse(ref ListNode? head)
    {
        ListNode? prev = null;
        var current = head;
        while (current != null)
        {
            var next = current.Next;
            current.Next = prev;
            prev = current;
            current = next;
        }
        head = prev;
    }


    // =========================================================================
    // 2. Перенос в начало (в конец) непустого списка L его последнего (первого) элемента
    // =========================================================================
    /// <summary>
    /// Функция 2a: Переносит в начало непустого списка его последний элемент.
    /// </summary>
    public static void MoveLastToFront(ref ListNode? head)
    {
        if (head == null || head.Next == null) return;

        ListNode prev = head;
        while (prev.Next!.Next != null)
        {
            prev = prev.Next;
        }

        var last = prev.Next;
        prev.Next = null;
        last.Next = head;
        head = last;
    }

    /// <summary>
    /// Функция 2b: Переносит в конец непустого списка его первый элемент.
    /// </summary>
    public static void MoveFrontToLast(ref ListNode? head)
    {
        if (head == null || head.Next == null) return;

        var first = head;
        head = head.Next;
        first.Next = null;

        var curr = head;
        while (curr.Next != null)
        {
            curr = curr.Next;
        }
        curr.Next = first;
    }


    // =========================================================================
    // 3. Количество различных элементов списка целых чисел
    // =========================================================================
    /// <summary>
    /// Функция 3: Определяет количество различных элементов списка.
    /// Сложность: O(N) по времени.
    /// </summary>
    public static int CountDistinct(ListNode? head)
    {
        if (head == null) return 0;
        var unique = new HashSet<int>();
        var curr = head;
        while (curr != null)
        {
            unique.Add(curr.Value);
            curr = curr.Next;
        }
        return unique.Count;
    }


    // =========================================================================
    // 4. Удаление из списка L неуникальных элементов
    // =========================================================================
    /// <summary>
    /// Функция 4a: Удаляет ВСЕ вхождения неуникальных элементов (остаются только те, что встречались ровно 1 раз).
    /// Пример: [1, 2, 2, 3, 4, 4, 5] -> [1, 3, 5]
    /// </summary>
    public static void RemoveNonUnique(ref ListNode? head)
    {
        if (head == null) return;

        var counts = new Dictionary<int, int>();
        var curr = head;
        while (curr != null)
        {
            counts[curr.Value] = counts.GetValueOrDefault(curr.Value, 0) + 1;
            curr = curr.Next;
        }

        var dummy = new ListNode(0, head);
        var prev = dummy;
        curr = head;

        while (curr != null)
        {
            if (counts[curr.Value] > 1)
            {
                prev.Next = curr.Next;
            }
            else
            {
                prev = curr;
            }
            curr = curr.Next;
        }

        head = dummy.Next;
    }

    /// <summary>
    /// Функция 4b: Удаление дубликатов (оставляет только первое вхождение каждого числа).
    /// Пример: [1, 2, 2, 3, 4, 4, 5] -> [1, 2, 3, 4, 5]
    /// </summary>
    public static void RemoveDuplicateOccurrences(ref ListNode? head)
    {
        if (head == null) return;
        var seen = new HashSet<int>();
        var dummy = new ListNode(0, head);
        var prev = dummy;
        var curr = head;

        while (curr != null)
        {
            if (seen.Contains(curr.Value))
            {
                prev.Next = curr.Next;
            }
            else
            {
                seen.Add(curr.Value);
                prev = curr;
            }
            curr = curr.Next;
        }

        head = dummy.Next;
    }


    // =========================================================================
    // 5. Вставка списка самого в себя вслед за первым вхождением числа x
    // =========================================================================
    /// <summary>
    /// Функция 5: Вставляет копию списка самого в себя вслед за первым вхождением числа x.
    /// </summary>
    public static bool InsertSelfAfterFirstX(ref ListNode? head, int x)
    {
        if (head == null) return false;

        var target = head;
        while (target != null && target.Value != x)
        {
            target = target.Next;
        }

        if (target == null) return false;

        // Клонируем исходный список
        var copyHead = Clone(head);
        if (copyHead == null) return false;

        var copyTail = copyHead;
        while (copyTail.Next != null)
        {
            copyTail = copyTail.Next;
        }

        // Вставляем между target и target.Next
        copyTail.Next = target.Next;
        target.Next = copyHead;
        return true;
    }


    // =========================================================================
    // 6. Вставка в упорядоченный список нового элемента с сохранением порядка
    // =========================================================================
    /// <summary>
    /// Функция 6: Вставляет элемент E в упорядоченный по неубыванию список L с сохранением порядка.
    /// </summary>
    public static void InsertSorted(ref ListNode? head, int e)
    {
        var newNode = new ListNode(e);
        if (head == null || e <= head.Value)
        {
            newNode.Next = head;
            head = newNode;
            return;
        }

        var curr = head;
        while (curr.Next != null && curr.Next.Value < e)
        {
            curr = curr.Next;
        }

        newNode.Next = curr.Next;
        curr.Next = newNode;
    }


    // =========================================================================
    // 7. Удаление из списка L всех элементов E
    // =========================================================================
    /// <summary>
    /// Функция 7: Удаляет из списка L все элементы со значением E. Возвращает количество удаленных.
    /// </summary>
    public static int RemoveAll(ref ListNode? head, int e)
    {
        int count = 0;
        while (head != null && head.Value == e)
        {
            head = head.Next;
            count++;
        }

        var curr = head;
        while (curr?.Next != null)
        {
            if (curr.Next.Value == e)
            {
                curr.Next = curr.Next.Next;
                count++;
            }
            else
            {
                curr = curr.Next;
            }
        }
        return count;
    }


    // =========================================================================
    // 8. Вставка нового элемента F перед первым вхождением элемента E
    // =========================================================================
    /// <summary>
    /// Функция 8: Вставляет элемент F перед первым вхождением элемента E, если E входит в L.
    /// </summary>
    public static bool InsertBeforeFirst(ref ListNode? head, int e, int f)
    {
        if (head == null) return false;

        if (head.Value == e)
        {
            head = new ListNode(f, head);
            return true;
        }

        var curr = head;
        while (curr.Next != null)
        {
            if (curr.Next.Value == e)
            {
                curr.Next = new ListNode(f, curr.Next);
                return true;
            }
            curr = curr.Next;
        }
        return false;
    }


    // =========================================================================
    // 9. Дописывание к списку L списка E (считать из файла)
    // =========================================================================
    /// <summary>
    /// Функция 9: Дописывает к списку L список E.
    /// </summary>
    public static void AppendList(ref ListNode? l, ListNode? e)
    {
        if (l == null)
        {
            l = Clone(e);
            return;
        }

        var curr = l;
        while (curr.Next != null)
        {
            curr = curr.Next;
        }
        curr.Next = Clone(e);
    }

    /// <summary>
    /// Считывание двух списков L и E из файла (1 строка - список L, 2 строка - список E)
    /// </summary>
    public static (ListNode? L, ListNode? E) LoadListsFromFile(string filename = "lists.txt")
    {
        string path = Stack.FindInputFile(filename);
        if (!File.Exists(path))
        {
            Console.WriteLine($"Файл '{path}' не найден. Создан демонстрационный файл.");
            File.WriteAllLines(path, new[] { "1 2 3 4 5", "6 7 8 9 10" });
        }

        var lines = File.ReadAllLines(path);
        var listL = lines.Length > 0 ? Parse(lines[0]) : null;
        var listE = lines.Length > 1 ? Parse(lines[1]) : null;
        return (listL, listE);
    }


    // =========================================================================
    // 10. Разбиение списка на два по первому вхождению заданного числа
    // =========================================================================
    /// <summary>
    /// Функция 10: Разбивает список целых чисел на два списка по первому вхождению заданного числа x.
    /// Первый список содержит элементы до x. Второй список начинается с x.
    /// Если числа нет, второй список пуст, а первый не меняется.
    /// </summary>
    public static (ListNode? L1, ListNode? L2) SplitByFirstOccurrence(ref ListNode? head, int x)
    {
        if (head == null) return (null, null);

        if (head.Value == x)
        {
            var second = head;
            head = null;
            return (null, second);
        }

        var curr = head;
        while (curr.Next != null)
        {
            if (curr.Next.Value == x)
            {
                var second = curr.Next;
                curr.Next = null; // Разрезаем список
                return (head, second);
            }
            curr = curr.Next;
        }

        return (head, null); // Число не найдено
    }


    // =========================================================================
    // 11. Удвоение списка (приписывание в конец списка себя самого)
    // =========================================================================
    /// <summary>
    /// Функция 11: Удваивает список, приписывая в его конец копию себя самого.
    /// </summary>
    public static void DoubleList(ref ListNode? head)
    {
        if (head == null) return;

        var copy = Clone(head);
        var curr = head;
        while (curr.Next != null)
        {
            curr = curr.Next;
        }
        curr.Next = copy;
    }


    // =========================================================================
    // 12. Обмен местами двух элементов списка (перелинковка ссылок!)
    // =========================================================================
    /// <summary>
    /// Функция 12a: Обмен местами двух узлов по их индексам (0-based) путем перестановки ССЫЛОК (pointer relinking).
    /// </summary>
    public static bool SwapNodesByIndex(ref ListNode? head, int idx1, int idx2)
    {
        if (head == null || idx1 == idx2) return true;

        if (idx1 > idx2) (idx1, idx2) = (idx2, idx1);

        var dummy = new ListNode(0, head);
        ListNode? prev1 = null, node1 = null;
        ListNode? prev2 = null, node2 = null;

        var curr = dummy;
        int i = 0;
        while (curr.Next != null)
        {
            if (i == idx1)
            {
                prev1 = curr;
                node1 = curr.Next;
            }
            if (i == idx2)
            {
                prev2 = curr;
                node2 = curr.Next;
                break;
            }
            curr = curr.Next;
            i++;
        }

        if (node1 == null || node2 == null)
            return false; // Один из индексов за пределами списка

        // Перелинковка ссылок
        if (node1.Next == node2) // Соседние узлы
        {
            prev1!.Next = node2;
            node1.Next = node2.Next;
            node2.Next = node1;
        }
        else // Несоседние узлы
        {
            var tempNext = node1.Next;
            prev1!.Next = node2;
            node1.Next = node2.Next;
            prev2!.Next = node1;
            node2.Next = tempNext;
        }

        head = dummy.Next;
        return true;
    }

    /// <summary>
    /// Функция 12b: Обмен местами двух элементов по их значениям (первые вхождения)
    /// </summary>
    public static bool SwapValues(ListNode? head, int val1, int val2)
    {
        if (head == null || val1 == val2) return true;

        ListNode? n1 = null, n2 = null;
        var curr = head;
        while (curr != null)
        {
            if (n1 == null && curr.Value == val1) n1 = curr;
            else if (n2 == null && curr.Value == val2) n2 = curr;
            curr = curr.Next;
        }

        if (n1 != null && n2 != null)
        {
            (n1.Value, n2.Value) = (n2.Value, n1.Value);
            return true;
        }
        return false;
    }


    // =========================================================================
    // Комплексный демонстрационный запуск всех 12 функций Задания 4
    // =========================================================================
    public static void RunAllDemo()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine("    ДЕМОНСТРАЦИЯ ВСЕХ 12 АЛГОРИТМОВ ЗАДАНИЯ 4 (СПИСКИ)");
        Console.WriteLine("=======================================================");

        // 1. Reverse
        var l1 = Create(1, 2, 3, 4, 5);
        Print("1. Исходный список", l1);
        Reverse(ref l1);
        Print("   Результат Reverse()", l1);

        // 2. Move last to front / front to last
        var l2 = Create(10, 20, 30, 40);
        Print("\n2. Исходный список", l2);
        MoveLastToFront(ref l2);
        Print("   Перенос последнего в начало", l2);
        MoveFrontToLast(ref l2);
        Print("   Перенос первого в конец (вернули)", l2);

        // 3. Count distinct
        var l3 = Create(5, 1, 2, 5, 3, 2, 4, 1);
        Print("\n3. Список чисел", l3);
        Console.WriteLine($"   Количество различных элементов: {CountDistinct(l3)} (ожидается 5: 1, 2, 3, 4, 5)");

        // 4. Remove non-unique
        var l4a = Create(1, 2, 2, 3, 4, 4, 5);
        Print("\n4. Исходный список с дубликатами", l4a);
        RemoveNonUnique(ref l4a);
        Print("   Удаление всех неуникальных элементов (остались только уникальные)", l4a);
        var l4b = Create(1, 2, 2, 3, 4, 4, 5);
        RemoveDuplicateOccurrences(ref l4b);
        Print("   Дедупликация (сохранено по одному экземпляру каждого)", l4b);

        // 5. Insert self after first X
        var l5 = Create(10, 20, 30);
        Print("\n5. Исходный список L", l5);
        InsertSelfAfterFirstX(ref l5, 20);
        Print("   Вставка L самого в себя вслед за первым числом 20", l5);

        // 6. Insert sorted
        var l6 = Create(10, 20, 30, 50, 60);
        Print("\n6. Упорядоченный список", l6);
        InsertSorted(ref l6, 35);
        Print("   Вставка 35 с сохранением упорядоченности", l6);
        InsertSorted(ref l6, 5);
        Print("   Вставка 5 в самое начало", l6);

        // 7. Remove all occurrences of E
        var l7 = Create(7, 1, 7, 7, 2, 3, 7);
        Print("\n7. Исходный список", l7);
        int remCount = RemoveAll(ref l7, 7);
        Print($"   После удаления всех элементов '7' (удалено {remCount} шт.)", l7);

        // 8. Insert F before first E
        var l8 = Create(1, 2, 4, 5);
        Print("\n8. Исходный список", l8);
        InsertBeforeFirst(ref l8, e: 4, f: 99);
        Print("   Вставка 99 перед первым вхождением числа 4", l8);

        // 9. Append list E to L (с чтением из файла)
        Console.WriteLine("\n9. Дописывание списка E к списку L из файла 'lists.txt':");
        var (fileL, fileE) = LoadListsFromFile("lists.txt");
        Print("   Считанный список L", fileL);
        Print("   Считанный список E", fileE);
        AppendList(ref fileL, fileE);
        Print("   Результат объединения L + E", fileL);

        // 10. Split by first occurrence
        var l10 = Create(1, 2, 3, 77, 4, 5);
        Print("\n10. Исходный список", l10);
        var (sub1, sub2) = SplitByFirstOccurrence(ref l10, 77);
        Print("    Список 1 (до 77)", sub1);
        Print("    Список 2 (начиная с 77)", sub2);

        // 11. Double list
        var l11 = Create(1, 2, 3);
        Print("\n11. Исходный список", l11);
        DoubleList(ref l11);
        Print("    Удвоенный список (приписан сам к себе)", l11);

        // 12. Swap nodes
        var l12 = Create(10, 20, 30, 40, 50);
        Print("\n12. Исходный список", l12);
        SwapNodesByIndex(ref l12, 1, 3); // Меняем индексы 1 (значение 20) и 3 (значение 40)
        Print("    Обмен местами элементов с индексами 1 и 3 (ссылки переставлены)", l12);
        Console.WriteLine("=======================================================\n");
    }
}
