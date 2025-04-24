using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Collections.Generic;

namespace BuddhistPlugin
{
    public class BuddhistPlugin : BasePlugin
    {
        public override string ModuleName => "100Buddhist";
        public override string ModuleVersion => "1.0.0";
        public override string ModuleAuthor => "qazlll456 with Grok (xAI)";
        public override string ModuleDescription => "Broadcasts Buddhist teachings in CS2 with customizable modes and multi-language support.";

        public bool IsRunning { get; private set; }
        public Dictionary<ulong, bool> PlayerSubscriptions { get; } = new();
        public Dictionary<ulong, string> PlayerLanguagePreferences { get; } = new();
        private CommandManager? _commandManager;
        private ConfigManager? _configManager;
        private LanguageManager? _languageManager;
        private BroadcastManager? _broadcastManager;
        private ChatFormatter? _chatFormatter;

        public override void Load(bool hotReload)
        {
            try
            {
                _chatFormatter = new ChatFormatter();
                _configManager = new ConfigManager(ModuleDirectory);
                _languageManager = new LanguageManager(ModuleDirectory);
                _broadcastManager = new BroadcastManager(this, _chatFormatter);
                _commandManager = new CommandManager(this, _configManager, _languageManager, _broadcastManager);

                bool configLoaded = _configManager.LoadConfig();
                bool languageLoaded = _languageManager.LoadLanguage(_configManager.Config?.LanguageFile ?? "english.json");

                if (configLoaded && languageLoaded && _configManager.Config != null && _languageManager.Language != null)
                {
                    _commandManager.RegisterCommands();
                    _broadcastManager.StartBroadcastTimer(_configManager.Config, _languageManager.Language);
                    IsRunning = true;
                    Server.PrintToConsole("100Buddhist plugin loaded successfully.");
                }
                else
                {
                    Server.PrintToConsole("Error: Failed to load config or language files. Plugin stopped.");
                }
            }
            catch (Exception ex)
            {
                Server.PrintToConsole($"Error: Failed to load 100Buddhist plugin ({ex.Message}).");
            }
        }

        public override void Unload(bool hotReload)
        {
            try
            {
                _commandManager?.UnregisterCommands();
                _broadcastManager?.StopBroadcastTimer();
                IsRunning = false;
                Server.PrintToConsole("100Buddhist plugin unloaded successfully.");
            }
            catch (Exception ex)
            {
                Server.PrintToConsole($"Error: Failed to unload 100Buddhist plugin ({ex.Message}).");
            }
        }

        public LanguageManager GetLanguageManager()
        {
            return _languageManager ?? throw new InvalidOperationException("LanguageManager is not initialized.");
        }
    }
}