// =====================================================================
//  SORTLE  -  SelfCheck.cs   (PROVIDED: students don't need to edit this)
// =====================================================================
//  Automatic checker for the two student files.
//
//    dotnet run -- check         -> check both files
//    dotnet run -- check sort    -> check BubbleSorter.cs only
//    dotnet run -- check score   -> check ScoreKeeper.cs only
//
//  [PASS] = that step works
//  [FAIL] = that step runs but gives a wrong answer (the message says what)
//  [TODO] = that step (or a step it depends on) isn't written yet
// =====================================================================

namespace Sortle;

public static class SelfCheck
{
    private static int _passed;
    private static int _failed;
    private static int _todo;

    public static int Run(string which)
    {
        bool checkSort = which == "all" || which == "sort";
        bool checkScore = which == "all" || which == "score";

        if (!checkSort && !checkScore)
        {
            Console.WriteLine("Unknown check. Use one of:");
            Console.WriteLine("  dotnet run -- check");
            Console.WriteLine("  dotnet run -- check sort");
            Console.WriteLine("  dotnet run -- check score");
            return 1;
        }

        if (checkSort)
        {
            CheckBubbleSorter();
        }

        if (checkScore)
        {
            CheckScoreKeeper();
        }

        Console.WriteLine();
        Console.WriteLine($"Passed: {_passed}   Failed: {_failed}   To do: {_todo}");
        if (_failed == 0 && _todo == 0)
        {
            Console.WriteLine("Everything works! Try playing:  dotnet run");
        }

        return _failed > 0 ? 1 : 0;
    }

