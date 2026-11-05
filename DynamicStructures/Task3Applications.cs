namespace DynamicStructures;

/// <summary>
/// Задание 3: Практическое применение динамических структур данных
/// 1. Список: LRU Cache (Кэш с вытеснением давно не использовавшихся элементов)
/// 2. Стек: Undo / Redo (Отмена и повтор действий в редакторе)
/// 3. Очередь: Очередь заданий на печать (Print Spooler)
/// 4. Дерево: Иерархия файловой системы (папки и файлы, рекурсивный расчет размера, вывод дерева)
/// </summary>
public static class Task3Applications
{
    // =========================================================================
    // 1. СПИСОК: LRU Cache (Least Recently Used Cache)
    // =========================================================================
    public class LruNode
    {
        public string Key;
        public string Value;
        public LruNode? Prev;
        public LruNode? Next;

        public LruNode(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    /// <summary>
    /// LRU Cache на базе собственного двусвязного списка + Dictionary для O(1) доступа.
    /// Применяется в: ядрах ОС (виртуальная память / page replacement),
    /// СУБД (буферный пул PostgreSQL, MySQL InnoDB), Redis, кэшировании HTTP-запросов.
    /// </summary>
    public class LruCache
    {
        private readonly int _capacity;
        private readonly Dictionary<string, LruNode> _map = new();
        private LruNode? _head; // Самый свежий элемент (MRU)
        private LruNode? _tail; // Самый старый / давно не используемый (LRU)

        public LruCache(int capacity)
        {
            _capacity = capacity;
        }

        public string? Get(string key)
        {
            if (!_map.TryGetValue(key, out var node))
                return null;

            MoveToHead(node);
            return node.Value;
        }

        public void Put(string key, string value)
        {
            if (_map.TryGetValue(key, out var existing))
            {
                existing.Value = value;
                MoveToHead(existing);
                return;
            }

            if (_map.Count >= _capacity && _tail != null)
            {
                // Вытесняем наименее востребованный с хвоста (LRU)
                Console.WriteLine($"   [LRU Вытеснение] Кэш переполнен (емкость={_capacity}). Удален самый старый элемент: key='{_tail.Key}'");
                _map.Remove(_tail.Key);
                RemoveNode(_tail);
            }

            var newNode = new LruNode(key, value);
            AddHead(newNode);
            _map[key] = newNode;
        }

        private void AddHead(LruNode node)
        {
            node.Next = _head;
            node.Prev = null;
            if (_head != null) _head.Prev = node;
            _head = node;
            _tail ??= _head;
        }

        private void RemoveNode(LruNode node)
        {
            if (node.Prev != null) node.Prev.Next = node.Next;
            else _head = node.Next;

            if (node.Next != null) node.Next.Prev = node.Prev;
            else _tail = node.Prev;
        }

        private void MoveToHead(LruNode node)
        {
            if (node == _head) return;
            RemoveNode(node);
            AddHead(node);
        }

        public void PrintState()
        {
            Console.Write("   Текущее состояние кэша (MRU -> LRU): ");
            var curr = _head;
            var list = new List<string>();
            while (curr != null)
            {
                list.Add($"[{curr.Key}: {curr.Value}]");
                curr = curr.Next;
            }
            Console.WriteLine(list.Count > 0 ? string.Join(" -> ", list) : "[Пусто]");
        }
    }

    public static void RunLruCacheDemo()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 1. ПРИМЕР ДЛЯ СТРУКТУРЫ 'СПИСОК': LRU CACHE");
        Console.WriteLine(" Назначение: Кэш с вытеснением давно не использовавшихся элементов.");
        Console.WriteLine(" Применение: Буферный пул СУБД, управление страницами памяти в ОС, Redis.");
        Console.WriteLine(" Двусвязный список позволяет за O(1) перемещать элементы в голову и вытеснять с хвоста.");
        Console.WriteLine("=======================================================");

        var cache = new LruCache(capacity: 3);
        Console.WriteLine("Создан LRU-кэш емкостью 3 элемента.\n");

        Console.WriteLine("1) Добавляем 'PageA', 'PageB', 'PageC':");
        cache.Put("PageA", "Содержимое страницы A");
        cache.Put("PageB", "Содержимое страницы B");
        cache.Put("PageC", "Содержимое страницы C");
        cache.PrintState();

        Console.WriteLine("\n2) Обращаемся к 'PageA' (чтение):");
        string? val = cache.Get("PageA");
        Console.WriteLine($"   Получено: {val}");
        Console.WriteLine("   PageA переместился в начало как самый свежий!");
        cache.PrintState();

        Console.WriteLine("\n3) Добавляем 'PageD' (кэш полон, самый старый должен удалиться):");
        cache.Put("PageD", "Содержимое страницы D");
        cache.PrintState();
    }


