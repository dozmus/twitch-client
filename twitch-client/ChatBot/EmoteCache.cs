using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace TwitchClient.ChatBot
{
    class EmoteCache
    {
        // TODO capture all emotes from http://twitchemotes.com/api_cache/v2/global.json
        private static readonly string EmotesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Chat Emotes");
        public static Dictionary<String, Image> EmotesCache = new Dictionary<String, Image>();

        public static void LoadEmotes()
        {
            // Checking if the emotes directory exists
            if (!Directory.Exists(EmotesDirectory))
            {
                // TODO spit error
                return;
            }

            // Loading all emotes
            string pathSeparator = Path.DirectorySeparatorChar.ToString();

            foreach (string file in Directory.GetFiles(EmotesDirectory, "*.png")) // TODO handle other extensions
            {
                // Skipping invalid files
                if (file.Length == 0 || !file.Contains(pathSeparator) || !file.Contains("."))
                {
                    continue;
                }

                // Parsing emote name
                int fileNameIndex = file.LastIndexOf(pathSeparator);
                string emoteName = file.Substring(fileNameIndex, file.LastIndexOf('.') - fileNameIndex);

                // Loading emote - there is no way a duplicate entry could occur due to the OS
                EmotesCache.Add(emoteName, Image.FromFile(file));
            }

#if DEBUG
            Debug.WriteLine("Loaded " + EmotesCache.Count + " chat emotes.");
#endif
        }
    }
}