    // =================================================================
    //  BubbleSorter.cs  (Student A)
    // =================================================================
    private static void CheckBubbleSorter()
    {
        Console.WriteLine();
        Console.WriteLine("---- BubbleSorter.cs (Student A) ----");

        Step("Step 1: GenerateRandomList", () =>
        {
            var random = new Random(1);
            foreach (int length in new[] { 3, 10, 25 })
            {
                int[] list = BubbleSorter.GenerateRandomList(length, random);
                if (list == null)
                {
                    return "returned null. Did you forget to return the array?";
                }
                if (list.Length != length)
                {
                    return $"GenerateRandomList({length}, ...) gave {list.Length} numbers, expected {length}.";
                }
            }

            int[] lots = BubbleSorter.GenerateRandomList(20000, random);
            foreach (int n in lots)
            {
                if (n < 1 || n > 1000)
                {
                    return $"found the number {n}. Every number must be from 1 to 1000.";
                }
            }
            if (!lots.Contains(1000))
            {
                return "never made the number 1000. Remember random.Next(min, max) does NOT include max.";
            }
            if (!lots.Contains(1))
            {
                return "never made the number 1. The smallest allowed number is 1.";
            }
            if (lots.Distinct().Count() < 500)
            {
                return "the numbers don't look random. Are you calling random.Next(...) for every slot?";
            }
            return null;
        });

        Step("Step 2: Swap", () =>
        {
            int[] a = { 10, 20, 30 };
            BubbleSorter.Swap(a, 0, 2);
            if (!Same(a, new[] { 30, 20, 10 }))
            {
                return $"Swap([10, 20, 30], 0, 2) changed the array to {Show(a)}, expected [30, 20, 10].";
            }

            int[] b = { 4, 9 };
            BubbleSorter.Swap(b, 0, 1);
            if (!Same(b, new[] { 9, 4 }))
            {
                return $"Swap([4, 9], 0, 1) changed the array to {Show(b)}, expected [9, 4].";
            }

            int[] c = { 1, 2, 3 };
            BubbleSorter.Swap(c, 1, 1);
            if (!Same(c, new[] { 1, 2, 3 }))
            {
                return $"Swap([1, 2, 3], 1, 1) changed the array to {Show(c)}, expected no change.";
            }
            return null;
        });

        Step("Step 3: IsSorted", () =>
        {
            var cases = new (int[] list, bool expected)[]
            {
                (new[] { 1, 2, 3 }, true),
                (new[] { 1, 3, 2 }, false),
                (new[] { 3, 2, 1 }, false),
                (new[] { 2, 2, 3 }, true),
                (new[] { 5, 5, 5 }, true),
                (new[] { 1, 2, 3, 4, 0 }, false),
                (new[] { 9, 1, 2, 3, 4 }, false),
                (new[] { 7 }, true),
                (new int[0], true),
            };
            foreach (var (list, expected) in cases)
            {
                bool actual = BubbleSorter.IsSorted(list);
                if (actual != expected)
                {
                    return $"IsSorted({Show(list)}) returned {Lower(actual)}, expected {Lower(expected)}.";
                }
            }
            return null;
        });

        Step("Step 4: CountSwaps", () =>
        {
            var cases = new (int[] list, int expected)[]
            {
                (new[] { 1, 2, 3 }, 0),
                (new[] { 2, 1 }, 1),
                (new[] { 3, 2, 1 }, 3),
                (new[] { 5, 1, 4, 2, 8 }, 4),
                (new[] { 4, 3, 2, 1 }, 6),
                (new[] { 2, 2, 1 }, 2),
                (new[] { 3, 3, 3 }, 0),
                (new[] { 7 }, 0),
            };
            foreach (var (list, expected) in cases)
            {
                int[] original = (int[])list.Clone();
                int actual = BubbleSorter.CountSwaps(list);
                if (actual != expected)
                {
                    return $"CountSwaps({Show(original)}) returned {actual}, expected {expected}.";
                }
                if (!Same(list, original))
                {
                    return $"CountSwaps changed the player's list from {Show(original)} to {Show(list)}. Sort a COPY instead (use Clone()).";
                }
            }

            // Bigger random lists, like the real game uses.
            var random = new Random(2026);
            for (int trial = 0; trial < 50; trial++)
            {
                int length = random.Next(3, 26);
                int[] list = new int[length];
                for (int k = 0; k < length; k++)
                {
                    // Small range on purpose, so there are lots of equal numbers.
                    list[k] = random.Next(1, 30);
                }
                int expected = ReferenceSwapCount(list);
                int actual = BubbleSorter.CountSwaps(list);
                if (actual != expected)
                {
                    return $"CountSwaps({Show(list)}) returned {actual}, expected {expected}.";
                }
            }
            return null;
        });

        Step("Step 5: GetSortSteps (challenge)", () =>
        {
            int[] input = { 3, 2, 1 };
            List<int[]> steps = BubbleSorter.GetSortSteps(input);
            if (steps == null)
            {
                return "returned null. Did you forget to return the list?";
            }
            if (!Same(input, new[] { 3, 2, 1 }))
            {
                return $"GetSortSteps changed the player's list to {Show(input)}. Sort a COPY instead.";
            }
            var expected = new[]
            {
                new[] { 2, 3, 1 },
                new[] { 2, 1, 3 },
                new[] { 1, 2, 3 },
            };
            if (steps.Count != expected.Length)
            {
                return $"GetSortSteps([3, 2, 1]) gave {steps.Count} snapshots, expected 3 (one per swap).";
            }
            for (int k = 0; k < expected.Length; k++)
            {
                if (!Same(steps[k], expected[k]))
                {
                    string extra = Same(steps[k], new[] { 1, 2, 3 })
                        ? " Every snapshot looks sorted. Are you adding a Clone() of the list, not the list itself?"
                        : "";
                    return $"snapshot {k + 1} of GetSortSteps([3, 2, 1]) is {Show(steps[k])}, expected {Show(expected[k])}.{extra}";
                }
            }

            if (BubbleSorter.GetSortSteps(new[] { 1, 2, 3 }).Count != 0)
            {
                return "an already sorted list should give an empty list of snapshots.";
            }

            var random = new Random(7);
            for (int trial = 0; trial < 20; trial++)
            {
                int[] list = new int[random.Next(3, 15)];
                for (int k = 0; k < list.Length; k++)
                {
                    list[k] = random.Next(1, 50);
                }
                int swaps = ReferenceSwapCount(list);
                List<int[]> s = BubbleSorter.GetSortSteps(list);
                if (s.Count != swaps)
                {
                    return $"GetSortSteps({Show(list)}) gave {s.Count} snapshots, but bubble sort makes {swaps} swaps.";
                }
            }
            return null;
        });
    }