    // =========================================================================
    // 2. СТЕК: Undo / Redo в текстовом редакторе
    // =========================================================================
    public record EditorAction(string Type, string Text);

    public class TextEditor
    {
        public string Content { get; private set; } = "";
        private readonly Stack<EditorAction> _undoStack = new();
        private readonly Stack<EditorAction> _redoStack = new();

        public void Type(string text)
        {
            Content += text;
            _undoStack.Push(new EditorAction("TYPE", text));
            _redoStack.Clear(); // Новое действие сбрасывает цепочку Redo
            Console.WriteLine($"   Введено: \"{text}\" | Текст: \"{Content}\"");
        }

        public void Backspace(int count)
        {
            if (Content.Length == 0) return;
            count = Math.Min(count, Content.Length);
            string deleted = Content.Substring(Content.Length - count);
            Content = Content.Substring(0, Content.Length - count);

            _undoStack.Push(new EditorAction("DELETE", deleted));
            _redoStack.Clear();
            Console.WriteLine($"   Удалено {count} симв. (\"{deleted}\") | Текст: \"{Content}\"");
        }

        public void Undo()
        {
            if (_undoStack.Count == 0)
            {
                Console.WriteLine("   [Undo] Нечего отменять!");
                return;
            }

            var action = _undoStack.Pop();
            if (action.Type == "TYPE")
            {
                Content = Content.Substring(0, Content.Length - action.Text.Length);
            }
            else if (action.Type == "DELETE")
            {
                Content += action.Text;
            }

            _redoStack.Push(action);
            Console.WriteLine($"   [Undo] Отменено действие {action.Type}(\"{action.Text}\") | Текст: \"{Content}\"");
        }

        public void Redo()
        {
            if (_redoStack.Count == 0)
            {
                Console.WriteLine("   [Redo] Нечего повторять!");
                return;
            }

            var action = _redoStack.Pop();
            if (action.Type == "TYPE")
            {
                Content += action.Text;
            }
            else if (action.Type == "DELETE")
            {
                Content = Content.Substring(0, Content.Length - action.Text.Length);
            }

            _undoStack.Push(action);
            Console.WriteLine($"   [Redo] Повторено действие {action.Type}(\"{action.Text}\") | Текст: \"{Content}\"");
        }
    }

    public static void RunUndoRedoDemo()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 2. ПРИМЕР ДЛЯ СТРУКТУРЫ 'СТЕК': UNDO / REDO В РЕДАКТОРЕ");
        Console.WriteLine(" Назначение: Отмена и повтор действий пользователя.");
        Console.WriteLine(" Применение: Текстовые редакторы (VS Code, Word), графические пакеты (Photoshop, Figma).");
        Console.WriteLine(" Стек идеально подходит под принцип LIFO (последнее действие отменяется первым).");
        Console.WriteLine("=======================================================");

        var editor = new TextEditor();
        editor.Type("Привет, ");
        editor.Type("мир!");
        editor.Backspace(5);

        Console.WriteLine("\n--- Выполняем 2 отмены (Undo) ---");
        editor.Undo();
        editor.Undo();

        Console.WriteLine("\n--- Выполняем 1 повтор (Redo) ---");
        editor.Redo();

        Console.WriteLine($"\nИтоговый текст: \"{editor.Content}\"");
    }


    // =========================================================================
    // 3. ОЧЕРЕДЬ: Очередь заданий на печать (Print Spooler)
    // =========================================================================
    public class PrintJob
    {
        public int Id { get; }
        public string DocumentName { get; }
        public string UserName { get; }
        public int PageCount { get; }

        public PrintJob(int id, string documentName, string userName, int pageCount)
        {
            Id = id;
            DocumentName = documentName;
            UserName = userName;
            PageCount = pageCount;
        }

        public override string ToString() =>
            $"[Задание #{Id}] \"{DocumentName}\" | Пользователь: {UserName} | Страниц: {PageCount}";
    }

    /// <summary>
    /// Диспетчер очереди печати (Print Spooler).
    /// Применяется в: службах печати операционных систем (Windows Print Spooler spoolsv.exe, CUPS в Linux/macOS),
    /// сетевых принт-серверах и офисных МФУ.
    /// Почему очередь: Принтер — это физически неделимый разделяемый ресурс.
    /// Принцип FIFO (First-In, First-Out) гарантирует, что документы будут напечатаны строго в порядке
    /// поступления запросов без перемешивания страниц разных пользователей.
    /// </summary>
    public class PrintSpooler
    {
        private readonly Queue<PrintJob> _jobQueue = new();
        private int _jobCounter = 1;

