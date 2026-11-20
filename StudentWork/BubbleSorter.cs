// =====================================================================
//  SORTLE  -  BubbleSorter.cs
//  OWNER: Yuhan
// =====================================================================
//
//  This file is the "brain" of the puzzle. It builds the random list the
//  player sees and works out the CORRECT ANSWER: how many swaps bubble
//  sort needs to sort that list.
//
//  HOW BUBBLE SORT WORKS
//  ---------------------
//  Walk through the list and compare each pair of neighbours.
//  If the left number is BIGGER than the right number, swap them.
//  One full walk through the list is called a "pass".
//  Keep doing passes until a whole pass makes no swaps. Then the list
//  is sorted.
//
//  Example with [5, 1, 4, 2]:
//
//    Pass 1:  [5, 1, 4, 2]   5 > 1  -> swap  -> [1, 5, 4, 2]   (swap 1)
//             [1, 5, 4, 2]   5 > 4  -> swap  -> [1, 4, 5, 2]   (swap 2)
//             [1, 4, 5, 2]   5 > 2  -> swap  -> [1, 4, 2, 5]   (swap 3)
//    Pass 2:  [1, 4, 2, 5]   1 > 4? no
//             [1, 4, 2, 5]   4 > 2  -> swap  -> [1, 2, 4, 5]   (swap 4)
//             [1, 2, 4, 5]   4 > 5? no
//    Pass 3:  no swaps at all -> finished!
//
//    Answer: 4 swaps.
//
//  WHAT TO DO
//  ----------
//  Do the steps IN ORDER (1 -> 5). Each step has a method with a
//  "throw new NotImplementedException(...)" line. Delete that line and
//  write your own code in its place.
//
//  After each step, check your work by running this in the terminal:
//
//      dotnet run -- check sort
//
//  [PASS] means the step works. [FAIL] tells you what went wrong.
//  [TODO] means you haven't started that step yet.
//
//  RULES
//  -----
//  * Only change the code INSIDE the method bodies { ... }.
//  * Do NOT change method names, parameters, or return types. The rest
//    of the game (and the website later on) calls these methods by name.
//
// =====================================================================
namespace Sortle;

public static class BubbleSorter
{
    // -----------------------------------------------------------------
    //  STEP 1: Make the puzzle list
    // -----------------------------------------------------------------
    //  Return a NEW array of whole numbers with `length` items.
    //  Every number must be random, from 1 to 1000 (including both 1 and 1000).
    //
    //  Example:  GenerateRandomList(4, random)  might return  [512, 7, 999, 43]
    //
    //  HINTS
    //  * Make an empty array that has `length` slots:
    //        int[] numbers = new int[length];
    //  * Use a for loop to visit every slot, from index 0 to length - 1.
    //  * random.Next(min, max) gives a random number that is >= min and < max.
    //    The max is NOT included! What should you pass to get 1 to 1000?
    //  * Don't forget to `return` the array at the end.
    // -----------------------------------------------------------------
    public static int[] GenerateRandomList(int length, Random random)
    {
        // TODO (Step 1): delete the line below and write your code.
        int [] numbers = new int[length];
        for (int i = 0; i < length; i++)
        {
            numbers[i] = random.Next(1, 1001);
        }
        return numbers;

        
    }


    // -----------------------------------------------------------------
    //  STEP 2: Swap two items
    // -----------------------------------------------------------------
    //  Swap the item at index i with the item at index j, IN the array.
    //  Nothing is returned (that's what `void` means). You change the
    //  array that was passed in.
    //
    //  Example:  numbers = [10, 20, 30]
    //            Swap(numbers, 0, 2)
    //            numbers is now [30, 20, 10]
    //
    //  HINTS
    //  * You can't just write  numbers[i] = numbers[j];  numbers[j] = numbers[i];
    //    Try it on paper with [10, 20]. What goes wrong?
    //  * Use a third "temporary" variable to hold one value for a moment,
    //    like using a spare cup to swap the drinks in two cups.
    // -----------------------------------------------------------------
    public static void Swap(int[] numbers, int i, int j)
    {
        // TODO (Step 2): delete the line below and write your code.
        int temp = numbers[i];
        numbers [i] = numbers [j];
        numbers [j] = temp;
    }