    // =================================================================
    //  ScoreKeeper.cs  (Student B)
    // =================================================================
    private static void CheckScoreKeeper()
    {
        Console.WriteLine();
        Console.WriteLine("---- ScoreKeeper.cs (Student B) ----");

        Step("Step 1: GetMinLength / GetMaxLength", () =>
        {
            var s = new ScoreKeeper();
            int[] mins = { 0, 3, 7, 10, 15, 20, 0 };   // levels 0..6
            int[] maxs = { 0, 5, 10, 15, 20, 25, 0 };
            for (int level = 0; level <= 6; level++)
            {
                int min = s.GetMinLength(level);
                if (min != mins[level])
                {
                    return $"GetMinLength({level}) returned {min}, expected {mins[level]}.";
                }
                int max = s.GetMaxLength(level);
                if (max != maxs[level])
                {
                    return $"GetMaxLength({level}) returned {max}, expected {maxs[level]}.";
                }
            }
            return null;
        });

        Step("Step 2: GetTimeBonus / GetTimePenalty", () =>
        {
            var s = new ScoreKeeper();
            int[] bonus = { 0, 5, 10, 15, 20, 25, 0 };   // levels 0..6
            int[] penalty = { 0, 5, 5, 7, 7, 7, 0 };
            for (int level = 0; level <= 6; level++)
            {
                int b = s.GetTimeBonus(level);
                if (b != bonus[level])
                {
                    return $"GetTimeBonus({level}) returned {b}, expected {bonus[level]}.";
                }
                int p = s.GetTimePenalty(level);
                if (p != penalty[level])
                {
                    return $"GetTimePenalty({level}) returned {p}, expected {penalty[level]}.";
                }
            }
            return null;
        });

        Step("Step 3: Tick", () =>
        {
            var s = new ScoreKeeper();
            s.Tick(10);
            if (!Near(s.TimeLeft, 50))
            {
                return $"new game, Tick(10): TimeLeft is {s.TimeLeft}, expected 50.";
            }
            s.Tick(0.5);
            if (!Near(s.TimeLeft, 49.5))
            {
                return $"then Tick(0.5): TimeLeft is {s.TimeLeft}, expected 49.5.";
            }
            if (s.IsGameOver)
            {
                return "IsGameOver became true while there was still time left.";
            }
            s.Tick(100);
            if (!Near(s.TimeLeft, 0))
            {
                return $"then Tick(100): TimeLeft is {s.TimeLeft}, expected 0 (no negative time).";
            }
            if (!s.IsGameOver)
            {
                return "the clock hit 0 but IsGameOver is still false.";
            }
            s.Tick(5);
            if (!Near(s.TimeLeft, 0) || !s.IsGameOver)
            {
                return "Tick after the game is over should do nothing.";
            }

            var exact = new ScoreKeeper();
            exact.Tick(60);
            if (!exact.IsGameOver || !Near(exact.TimeLeft, 0))
            {
                return "new game, Tick(60): the clock is exactly 0, so the game should be over.";
            }
            return null;
        });

        Step("Step 4: SubmitAnswer", () =>
        {
            var s = new ScoreKeeper();
            s.SubmitAnswer(true);
            string? problem = Expect(s, "new game, 1 correct answer",
                time: 65, level: 1, solved: 1, correct: 1, wrong: 0, cleared: 0, over: false, won: false);
            if (problem != null) return problem;

            s.SubmitAnswer(false);
            problem = Expect(s, "new game, 1 correct then 1 wrong",
                time: 60, level: 1, solved: 1, correct: 1, wrong: 1, cleared: 0, over: false, won: false);
            if (problem != null) return problem;

            var t = new ScoreKeeper();
            Answer(t, true, 5);
            problem = Expect(t, "new game, 5 correct answers",
                time: 85, level: 2, solved: 0, correct: 5, wrong: 0, cleared: 1, over: false, won: false);
            if (problem != null) return problem;

            t.SubmitAnswer(false);
            problem = Expect(t, "level 2, then 1 wrong answer (-5s)",
                time: 80, level: 2, solved: 0, correct: 5, wrong: 1, cleared: 1, over: false, won: false);
            if (problem != null) return problem;

            Answer(t, true, 5);
            t.SubmitAnswer(false);
            problem = Expect(t, "clear level 2, then 1 wrong answer at level 3 (-7s)",
                time: 123, level: 3, solved: 0, correct: 10, wrong: 2, cleared: 2, over: false, won: false);
            if (problem != null) return problem;

            var u = new ScoreKeeper();
            u.Tick(57);
            u.SubmitAnswer(false);
            problem = Expect(u, "3 seconds left, then 1 wrong answer",
                time: 0, level: 1, solved: 0, correct: 0, wrong: 1, cleared: 0, over: true, won: false);
            if (problem != null) return problem;

            u.SubmitAnswer(true);
            problem = Expect(u, "answering after the game is over (should change nothing)",
                time: 0, level: 1, solved: 0, correct: 0, wrong: 1, cleared: 0, over: true, won: false);
            if (problem != null) return problem;

            var w = new ScoreKeeper();
            Answer(w, true, 25);
            problem = Expect(w, "new game, 25 correct answers (beat the game)",
                time: 435, level: 5, solved: 5, correct: 25, wrong: 0, cleared: 5, over: true, won: true);
            if (problem != null) return problem;

            return null;
        });

        Step("Step 5: GetMasteryRating", () =>
        {
            string[] titles =
            {
                "Not Yet Sorted",
                "Bubble Beginner",
                "Swap Apprentice",
                "Sorting Student",
                "Bubble Expert",
                "Bubble Sort Master",
            };
            for (int levels = 0; levels <= 5; levels++)
            {
                var s = new ScoreKeeper();
                Answer(s, true, levels * ScoreKeeper.ProblemsPerLevel);   // uses Step 4
                string rating = s.GetMasteryRating();
                if (rating != titles[levels])
                {
                    return $"with {levels} level(s) cleared, GetMasteryRating() returned \"{rating}\", expected \"{titles[levels]}\".";
                }
            }
            return null;
        });
    }

