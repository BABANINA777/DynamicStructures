using DynamicStructures;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Node? stackTop = null;

while (true)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("========================================================================");
    Console.WriteLine("        ЛАБОРАТОРНАЯ РАБОТА № 3: ДИНАМИЧЕСКИЕ СТРУКТУРЫ ДАННЫХ          ");
    Console.WriteLine("========================================================================");
    Console.ResetColor();
    Console.WriteLine("1. [Часть 1] Стек: Чтение операций из input.txt с замером времени");
    Console.WriteLine("2. [Часть 1] Стек: Интерактивное управление (Push, Pop, Top, Print, IsEmpty)");
    Console.WriteLine("3. [Часть 1] Постфиксные вычисления (RPN) и перевод Инфикс -> Постфикс");
    Console.WriteLine("4. [Часть 2] Очередь: Сравнение Своей очереди и Queue<string> на input.txt");
    Console.WriteLine("5. [Часть 2] Очередь: Сравнительный бенчмарк (разная длина и разный состав)");
    Console.WriteLine("6. [Часть 2] Очередь: Интерактивное управление (Enqueue, Dequeue, Peek, Print)");
    Console.WriteLine("7. [Часть 3] Практические применения структур (LRU Cache, Undo/Redo, Принтер, Файлы)");
    Console.WriteLine("8. [Часть 4] 12 алгоритмов работы со СВОИМИ связными списками (Авто-демо)");
    Console.WriteLine("9. [Часть 4] 12 алгоритмов связных списков (Интерактивный выбор задачи)");
    Console.WriteLine("0. Выход");
    Console.Write("\nВыберите пункт меню: ");

    string? choice = Console.ReadLine()?.Trim();
    if (choice == "0")
    {
        Console.WriteLine("\nЗавершение работы программы. До свидания!");
        break;
    }

    switch (choice)
    {
        case "1":
            RunStackFileOperations(ref stackTop);
            break;

        case "2":
            RunInteractiveStack(ref stackTop);
            break;

        case "3":
            RunPostfixMenu();
            break;

        case "4":
            QueueTaskRunner.CompareBothOnInputFile("input.txt");
            break;

        case "5":
            QueueTaskRunner.RunFullBenchmarkSuite();
            break;

        case "6":
            RunInteractiveQueue();
            break;

        case "7":
            RunTask3Menu();
            break;

        case "8":
            Task4LinkedList.RunAllDemo();
            break;

        case "9":
            RunTask4Interactive();
            break;

        default:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Неверный пункт меню, попробуйте снова.");
            Console.ResetColor();
            break;
    }
}

static void RunStackFileOperations(ref Node? stackTop)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n--- Запуск выполнения команд стека из файла input.txt ---");
    Console.ResetColor();
    Stack.LoadFile(ref stackTop);
}

static void RunInteractiveStack(ref Node? stackTop)
{
    while (true)
    {
        Console.WriteLine("\n--- ИНТЕРАКТИВНЫЙ СТЕК ---");
        Console.WriteLine("1. Push(elem) - Поместить элемент");
        Console.WriteLine("2. Pop()      - Извлечь вершину");
        Console.WriteLine("3. Top()      - Посмотреть вершину");
        Console.WriteLine("4. isEmpty()  - Проверить на пустоту");
        Console.WriteLine("5. Print()    - Распечатать стек");
        Console.WriteLine("6. Очистить стек");
        Console.WriteLine("0. Назад в главное меню");
        Console.Write("Ваш выбор: ");

        string? cmd = Console.ReadLine()?.Trim();
        if (cmd == "0") break;

        switch (cmd)
        {
            case "1":
                Console.Write("Введите значение элемента: ");
                string? val = Console.ReadLine();
                if (!string.IsNullOrEmpty(val))
                {
                    Stack.Push(ref stackTop, val);
                    Console.WriteLine($"Элемент '{val}' добавлен на вершину стека.");
                }
                break;

            case "2":
                try
                {
                    string popped = Stack.Pop(ref stackTop);
                    Console.WriteLine($"Извлечен элемент: '{popped}'");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.ResetColor();
                }
                break;

            case "3":
                string? topVal = Stack.Top(stackTop);
                if (topVal != null)
                    Console.WriteLine($"На вершине стека: '{topVal}'");
                else
                    Console.WriteLine("Стек пуст.");
                break;

            case "4":
                Console.WriteLine($"isEmpty: {Stack.IsEmpty(stackTop)}");
                break;

            case "5":
                Console.Write("Текущий стек (вершина -> основание): ");
                Stack.Print(stackTop);
                break;

            case "6":
                stackTop = null;
                Console.WriteLine("Стек очищен.");
                break;

            default:
                Console.WriteLine("Неизвестная команда.");
                break;
        }
    }
}

