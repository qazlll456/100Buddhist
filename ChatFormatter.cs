using CounterStrikeSharp.API;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BuddhistPlugin
{
    public class ChatFormatter
    {
        private readonly Dictionary<string, string> _colorMap = new()
        {
            { "{white}", "\x01" },
            { "{red}", "\x07" },
            { "{blue}", "\x0B" },
            { "{green}", "\x04" },
            { "{yellow}", "\x09" },
            { "{purple}", "\x0E" },
            { "{cyan}", "\x0C" },
            { "{orange}", "\x10" },
            { "{pink}", "\x0F" },
            { "{olive}", "\x05" },
            { "{lime}", "\x06" },
            { "{violet}", "\x0E" },
            { "{lightblue}", "\x0B" }
        };
        private const int MaxTagsPerMessage = 5;

        public string FormatMessage(string? messagePrefix, int messageId, string? messageText)
        {
            Server.PrintToConsole($"Formatting message - Prefix: {messagePrefix ?? "null"}, ID: {messageId}, Text: {messageText ?? "null"}");

            messagePrefix = string.IsNullOrEmpty(messagePrefix)
                ? "{green}100Buddhist {0}: {white}{1}"
                : messagePrefix;
            messageText = messageText ?? "No Text";

            string formatted = messagePrefix.Replace("{0}", messageId.ToString()).Replace("{1}", messageText);

            if (!Regex.IsMatch(formatted, @"\{(white|red|blue|green|yellow|purple|cyan|orange|pink|olive|lime|violet|lightblue)\}"))
            {
                formatted = "{white}" + formatted;
                Server.PrintToConsole("Applied default white color.");
            }

            var tagMatches = Regex.Matches(formatted, @"\{(white|red|blue|green|yellow|purple|cyan|orange|pink|olive|lime|violet|lightblue)\}");
            if (tagMatches.Count > MaxTagsPerMessage)
            {
                Server.PrintToConsole($"Warning: Message contains {tagMatches.Count} color tags, exceeding limit of {MaxTagsPerMessage}. Stripping excess.");
                int tagsToKeep = MaxTagsPerMessage;
                string temp = formatted;
                formatted = "";
                int lastIndex = 0;
                foreach (Match match in tagMatches)
                {
                    if (tagsToKeep > 0)
                    {
                        formatted += temp.Substring(lastIndex, match.Index - lastIndex) + match.Value;
                        lastIndex = match.Index + match.Length;
                        tagsToKeep--;
                    }
                }
                formatted += temp.Substring(lastIndex);
                NotifyAdmins($"Message ID {messageId} exceeded color tag limit ({MaxTagsPerMessage}).");
            }

            foreach (var color in _colorMap)
            {
                formatted = formatted.Replace(color.Key, " " + color.Value);
            }

            var invalidTags = Regex.Matches(formatted, @"\{\w+\}");
            if (invalidTags.Count > 0)
            {
                Server.PrintToConsole($"Warning: Found {invalidTags.Count} unknown tags in message ID {messageId}.");
                foreach (Match tag in invalidTags)
                {
                    formatted = formatted.Replace(tag.Value, "");
                }
                NotifyAdmins($"Message ID {messageId} contains invalid tags.");
            }

            Server.PrintToConsole($"Final formatted message: {formatted}");
            return formatted;
        }

        private void NotifyAdmins(string message)
        {
            foreach (var player in Utilities.GetPlayers())
            {
                if (player != null && !player.IsBot && player.IsValid)
                {
                    player.PrintToChat($"[100Buddhist Admin] {message}");
                }
            }
        }
    }
}