    // =================================================================
    //  Helpers
    // =================================================================

    // Runs one step's test and prints PASS / FAIL / TODO.
    private static void Step(string name, Func<string?> test)
    {
        try
        {
            string? problem = test();
            if (problem == null)
            {
                _passed++;
                Console.WriteLine($"  [PASS] {name}");
            }
            else
            {
                _failed++;
                Console.WriteLine($"  [FAIL] {name}");
                Console.WriteLine($"         {problem}");
            }
        }
        catch (NotImplementedException ex)
        {
            _todo++;
            Console.WriteLine($"  [TODO] {name}   (waiting on: {ex.Message})");
        }
        catch (Exception ex)
        {
            _failed++;
            Console.WriteLine($"  [FAIL] {name}");
            Console.WriteLine($"         Your code crashed with {ex.GetType().Name}: {ex.Message}");
            if (ex is IndexOutOfRangeException)
            {
                Console.WriteLine("         (Tip: a loop is probably going one index too far.)");
            }
        }
    }

    private static void Answer(ScoreKeeper s, bool isCorrect, int times)
    {
        for (int k = 0; k < times; k++)
        {
            s.SubmitAnswer(isCorrect);
        }
    }

    private static string? Expect(ScoreKeeper s, string situation,
        double time, int level, int solved, int correct, int wrong, int cleared, bool over, bool won)
    {
        var mismatches = new List<string>();
        if (!Near(s.TimeLeft, time)) mismatches.Add($"TimeLeft is {s.TimeLeft}, expected {time}");
        if (s.CurrentLevel != level) mismatches.Add($"CurrentLevel is {s.CurrentLevel}, expected {level}");
        if (s.ProblemsSolvedThisLevel != solved) mismatches.Add($"ProblemsSolvedThisLevel is {s.ProblemsSolvedThisLevel}, expected {solved}");
        if (s.TotalCorrect != correct) mismatches.Add($"TotalCorrect is {s.TotalCorrect}, expected {correct}");
        if (s.TotalWrong != wrong) mismatches.Add($"TotalWrong is {s.TotalWrong}, expected {wrong}");
        if (s.LevelsCleared != cleared) mismatches.Add($"LevelsCleared is {s.LevelsCleared}, expected {cleared}");
        if (s.IsGameOver != over) mismatches.Add($"IsGameOver is {Lower(s.IsGameOver)}, expected {Lower(over)}");
        if (s.HasWonGame != won) mismatches.Add($"HasWonGame is {Lower(s.HasWonGame)}, expected {Lower(won)}");

        if (mismatches.Count == 0)
        {
            return null;
        }
        return $"after {situation}: " + string.Join("; ", mismatches) + ".";
    }

    // Counts swaps a different way (counting out-of-order pairs), so we can
    // check the student's bubble sort without using bubble sort here.
    private static int ReferenceSwapCount(int[] list)
    {
        int count = 0;
        for (int a = 0; a < list.Length; a++)
        {
            for (int b = a + 1; b < list.Length; b++)
            {
                if (list[a] > list[b])
                {
                    count++;
                }
            }
        }
        return count;
    }

    private static bool Same(int[] a, int[] b) => a != null && b != null && a.SequenceEqual(b);

    private static bool Near(double a, double b) => Math.Abs(a - b) < 0.001;

    private static string Show(int[] list) => list == null ? "null" : "[" + string.Join(", ", list) + "]";

    private static string Lower(bool value) => value ? "true" : "false";
}