static void RunInteractiveQueue()
{
    var queue = new CustomQueue();
    while (true)
    {
        Console.WriteLine("\n--- ИНТЕРАКТИВНАЯ ОЧЕРЕДЬ (CustomQueue) ---");
        Console.WriteLine("1. Enqueue(elem) - Добавить элемент в конец (хвост)");
        Console.WriteLine("2. Dequeue()      - Извлечь элемент из начала (голова)");
        Console.WriteLine("3. Peek()         - Посмотреть начало очереди");
        Console.WriteLine("4. isEmpty()      - Проверить на пустоту");
        Console.WriteLine("5. Print()        - Распечатать очередь");
        Console.WriteLine("6. Очистить очередь");
        Console.WriteLine("0. Назад в главное меню");
        Console.Write("Ваш выбор: ");

        string? cmd = Console.ReadLine()?.Trim();
        if (cmd == "0") break;

        switch (cmd)
        {
            case "1":
                Console.Write("Введите значение элемента: ");
                string? val = Console.ReadLine();
                if (!string.IsNullOrEmpty(val))
                {
                    queue.Enqueue(val);
                    Console.WriteLine($"Элемент '{val}' добавлен в конец очереди.");
                }
                break;

            case "2":
                try
                {
                    string removed = queue.Dequeue();
                    Console.WriteLine($"Извлечен элемент: '{removed}'");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    Console.ResetColor();
                }
                break;

            case "3":
                string? first = queue.Peek();
                if (first != null) Console.WriteLine($"В начале очереди: '{first}'");
                else Console.WriteLine("Очередь пуста.");
                break;

            case "4":
                Console.WriteLine($"isEmpty: {queue.IsEmpty()} (элементов: {queue.Count})");
                break;

            case "5":
                Console.Write("Очередь (начало -> конец): ");
                queue.Print();
                break;

            case "6":
                queue.Clear();
                Console.WriteLine("Очередь очищена.");
                break;

            default:
                Console.WriteLine("Неизвестная команда.");
                break;
        }
    }
}

static void RunPostfixMenu()
{
    while (true)
    {
        Console.WriteLine("\n--- ПОСТФИКСНЫЕ ВЫЧИСЛЕНИЯ (RPN) ---");
        Console.WriteLine("1. Перевод Инфиксного выражения в Постфиксное (Shunting-yard)");
        Console.WriteLine("2. Вычисление готового Постфиксного выражения");
        Console.WriteLine("3. Полный цикл: Ввод формулы -> Перевод в RPN -> Вычисление результата");
        Console.WriteLine("0. Назад в главное меню");
        Console.Write("Ваш выбор: ");

        string? subChoice = Console.ReadLine()?.Trim();
        if (subChoice == "0") break;

        switch (subChoice)
        {
            case "1":
                Console.Write("Введите инфиксное выражение (например, '(7 + 8) / (3 + 2)' или 'cos(0) + 2 ^ 3'): ");
                string? infix = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(infix))
                {
                    try
                    {
                        string rpn = PostfixCalculator.InfixToPostfix(infix);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Постфиксная запись (RPN): {rpn}");
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        Console.ResetColor();
                    }
                }
                break;

            case "2":
                Console.Write("Введите постфиксное выражение (например, '7 8 + 3 2 + /'): ");
                string? rpnInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(rpnInput))
                {
                    try
                    {
                        double result = PostfixCalculator.EvaluatePostfix(rpnInput);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Результат вычисления: {result}");
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        Console.ResetColor();
                    }
                }
                break;

            case "3":
                Console.Write("Введите математическое выражение: ");
                string? formula = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(formula))
                {
                    try
                    {
                        string postfix = PostfixCalculator.InfixToPostfix(formula);
                        Console.WriteLine($"1) Постфиксная форма: {postfix}");
                        double res = PostfixCalculator.EvaluatePostfix(postfix);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"2) Результат вычисления: {res}");
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Ошибка: {ex.Message}");
                        Console.ResetColor();
                    }
                }
                break;

        }
    }
}