    // -----------------------------------------------------------------
    //  STEP 3: Is the list sorted?
    // -----------------------------------------------------------------
    //  Return true if the numbers go from smallest to biggest, otherwise false.
    //  Equal neighbours are fine: [2, 2, 3] counts as sorted.
    //  An empty list or a list with 1 number counts as sorted too.
    //
    //  Examples:  IsSorted([1, 2, 3])  -> true
    //             IsSorted([1, 3, 2])  -> false
    //             IsSorted([7])        -> true
    //
    //  HINTS
    //  * Loop through the list and compare each number with the one
    //    NEXT to it: numbers[k] and numbers[k + 1].
    //  * Careful with where the loop stops. If k goes all the way to the
    //    last index, numbers[k + 1] is outside the array and crashes.
    //  * Once you find ONE pair in the wrong order, you can return false
    //    right away. If the loop finishes, every pair was OK, so return true.
    // -----------------------------------------------------------------
    public static bool IsSorted(int[] numbers)
    {
        // TODO (Step 3): delete the line below and write your code.
        for (int k = 0; k < numbers.Length - 1; k++)
        {
            if (numbers[k] > numbers[k + 1])
            {
                return false;
            }
        } 
        return true;

    }


    // -----------------------------------------------------------------
    //  STEP 4: Count the swaps  (THE MAIN ONE!)
    // -----------------------------------------------------------------
    //  Bubble sort the numbers and return HOW MANY SWAPS it took.
    //  This number is the correct answer the player is trying to guess.
    //
    //  Examples:  CountSwaps([1, 2, 3])        -> 0   (already sorted)
    //             CountSwaps([2, 1])           -> 1
    //             CountSwaps([3, 2, 1])        -> 3
    //             CountSwaps([5, 1, 4, 2, 8])  -> 4
    //
    //  IMPORTANT: do NOT change the array that was passed in. The game
    //  still needs to show the player the unsorted list. Sort a COPY:
    //        int[] copy = (int[])numbers.Clone();
    //
    //  HINTS
    //  * Make a counter:  int swaps = 0;
    //  * You need a loop INSIDE a loop:
    //      - the inner loop walks along the list comparing neighbours
    //        (just like in Step 3), and when the left one is bigger it
    //        calls your Swap method from Step 2 and adds 1 to the counter;
    //      - the outer loop repeats passes until the list is sorted.
    //        You could use your IsSorted method from Step 3, or a
    //        bool that remembers "did I swap anything this pass?".
    //  * Only swap when left > right, NOT when they are equal.
    //  * Work through [3, 2, 1] on paper first. You should get 3 swaps.
    // -----------------------------------------------------------------
    public static int CountSwaps(int[] numbers)
    {
        // TODO (Step 4): delete the line below and write your code.
        int[] copy = (int[])numbers.Clone();
        int swaps = 0;
        while (!IsSorted(copy))
        {
            for (int k = 0; k < copy.Length - 1; k++)
            {
                if (copy[k] > copy[k + 1])
                {
                    Swap(copy, k, k + 1);
                    swaps++;
                }
            }

        }
        return swaps;
    }


    // -----------------------------------------------------------------
    //  STEP 5 (CHALLENGE): Record every step of the sort
    // -----------------------------------------------------------------
    //  This one powers the "show me how" replay. When a player gets an
    //  answer wrong, the game replays the sort one swap at a time so they
    //  can see where they went wrong.
    //
    //  Do the same bubble sort as Step 4, but every time you make a swap,
    //  save a SNAPSHOT (a copy) of the whole list right after the swap.
    //  Return the list of all snapshots, in order.
    //
    //  Example:  GetSortSteps([3, 2, 1]) returns 3 snapshots:
    //               [2, 3, 1]    <- after swap 1
    //               [2, 1, 3]    <- after swap 2
    //               [1, 2, 3]    <- after swap 3
    //
    //  An already sorted list has no swaps, so it returns an empty list.
    //  Just like Step 4, don't change the array that was passed in.
    //
    //  HINTS
    //  * Start with:   List<int[]> steps = new List<int[]>();
    //  * Add a snapshot with:   steps.Add((int[])copy.Clone());
    //    Why Clone() again? If you add `copy` itself, every snapshot points
    //    at the SAME array, and they all end up as the final sorted list.
    //  * Check: steps.Count should equal CountSwaps(numbers).
    // -----------------------------------------------------------------
    public static List<int[]> GetSortSteps(int[] numbers)
    {
        // TODO (Step 5): delete the line below and write your code.
        List<int[]> steps = new List<int[]>();
        int[] copy = (int[])numbers.Clone();
        while (!IsSorted(copy))
        {
            for (int k = 0; k < copy.Length - 1; k++)
            {
                if (copy[k] > copy[k + 1])
                {
                    Swap(copy, k, k + 1);
                    steps.Add((int[])copy.Clone());
                }
            }

        }
        return steps;
    }
}