        public void SendToPrint(string documentName, string userName, int pages)
        {
            var job = new PrintJob(_jobCounter++, documentName, userName, pages);
            _jobQueue.Enqueue(job);
            Console.WriteLine($"   -> Отправлено в очередь: {job}");
        }

        public void ProcessNextJob()
        {
            if (_jobQueue.Count == 0)
            {
                Console.WriteLine("   Очередь печати пуста. Принтер простаивает.");
                return;
            }

            var job = _jobQueue.Dequeue();
            Console.WriteLine($"   Печать документа: {job} ...");
            Console.WriteLine($"   Успешно напечатано {job.PageCount} стр. документа \"{job.DocumentName}\".");
        }

        public void ProcessAll()
        {
            Console.WriteLine("\n--- Запуск обработки всех заданий в очереди печати ---");
            while (_jobQueue.Count > 0)
            {
                ProcessNextJob();
            }
            Console.WriteLine("--- Все документы в очереди успешно напечатаны! ---");
        }

        public void PrintQueueStatus()
        {
            Console.WriteLine($"\nТекущая очередь печати (всего заданий: {_jobQueue.Count}):");
            if (_jobQueue.Count == 0)
            {
                Console.WriteLine("   [Очередь пуста]");
                return;
            }

            int pos = 1;
            foreach (var job in _jobQueue)
            {
                Console.WriteLine($"   {pos++}. {job}");
            }
        }
    }

    public static void RunPrintSpoolerDemo()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 3. ПРИМЕР ДЛЯ СТРУКТУРЫ 'ОЧЕРЕДЬ': ОЧЕРЕДЬ ЗАДАНИЙ НА ПЕЧАТЬ (PRINT SPOOLER)");
        Console.WriteLine(" Назначение: Управление очередностью печати документов от разных пользователей.");
        Console.WriteLine(" Применение: Служба очереди печати Windows (spoolsv.exe), CUPS (Linux/macOS), принт-серверы.");
        Console.WriteLine(" Очередь FIFO гарантирует печать строго в порядке поступления без перемешивания страниц.");
        Console.WriteLine("=======================================================");

        var spooler = new PrintSpooler();

        Console.WriteLine("1) Пользователи отправляют документы на печать:");
        spooler.SendToPrint("Дипломная_работа.pdf", "Иван", 64);
        spooler.SendToPrint("Договор_аренды.docx", "Анна (Бухгалтерия)", 5);
        spooler.SendToPrint("Отчет_за_квартал.xlsx", "Петр (Аналитик)", 18);
        spooler.SendToPrint("Билеты_на_поезд.pdf", "Ольга", 2);

        spooler.PrintQueueStatus();

        Console.WriteLine("\n2) Принтер печатает первое задание (по FIFO):");
        spooler.ProcessNextJob();

        Console.WriteLine("\n3) В это время поступает еще один срочный документ:");
        spooler.SendToPrint("Акт_выполненных_работ.pdf", "Директор", 3);

        spooler.PrintQueueStatus();