static void RunTask3Menu()
{
    while (true)
    {
        Console.WriteLine("\n--- ЗАДАНИЕ 3: ПРАКТИЧЕСКИЕ ПРИМЕНЕНИЯ СТРУКТУР ---");
        Console.WriteLine("1. Структура 'СПИСОК': LRU Cache (Кэш с вытеснением давно не использовавшихся элементов)");
        Console.WriteLine("2. Структура 'СТЕК': Undo / Redo (Отмена и повтор действий в редакторе)");
        Console.WriteLine("3. Структура 'ОЧЕРЕДЬ': Очередь заданий на печать (Print Spooler)");
        Console.WriteLine("4. Структура 'ДЕРЕВЬЯ': Иерархия файловой системы (дерево папок и файлов)");
        Console.WriteLine("5. Запустить все 4 примера подряд");
        Console.WriteLine("0. Назад в главное меню");
        Console.Write("Ваш выбор: ");

        string? sub = Console.ReadLine()?.Trim();
        if (sub == "0") break;

        switch (sub)
        {
            case "1": Task3Applications.RunLruCacheDemo(); break;
            case "2": Task3Applications.RunUndoRedoDemo(); break;
            case "3": Task3Applications.RunPrintSpoolerDemo(); break;
            case "4": Task3Applications.RunFileSystemTreeDemo(); break;
            case "5": Task3Applications.RunAll(); break;
            default: Console.WriteLine("Неизвестный пункт."); break;
        }
    }
}

