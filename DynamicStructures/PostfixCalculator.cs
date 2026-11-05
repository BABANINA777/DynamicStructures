using System.Globalization;

namespace DynamicStructures;

/// <summary>
/// Реализация вычисления постфиксных выражений и алгоритма Дейкстры (Shunting-yard)
/// для перевода инфиксной записи в постфиксную.
/// Поддерживаемые операторы: +, -, *, /, :, ^, ln, cos, sin, sqrt.
/// </summary>
public static class PostfixCalculator
{
    private static readonly HashSet<string> UnaryFunctions = new(StringComparer.OrdinalIgnoreCase)
    {
        "ln", "cos", "sin", "sqrt"
    };

    private static int GetPrecedence(string op)
    {
        return op switch
        {
            "+" or "-" => 1,
            "*" or "/" or ":" => 2,
            "^" => 3,
            "ln" or "cos" or "sin" or "sqrt" => 4,
            _ => 0
        };
    }

    private static bool IsRightAssociative(string op) => op == "^";

    /// <summary>
    /// Перевод инфиксной записи (например "7 + 8 * ( 3 + 2 )") в постфиксную ("7 8 3 2 + * +")
    /// Алгоритм "Сортировочная станция" (Shunting-yard) Дейкстры.
    /// Сложность: O(N) по времени, O(N) по памяти.
    /// </summary>
    public static string InfixToPostfix(string infix)
    {
        if (string.IsNullOrWhiteSpace(infix)) return "";

        // Нормализация скобок и пробелов
        string spaced = infix
            .Replace("(", " ( ")
            .Replace(")", " ) ")
            .Replace("+", " + ")
            .Replace("-", " - ")
            .Replace("*", " * ")
            .Replace("/", " / ")
            .Replace(":", " : ")
            .Replace("^", " ^ ");

        string[] tokens = spaced.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var output = new List<string>();
        Node? opStack = null;

        foreach (var token in tokens)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _) ||
                double.TryParse(token, NumberStyles.Any, new CultureInfo("ru-RU"), out _))
            {
                output.Add(token);
            }
            else if (UnaryFunctions.Contains(token))
            {
                Stack.Push(ref opStack, token);
            }
            else if (token is "+" or "-" or "*" or "/" or ":" or "^")
            {
                string normOp = token == ":" ? "/" : token;
                while (!Stack.IsEmpty(opStack))
                {
                    string topOp = Stack.Top(opStack)!;
                    if (topOp == "(") break;

                    int topPrec = GetPrecedence(topOp);
                    int currentPrec = GetPrecedence(normOp);

                    if (topPrec > currentPrec || (topPrec == currentPrec && !IsRightAssociative(normOp)))
                    {
                        output.Add(Stack.Pop(ref opStack));
                    }
                    else
                    {
                        break;
                    }
                }
                Stack.Push(ref opStack, normOp);
            }
            else if (token == "(")
            {
                Stack.Push(ref opStack, "(");
            }
            else if (token == ")")
            {
                bool foundOpening = false;
                while (!Stack.IsEmpty(opStack))
                {
                    string topOp = Stack.Pop(ref opStack);
                    if (topOp == "(")
                    {
                        foundOpening = true;
                        break;
                    }
                    output.Add(topOp);
                }

                if (!foundOpening)
                    throw new ArgumentException("Несбалансированные скобки в выражении!");

                if (!Stack.IsEmpty(opStack) && UnaryFunctions.Contains(Stack.Top(opStack)!))
                {
                    output.Add(Stack.Pop(ref opStack));
                }
            }
            else
            {
                throw new ArgumentException($"Неизвестный символ: {token}");
            }
        }

        while (!Stack.IsEmpty(opStack))
        {
            string topOp = Stack.Pop(ref opStack);
            if (topOp is "(" or ")")
                throw new ArgumentException("Несбалансированные скобки в выражении!");
            output.Add(topOp);
        }

        return string.Join(" ", output);
    }

    /// <summary>
    /// Вычисление значения постфиксного выражения (RPN) с использованием стека.
    /// Поддерживает числа и операторы +, -, *, /, :, ^, ln, cos, sin, sqrt.
    /// Сложность: O(N) по времени, O(N) по памяти.
    /// </summary>
    public static double EvaluatePostfix(string postfix)
    {
        if (string.IsNullOrWhiteSpace(postfix))
            throw new ArgumentException("Выражение пусто!");

        string[] tokens = postfix.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Node? stack = null;

        foreach (var token in tokens)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double num) ||
                double.TryParse(token, NumberStyles.Any, new CultureInfo("ru-RU"), out num))
            {
                Stack.Push(ref stack, num.ToString(CultureInfo.InvariantCulture));
            }
            else if (UnaryFunctions.Contains(token))
            {
                if (Stack.IsEmpty(stack))
                    throw new InvalidOperationException($"Недостаточно аргументов для унарной функции '{token}'");

                double val = double.Parse(Stack.Pop(ref stack), CultureInfo.InvariantCulture);
                double res = token.ToLowerInvariant() switch
                {
                    "ln" => val <= 0 ? throw new ArgumentException("ln от неположительного числа") : Math.Log(val),
                    "cos" => Math.Cos(val),
                    "sin" => Math.Sin(val),
                    "sqrt" => val < 0 ? throw new ArgumentException("sqrt от отрицательного числа") : Math.Sqrt(val),
                    _ => throw new InvalidOperationException($"Неизвестная функция '{token}'")
                };

                Stack.Push(ref stack, res.ToString(CultureInfo.InvariantCulture));
            }
            else if (token is "+" or "-" or "*" or "/" or ":" or "^")
            {
                if (Stack.IsEmpty(stack))
                    throw new InvalidOperationException($"Недостаточно аргументов для бинарного оператора '{token}'");
                double b = double.Parse(Stack.Pop(ref stack), CultureInfo.InvariantCulture);

                if (Stack.IsEmpty(stack))
                    throw new InvalidOperationException($"Недостаточно аргументов для бинарного оператора '{token}'");
                double a = double.Parse(Stack.Pop(ref stack), CultureInfo.InvariantCulture);

                double res = token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" or ":" => b == 0 ? throw new DivideByZeroException("Деление на ноль!") : a / b,
                    "^" => Math.Pow(a, b),
                    _ => throw new InvalidOperationException()
                };

                Stack.Push(ref stack, res.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                throw new ArgumentException($"Неизвестный токен в постфиксном выражении: '{token}'");
            }
        }

        if (Stack.IsEmpty(stack))
            throw new InvalidOperationException("Стек пуст после вычисления!");

        double finalResult = double.Parse(Stack.Pop(ref stack), CultureInfo.InvariantCulture);

        if (!Stack.IsEmpty(stack))
            throw new InvalidOperationException("Ошибка: в выражении остались лишние операнды!");

        return finalResult;
    }

    /// <summary>
    /// Теоретический расчет и объяснение сложности алгоритма
    /// </summary>
    public static string GetComplexityReport()
    {
        return @"
========================================================================
            ОЦЕНКА СЛОЖНОСТИ ВЫЧИСЛЕНИЯ ПОСТФИКСНОЙ ЗАПИСИ
========================================================================
1. Временная сложность (Time Complexity): O(N), где N — количество токенов.
   - Алгоритм делает ровно 1 проход слева направо по массиву токенов (N шагов).
   - Для каждого числа выполняется 1 операция Push (O(1)).
   - Для каждой унарной функции: 1 Pop, 1 расчет функции, 1 Push (O(1)).
   - Для каждого бинарного оператора: 2 Pop, 1 базовая арифметика, 1 Push (O(1)).
   - Суммарное время: O(N) * O(1) = O(N) — линейная сложность.

2. Пространственная сложность (Space Complexity): O(N).
   - В худшем случае (например, все числа идут подряд перед операторами: '1 2 3 4 5 + + + +')
     стек операндов содержит до M чисел, где M <= N.
   - Дополнительная память: O(N) для хранения стека.
========================================================================";
    }
}