        Console.WriteLine("\n4) Принтер печатает все оставшиеся задания:");
        spooler.ProcessAll();
    }


    // =========================================================================
    // 4. ДЕРЕВЬЯ: Иерархия файловой системы (папки и файлы)
    // =========================================================================
    /// <summary>
    /// Узел n-арного дерева файловой системы.
    /// Может быть либо папкой (содержит дочерние узлы), либо файлом (лист дерева).
    /// </summary>
    public class FileSystemNode
    {
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public long SizeInKb { get; set; } // Для файлов задается, для директорий вычисляется
        public List<FileSystemNode> Children { get; } = new();

        public FileSystemNode(string name, bool isDirectory, long sizeInKb = 0)
        {
            Name = name;
            IsDirectory = isDirectory;
            SizeInKb = sizeInKb;
        }

        public void AddChild(FileSystemNode child)
        {
            if (!IsDirectory)
                throw new InvalidOperationException("Нельзя добавить дочерний элемент в файл, только в директорию!");
            Children.Add(child);
        }

        /// <summary>
        /// Рекурсивный обход дерева (Post-order) для вычисления суммарного размера директории.
        /// Сложность: O(N), где N — количество файлов и подкаталогов.
        /// </summary>
        public long CalculateTotalSize()
        {
            if (!IsDirectory)
                return SizeInKb;

            long total = 0;
            foreach (var child in Children)
            {
                total += child.CalculateTotalSize();
            }
            return total;
        }

        /// <summary>
        /// Поиск файла или папки в поддереве по имени (рекурсивный поиск в глубину DFS).
        /// </summary>
        public FileSystemNode? Find(string name)
        {
            if (string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
                return this;

            foreach (var child in Children)
            {
                var found = child.Find(name);
                if (found != null) return found;
            }

            return null;
        }

        /// <summary>
        /// Наглядная визуализация дерева каталогов с псевдографикой.
        /// </summary>
        public void PrintTree(string indent = "", bool isLast = true)
        {
            string marker = isLast ? "└── " : "├── ";
            string sizeInfo = IsDirectory
                ? $"[DIR: {CalculateTotalSize()} KB]"
                : $"[FILE: {SizeInKb} KB]";

            Console.WriteLine($"{indent}{marker}{Name} {sizeInfo}");

            indent += isLast ? "    " : "│   ";
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].PrintTree(indent, i == Children.Count - 1);
            }
        }
    }

    public static void RunFileSystemTreeDemo()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine(" 4. ПРИМЕР ДЛЯ СТРУКТУРЫ 'ДЕРЕВЬЯ': ИЕРАРХИЯ ФАЙЛОВОЙ СИСТЕМЫ");
        Console.WriteLine(" Назначение: Организация файлов и папок в виде иерархического n-арного дерева.");
        Console.WriteLine(" Применение: Файловые системы ОС (NTFS, ext4, APFS), Проводник Windows / Finder,");
        Console.WriteLine(" облачные хранилища (Google Drive, S3), виртуальные файловые системы.");
        Console.WriteLine(" Дерево позволяет организовывать вложенность любой глубины, быстро находить пути");
        Console.WriteLine(" и рекурсивно вычислять размер каталогов обходом дерева.");
        Console.WriteLine("=======================================================");

        // Создаем корень дерева
        var root = new FileSystemNode("C:", isDirectory: true);

        // Папка Windows
        var dirWindows = new FileSystemNode("Windows", isDirectory: true);
        var dirSys32 = new FileSystemNode("System32", isDirectory: true);
        dirSys32.AddChild(new FileSystemNode("kernel32.dll", isDirectory: false, sizeInKb: 1250));
        dirSys32.AddChild(new FileSystemNode("cmd.exe", isDirectory: false, sizeInKb: 380));
        dirWindows.AddChild(dirSys32);
        dirWindows.AddChild(new FileSystemNode("notepad.exe", isDirectory: false, sizeInKb: 210));
        root.AddChild(dirWindows);

        // Папка Users
        var dirUsers = new FileSystemNode("Users", isDirectory: true);
        var dirStudent = new FileSystemNode("Student", isDirectory: true);

        var dirProjects = new FileSystemNode("Projects", isDirectory: true);
        dirProjects.AddChild(new FileSystemNode("Lab3_Algorithms.cs", isDirectory: false, sizeInKb: 45));
        dirProjects.AddChild(new FileSystemNode("Report.docx", isDirectory: false, sizeInKb: 2800));

        var dirPhotos = new FileSystemNode("Pictures", isDirectory: true);
        dirPhotos.AddChild(new FileSystemNode("photo1.png", isDirectory: false, sizeInKb: 4300));
        dirPhotos.AddChild(new FileSystemNode("photo2.jpg", isDirectory: false, sizeInKb: 3100));

        dirStudent.AddChild(dirProjects);
        dirStudent.AddChild(dirPhotos);
        dirUsers.AddChild(dirStudent);
        root.AddChild(dirUsers);

        Console.WriteLine("1) Структура файлового дерева каталогов (с рекурсивно подсчитанным размером):");
        root.PrintTree();

        Console.WriteLine("\n2) Рекурсивный расчет размера каталогов (обход дерева):");
        Console.WriteLine($"   - Общий размер всего диска 'C:': {root.CalculateTotalSize()} KB ({root.CalculateTotalSize() / 1024.0:F2} MB)");
        Console.WriteLine($"   - Размер папки 'Users': {dirUsers.CalculateTotalSize()} KB");
        Console.WriteLine($"   - Размер папки 'Windows': {dirWindows.CalculateTotalSize()} KB");
        Console.WriteLine($"   - Размер папки 'Pictures': {dirPhotos.CalculateTotalSize()} KB");

        Console.WriteLine("\n3) Поиск узла в дереве по имени (DFS обход):");
        string searchTarget = "Lab3_Algorithms.cs";
        var found = root.Find(searchTarget);
        if (found != null)
        {
            Console.WriteLine($"   Узел '{searchTarget}' найден! Тип: {(found.IsDirectory ? "Папка" : "Файл")}, Размер: {found.SizeInKb} KB");
        }
        else
        {
            Console.WriteLine($"   Узел '{searchTarget}' не найден.");
        }
    }

    /// <summary>
    /// Комплексный запуск всех 4 примеров Задания 3
    /// </summary>
    public static void RunAll()
    {
        RunLruCacheDemo();
        RunUndoRedoDemo();
        RunPrintSpoolerDemo();
        RunFileSystemTreeDemo();
    }
}