static void RunTask4Interactive()
{
    var list = Task4LinkedList.Create(1, 2, 3, 4, 5);

    while (true)
    {
        Console.WriteLine("\n-----------------------------------------------------------");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Task4LinkedList.Print("ТЕКУЩИЙ СПИСОК L", list);
        Console.ResetColor();
        Console.WriteLine("-----------------------------------------------------------");
        Console.WriteLine("1.  Переворот списка L (разворот ссылок)");
        Console.WriteLine("2.  Перенос в начало (или конец) последнего (первого) элемента");
        Console.WriteLine("3.  Количество различных элементов списка");
        Console.WriteLine("4.  Удаление неуникальных элементов");
        Console.WriteLine("5.  Вставка списка самого в себя вслед за первым числом X");
        Console.WriteLine("6.  Вставка нового элемента в упорядоченный список");
        Console.WriteLine("7.  Удаление всех элементов E");
        Console.WriteLine("8.  Вставка F перед первым вхождением E");
        Console.WriteLine("9.  Дописывание к L списка E (считать из файла lists.txt)");
        Console.WriteLine("10. Разбиение списка на два по первому вхождению числа");
        Console.WriteLine("11. Удвоение списка (приписывание себя в конец)");
        Console.WriteLine("12. Обмен местами двух элементов (перелинковка ссылок)");
        Console.WriteLine("13. Задать новый список вручную (через пробел)");
        Console.WriteLine("0.  Назад в главное меню");
        Console.Write("Выберите алгоритм: ");

        string? taskNum = Console.ReadLine()?.Trim();
        if (taskNum == "0") break;

        switch (taskNum)
        {
            case "1":
                Task4LinkedList.Reverse(ref list);
                Task4LinkedList.Print("Результат разворота", list);
                break;

            case "2":
                Console.WriteLine("a) Перенести последний в начало");
                Console.WriteLine("b) Перенести первый в конец");
                Console.Write("Выбор (a/b): ");
                string? ab = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (ab == "b") Task4LinkedList.MoveFrontToLast(ref list);
                else Task4LinkedList.MoveLastToFront(ref list);
                Task4LinkedList.Print("Результат", list);
                break;

            case "3":
                int distinct = Task4LinkedList.CountDistinct(list);
                Console.WriteLine($"Количество различных элементов: {distinct}");
                break;

            case "4":
                Console.WriteLine("a) Удалить все неуникальные числа (оставить только те, что были по 1 разу)");
                Console.WriteLine("b) Дедупликация (оставить по одному экземпляру каждого числа)");
                Console.Write("Выбор (a/b): ");
                string? dOpt = Console.ReadLine()?.Trim().ToLowerInvariant();
                if (dOpt == "b") Task4LinkedList.RemoveDuplicateOccurrences(ref list);
                else Task4LinkedList.RemoveNonUnique(ref list);
                Task4LinkedList.Print("Результат", list);
                break;

            case "5":
                Console.Write("Введите число X, после которого вставить копию списка: ");
                if (int.TryParse(Console.ReadLine(), out int x))
                {
                    bool ok = Task4LinkedList.InsertSelfAfterFirstX(ref list, x);
                    if (ok) Task4LinkedList.Print("Результат", list);
                    else Console.WriteLine($"Число {x} не найдено в списке.");
                }
                break;

            case "6":
                Console.Write("Введите число для вставки в упорядоченный список: ");
                if (int.TryParse(Console.ReadLine(), out int eVal))
                {
                    Task4LinkedList.InsertSorted(ref list, eVal);
                    Task4LinkedList.Print("Результат", list);
                }
                break;

            case "7":
                Console.Write("Введите число E для полного удаления из списка: ");
                if (int.TryParse(Console.ReadLine(), out int delE))
                {
                    int deleted = Task4LinkedList.RemoveAll(ref list, delE);
                    Console.WriteLine($"Удалено элементов: {deleted}");
                    Task4LinkedList.Print("Результат", list);
                }
                break;

            case "8":
                Console.Write("Введите число E (перед которым вставить): ");
                int.TryParse(Console.ReadLine(), out int beforeE);
                Console.Write("Введите число F (которое вставить): ");
                int.TryParse(Console.ReadLine(), out int insertF);
                bool insOk = Task4LinkedList.InsertBeforeFirst(ref list, beforeE, insertF);
                if (insOk) Task4LinkedList.Print("Результат", list);
                else Console.WriteLine($"Элемент {beforeE} не найден в списке.");
                break;

            case "9":
                var (fileL, fileE) = Task4LinkedList.LoadListsFromFile("lists.txt");
                Task4LinkedList.Print("Список L из файла", fileL);
                Task4LinkedList.Print("Список E из файла", fileE);
                Task4LinkedList.AppendList(ref fileL, fileE);
                Task4LinkedList.Print("Объединенный список L + E", fileL);
                list = fileL;
                break;

            case "10":
                Console.Write("Введите число-разделитель: ");
                if (int.TryParse(Console.ReadLine(), out int splitVal))
                {
                    var (p1, p2) = Task4LinkedList.SplitByFirstOccurrence(ref list, splitVal);
                    Task4LinkedList.Print("Первая часть (до числа)", p1);
                    Task4LinkedList.Print("Вторая часть (начиная с числа)", p2);
                    list = p1; // Сохраняем первую часть как текущий
                }
                break;

            case "11":
                Task4LinkedList.DoubleList(ref list);
                Task4LinkedList.Print("Результат удвоения", list);
                break;

            case "12":
                Console.Write("Введите первый индекс (с 0): ");
                int.TryParse(Console.ReadLine(), out int idx1);
                Console.Write("Введите второй индекс (с 0): ");
                int.TryParse(Console.ReadLine(), out int idx2);
                bool swapped = Task4LinkedList.SwapNodesByIndex(ref list, idx1, idx2);
                if (swapped) Task4LinkedList.Print("Результат обмена ссылок", list);
                else Console.WriteLine("Один из индексов находится вне границ списка.");
                break;

            case "13":
                Console.Write("Введите элементы списка через пробел: ");
                string? raw = Console.ReadLine();
                list = Task4LinkedList.Parse(raw ?? "");
                Task4LinkedList.Print("Новый список", list);
                break;

            default:
                Console.WriteLine("Неизвестный пункт.");
                break;
        }
    }
}
