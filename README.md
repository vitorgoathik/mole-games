# mole-games

Monorepo for mobile games built with Godot 4 + C#.

## Structure

```
games/          # one subfolder per game (each is its own Godot project)
shared/
  assets/       # fonts, sounds, sprites shared across games
  themes/       # UI themes
docs/           # game design docs
```

## Requirements

- [Godot Engine 4.x (.NET version)](https://godotengine.org/download) — required for C# support
- [.NET SDK 8+](https://dotnet.microsoft.com/download)

## Adding a new game

1. Create a new Godot project inside `games/<game-name>/`
2. Set the project to use C# (Project > Project Settings > Mono)
3. Add shared assets via symlink or copy from `shared/`
