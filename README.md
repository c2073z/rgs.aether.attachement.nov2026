# Sortle

**A game that teaches bubble sort.**

The game shows you a list of random numbers. You guess how many **swaps** bubble sort will make to sort the list from smallest to biggest. Right answers add time to the clock, and wrong answers take time away. Clear all 5 levels before your time runs out.

You write the main logic of the game yourselves, by hand, in C#. The website version (from our Canva design) gets added later and will use the code you write here.

---

## Who does what

| File | Owner | What it does |
|---|---|---|
| [`StudentWork/BubbleSorter.cs`](StudentWork/BubbleSorter.cs) | **Student A** | Makes the random list and works out the correct answer: how many swaps bubble sort makes. |
| [`StudentWork/ScoreKeeper.cs`](StudentWork/ScoreKeeper.cs) | **Student B** | Runs the game rules: the clock, the levels, time bonuses and penalties, and the mastery rating. |
| `GameEngine/` | (provided) | The terminal version of the game, plus the checker that tests your code. **You don't need to edit anything in here.** |

Each file has **5 steps**. Do them in order. Every step has a comment above it that explains what to build, gives an example, and has hints.

Each of you only edits your own file, so you won't overwrite each other's work.

---

## One-time setup

1. **Install the .NET 8 SDK** from <https://dotnet.microsoft.com/download/dotnet/8.0>. Pick the installer for your computer (macOS Arm64 for newer Macs, x64 for Windows).
   Check that it worked by opening a terminal and typing:
   ```bash
   dotnet --version
   ```
   You should see a number starting with `8.`.
2. **Install VS Code** and its **C# Dev Kit** extension. The extension colours your code and underlines mistakes.
3. **Clone this repo:**
   ```bash
   git clone https://github.com/c2073z/rgs.aether.attachement.nov2026.git Sortle
   ```
   ```bash
   cd Sortle
   ```
4. Open the `Sortle` folder in VS Code.

---

## How to work

1. Open your file in `StudentWork/`.
2. Find the next step. Delete the line `throw new NotImplementedException(...)` and write your code in its place.
3. Check your work in the terminal:

   ```bash
   dotnet run -- check sort
   ```
   That one is for Student A. Student B runs:
   ```bash
   dotnet run -- check score
   ```
   To check both files at once:
   ```bash
   dotnet run -- check
   ```

   What the results mean:
   - `[PASS]`: the step works 🎉
   - `[FAIL]`: your code runs but gives a wrong answer. The message tells you what it got and what it expected.
   - `[TODO]`: you haven't written this step yet, or it needs an earlier step to be done first.

4. Once **both** files pass, play the game:
   ```bash
   dotnet run
   ```

### Saving and sharing your work (git)

When a step passes, save it to GitHub:

```bash
git add StudentWork
```
```bash
git commit -m "BubbleSorter: finished step 2"
```
```bash
git pull
```
```bash
git push
```

Run `git pull` before `git push`, so you also get your teammate's latest work.

---

## Tips

- **Work it out on paper first.** Take `[5, 1, 4, 2]` and do bubble sort by hand, counting the swaps. Then write the code.
- **Read the error message.** `IndexOutOfRangeException` almost always means a loop went one step too far.
- **Only change code inside the `{ }` of the methods.** Don't rename methods or change what they take in or give back. The website will call them by these exact names later.
- Stuck? Use `Console.WriteLine(...)` to print values and see what your code is really doing.
