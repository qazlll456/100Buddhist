# 100Buddhist Plugin for CounterStrikeSharp

A CounterStrikeSharp plugin that broadcasts 100 Buddhist teachings in-game, supporting multiple languages, configurable message filters, and broadcast modes.

## Features

- Broadcasts 100 Buddhist teachings with customizable intervals (default: 30 seconds).
- Supports `Sequential`, `FullRandom`, and `ShuffleRandom` message selection.
- Public or private broadcast modes (toggle via `!100buddhist`).
- Filter messages by ID range (`MessageGroups`) or text content (`TextSearches`).
- Multi-language support (English, Traditional Chinese).
- Color-coded messages for visual appeal.
- Admin commands for self-check and reload (`css_100buddhist`).

![image](https://github.com/qazlll456/100Buddhist/blob/master/buddhist1.png)

## Donate
If you enjoy it and find it helpful, consider donating to me! Every bit helps me keep developing.
Money, Steam games, or any valuable contribution is welcome.
- **Ko-fi**: [Support on Ko-fi](https://ko-fi.com/qazlll456)
- **Patreon**: [Become a Patron](https://www.patreon.com/c/qazlll456)
- **Streamlabs**: [Tip via Streamlabs](https://streamlabs.com/BKCqazlll456/tip)

## Requirements

- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) (latest version).
- CS2 server with .NET Core 3.1 support.
- Write permissions for `csgo/addons/counterstrikesharp/plugins/100Buddhist/language/`.

## Installation

1. Clone or download this repository.
2. Build the project:
   ```bash
   dotnet build -c Release
   ```
3. Copy the built plugin to your CS2 server:
   - Copy `100Buddhist/bin/Release/netcoreapp3.1/100Buddhist.dll` to `csgo/addons/counterstrikesharp/plugins/100Buddhist/`.
   - Copy `100Buddhist/configs/config.json` to `csgo/addons/counterstrikesharp/plugins/100Buddhist/configs/`.
4. Start your CS2 server. The plugin will generate `english.json` and `t-chinese.json` in `csgo/addons/counterstrikesharp/plugins/100Buddhist/language/` on first run.

## Configuration

Edit `csgo/addons/counterstrikesharp/plugins/100Buddhist/configs/config.json`:

```json
{
  "BroadcastInterval": 30,
  "RandomMode": "Sequential",
  "LanguageFile": "english.json",
  "BroadcastMode": "public",
  "MessageGroups": [],
  "TextSearches": []
}
```

### Configuration Options

- **BroadcastInterval**: Time (in seconds) between broadcasts (e.g., `30`).
- **RandomMode**: `Sequential`, `FullRandom`, or `ShuffleRandom`.
- **LanguageFile**: Language file (e.g., `english.json`, `t-chinese.json`).
- **BroadcastMode**: `public` (all players) or `private` (subscribed players).
- **MessageGroups**: ID ranges (e.g., `["1-5"]` for messages 1-5). Empty for all messages.
- **TextSearches**: Text filters (e.g., `["Buddha"]` for messages containing "Buddha").

> **Note**: JSON does not support comments. Ensure `config.json` is valid JSON.

## Commands

### Admin Commands
Run in console or by admin players:

- `css_100buddhist`: Self-check, showing config and language status.
- `css_100buddhist reload`: Reloads `config.json` and language files.

### Player Commands
Run in-game via chat:

- `!100buddhist`: Toggles private mode subscription (if `BroadcastMode: "private"`).
- `!100buddhist language list`: Lists available languages.
- `!100buddhist language <name>`: Sets player language (e.g., `t-chinese`).

## Language Files

- Located in `csgo/addons/counterstrikesharp/plugins/100Buddhist/language/`.
- Default files: `english.json` and `t-chinese.json` (100 messages each).
- Format:
  ```json
  {
    "MessagePrefix": "{green}100Buddhist {0}: {white}{1}",
    "Messages": [
      { "Id": 1, "Text": "{white}All that we are is the result of what we have thought. - Buddha, Dhammapada 1" },
      // ... 100 messages
    ]
  }
  ```

## Development

- Built with .NET Core 3.1 and CounterStrikeSharp.
- Source files: `CommandManager.cs`, `ConfigManager.cs`, `BroadcastManager.cs`, `LanguageManager.cs`, `ChatFormatter.cs`, `BuddhistPlugin.cs`.
- To build:
  ```bash
  dotnet build -c Release
  ```

## License

MIT License. See [LICENSE](LICENSE) for details.

## Contributing

Open issues or submit pull requests on GitHub.

## Credits

Developed by qazlll456 with assistance from Grok (xAI).
