# Offline Wordle

A Wordle clone for the terminal, written in C#. It runs fully offline: the word list is embedded in the executable, so no internet connection is needed.

> Version 1.0 - 18.11.2026

## Features

- Play Wordle in the console with colored feedback
- Built-in word list (embedded resource, no external files required)
- 5 attempts per game
- Correct handling of repeated letters (same rules as the original game)
- Typewriter-style intro animation

### Planned

- [ ] Play with a custom word list (menu option `C`)
- [ ] "About" screen (menu option `A`)
- [ ] Reject guesses that are not in the word list
- [ ] Reveal the word when you lose

## How to play

1. Start the game and press `P` to play with the default word list.
2. The game tells you how many letters the secret word has.
3. Type a guess of the same length and press Enter.
4. Each letter is colored to show how close you are:

| Color  | Meaning                                        |
|--------|------------------------------------------------|
| Green  | Correct letter in the correct position         |
| Yellow | Letter is in the word, but in a different spot |
| Red    | Letter is not in the word (or used up already) |

5. You have 5 attempts. Guess the word before they run out.

Guesses with the wrong length are discarded and do not use an attempt.

### Menu

```
  1. Play Wordle with default wordlist. (Press P)
  2. Play Wordle with custom wordlist. (Press C)
  3. About the application. (Press A)
```

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) 6.0 or newer
- A terminal that supports colors

## Build and run

```bash
git clone https://github.com/UygarDRK/offline-wordle
cd offline-wordle
dotnet run
```

To build a release version:

```bash
dotnet build -c Release
```

## Project structure

```
offline-wordle/
├── Program.cs                          # Game logic and menu
├── wordlists/
│   └── valid-wordle-words.txt          # Default word list (embedded resource)
└── offline-wordle.csproj
```

The word list is compiled into the program as an embedded resource and read at startup with `Assembly.GetManifestResourceStream`. In the `.csproj` it must be registered like this:

```xml
<ItemGroup>
  <EmbeddedResource Include="wordlists\valid-wordle-words.txt" />
</ItemGroup>
```

The resource name used in code is `offline_wordle.wordlists.valid-wordle-words.txt` (namespace + folder + file name).

## How the coloring works

The guess is evaluated in two passes so repeated letters behave correctly:

1. **Green pass:** every letter that matches the secret word at the same position is marked green and removed from the pool of available letters.
2. **Yellow pass:** every remaining letter is looked up in the pool. If found, it is marked yellow and removed from the pool, so each letter in the secret word can only be matched once. Everything else stays red.

For example, with the secret word `terra`:

| Guess   | Result                                  |
|---------|-----------------------------------------|
| `rrrrr` | red red green green red                 |
| `alert` | yellow red yellow green yellow          |
| `error` | yellow yellow green red red             |

## Word list

The default list is `valid-wordle-words.txt`. Add the source and its license here if you redistribute it.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Author

Made by Uygar Dirik, https://uygardirik.com.tr.
