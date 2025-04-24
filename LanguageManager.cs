using CounterStrikeSharp.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BuddhistPlugin
{
    public class LanguageManager
    {
        private readonly string _moduleDirectory;
        private readonly Dictionary<string, Language> _languageCache = new();
        public Language? Language { get; private set; }

        public LanguageManager(string moduleDirectory)
        {
            _moduleDirectory = moduleDirectory ?? throw new ArgumentNullException(nameof(moduleDirectory));
        }

        public bool LoadLanguage(string languageFile)
        {
            if (string.IsNullOrEmpty(languageFile))
            {
                Server.PrintToConsole("Error: Language file name is empty. Falling back to english.json.");
                languageFile = "english.json";
            }

            if (_languageCache.TryGetValue(languageFile, out var cachedLanguage))
            {
                Language = cachedLanguage;
                Server.PrintToConsole($"Loaded cached language: {languageFile}");
                return true;
            }

            var languagePath = Path.Combine(_moduleDirectory, "language", languageFile);
            if (!File.Exists(languagePath))
            {
                if (languageFile != "english.json")
                {
                    Server.PrintToConsole($"Error: Language file {languageFile} not found, falling back to english.json.");
                    return LoadLanguage("english.json");
                }
                GenerateDefaultLanguages();
            }

            try
            {
                var json = File.ReadAllText(languagePath);
                var language = JsonSerializer.Deserialize<Language>(json);
                if (language == null || string.IsNullOrEmpty(language.MessagePrefix) || language.Messages == null ||
                    language.Messages.Length == 0 || language.Messages.Any(m => m == null || m.Id < 1 || string.IsNullOrEmpty(m.Text)) ||
                    language.Messages.GroupBy(m => m.Id).Any(g => g.Count() > 1))
                {
                    Server.PrintToConsole($"Error: Invalid language file {languageFile}. Plugin stopped.");
                    return false;
                }

                Language = language;
                _languageCache[languageFile] = language;
                Server.PrintToConsole($"Loaded language: {languageFile}, {language.Messages.Length} messages.");
                return true;
            }
            catch (Exception ex)
            {
                Server.PrintToConsole($"Error: Invalid JSON in {languageFile} ({ex.Message}). Plugin stopped.");
                return false;
            }
        }

        public Language? GetPlayerLanguage(string languageFile)
        {
            if (_languageCache.TryGetValue(languageFile, out var cachedLanguage))
            {
                return cachedLanguage;
            }
            return LoadLanguage(languageFile) ? _languageCache[languageFile] : null;
        }

        private void GenerateDefaultLanguages()
        {
            var languageDir = Path.Combine(_moduleDirectory, "language");
            Directory.CreateDirectory(languageDir);

            var english = new Language
            {
                MessagePrefix = "{green}100Buddhist {0}: {white}{1}",
                Messages = new Message[100]
            };

            var tChinese = new Language
            {
                MessagePrefix = "{blue}100Buddhist {0}: {white}{1}",
                Messages = new Message[100]
            };

            for (int i = 0; i < 100; i++)
            {
                int id = i + 1;
                string color = GetColorTag(id);
                english.Messages[i] = new Message
                {
                    Id = id,
                    Text = $"{color}{GetEnglishMessage(id)}"
                };
                tChinese.Messages[i] = new Message
                {
                    Id = id,
                    Text = $"{color}{GetChineseMessage(id)}"
                };
            }

            File.WriteAllText(Path.Combine(languageDir, "english.json"), JsonSerializer.Serialize(english, new JsonSerializerOptions { WriteIndented = true }));
            File.WriteAllText(Path.Combine(languageDir, "t-chinese.json"), JsonSerializer.Serialize(tChinese, new JsonSerializerOptions { WriteIndented = true }));
            Server.PrintToConsole("Generated default english.json and t-chinese.json with 100 messages each.");
            LoadLanguage("english.json");
        }

        private string GetColorTag(int id)
        {
            string[] colors = { "{white}", "{green}", "{yellow}", "{blue}", "{red}", "{cyan}", "{purple}" };
            return colors[id % colors.Length];
        }

        private string GetEnglishMessage(int id)
        {
            var messages = new Dictionary<int, string>
            {
                { 1, "All that we are is the result of what we have thought. - Buddha, Dhammapada 1" },
                { 2, "Peace comes from within. Do not seek it without. - Buddha" },
                { 3, "Hatred does not cease by hatred, but only by love. - Buddha, Dhammapada 5" },
                { 4, "The mind is everything. What you think, you become. - Buddha" },
                { 5, "To understand everything is to forgive everything. - Buddha" },
                { 6, "Let go of anger; abandon pride. - Buddha, Dhammapada 221" },
                { 7, "Better than a thousand hollow words is one word that brings peace. - Buddha, Dhammapada 100" },
                { 8, "Do not dwell in the past, do not dream of the future, concentrate the mind on the present moment. - Buddha" },
                { 9, "Three things cannot be long hidden: the sun, the moon, and the truth. - Buddha" },
                { 10, "You yourself must strive. The Buddhas only point the way. - Buddha, Dhammapada 276" },
                { 11, "Health is the greatest gift, contentment the greatest wealth, faithfulness the best relationship. - Buddha" },
                { 12, "An insincere and evil friend is more to be feared than a wild beast. - Buddha" },
                { 13, "There is no path to happiness: happiness is the path. - Buddha" },
                { 14, "No one saves us but ourselves. No one can and no one may. - Buddha, Dhammapada 165" },
                { 15, "A disciplined mind brings happiness. - Buddha, Dhammapada 35" },
                { 16, "Meditation brings wisdom; lack of meditation leaves ignorance. - Buddha" },
                { 17, "The root of suffering is attachment. - Buddha" },
                { 18, "In the sky, there is no distinction of east and west; people create distinctions out of their own minds. - Buddha" },
                { 19, "The way is not in the sky; the way is in the heart. - Buddha" },
                { 20, "We are shaped by our thoughts; we become what we think. - Buddha, Dhammapada 1" },
                { 21, "Just as a candle cannot burn without fire, men cannot live without a spiritual life. - Buddha" },
                { 22, "The wise ones fashioned speech with their thought, sifting it as grain is sifted through a sieve. - Buddha" },
                { 23, "To conquer oneself is a greater victory than to conquer thousands in a battle. - Buddha, Dhammapada 103" },
                { 24, "A jug fills drop by drop. - Buddha" },
                { 25, "Radiate boundless love towards the entire world. - Buddha" },
                { 26, "It is better to travel well than to arrive. - Buddha" },
                { 27, "Holding on to anger is like grasping a hot coal with the intent of throwing it at someone else. - Buddha" },
                { 28, "Every morning we are born again. What we do today is what matters most. - Buddha" },
                { 29, "There is nothing more dreadful than the habit of doubt. - Buddha" },
                { 30, "Even death is not to be feared by one who has lived wisely. - Buddha" },
                { 31, "The only real failure in life is not to be true to the best one knows. - Buddha" },
                { 32, "To keep the body in good health is a duty, otherwise we shall not be able to keep our mind strong and clear. - Buddha" },
                { 33, "The tongue like a sharp knife kills without drawing blood. - Buddha" },
                { 34, "Do not overrate what you have received, nor envy others. - Buddha, Dhammapada 6" },
                { 35, "Work out your own salvation. Do not depend on others. - Buddha, Dhammapada 160" },
                { 36, "A man is not called wise because he talks and talks again; but if he is peaceful, loving and fearless then he is in truth called wise. - Buddha" },
                { 37, "Few among men are those who cross to the farther shore. - Buddha, Dhammapada 85" },
                { 38, "There are only two mistakes one can make along the road to truth: not going all the way, and not starting. - Buddha" },
                { 39, "If you light a lamp for someone else, it will also brighten your path. - Buddha" },
                { 40, "Your purpose in life is to find your purpose and give your whole heart and soul to it. - Buddha" },
                { 41, "An idea that is developed and put into action is more important than an idea that exists only as an idea. - Buddha" },
                { 42, "Chaos is inherent in all compounded things. Strive on with diligence. - Buddha" },
                { 43, "Those who are free of resentful thoughts surely find peace. - Buddha" },
                { 44, "The greatest prayer is patience. - Buddha" },
                { 45, "What we think, we become. - Buddha, Dhammapada 2" },
                { 46, "Even as a solid rock is unshaken by the wind, so are the wise unshaken by praise or blame. - Buddha, Dhammapada 81" },
                { 47, "If you do not change direction, you may end up where you are heading. - Buddha" },
                { 48, "The trouble is, you think you have time. - Buddha" },
                { 49, "To be idle is a short road to death and to be diligent is a way of life. - Buddha" },
                { 50, "Give, even if you only have a little. - Buddha" },
                { 51, "You will not be punished for your anger; you will be punished by your anger. - Buddha" },
                { 52, "Patience is key to overcoming all obstacles. - Buddha" },
                { 53, "The fool who knows his folly is wise at least to that extent. - Buddha, Dhammapada 63" },
                { 54, "When you realize how perfect everything is, you will tilt your head back and laugh at the sky. - Buddha" },
                { 55, "Nothing can harm you as much as your own thoughts unguarded. - Buddha" },
                { 56, "Better than a hundred years of ignorance is one day of knowing. - Buddha" },
                { 57, "Speak only endearing speech, speech that is welcomed. - Buddha" },
                { 58, "The past is already gone, the future is not yet here. There’s only one moment for you to live. - Buddha" },
                { 59, "Drop by drop is the water pot filled. Likewise, the wise man gathers goodness. - Buddha, Dhammapada 121" },
                { 60, "One who acts on truth is happy in this world and beyond. - Buddha, Dhammapada 168" },
                { 61, "Live every act fully, as if it were your last. - Buddha" },
                { 62, "The greatest gift is to give people your enlightenment, to share it. - Buddha" },
                { 63, "If you find no one to support you on the spiritual path, walk alone. - Buddha" },
                { 64, "A mind unruffled by the vagaries of fortune is a wonderful thing. - Buddha, Dhammapada 83" },
                { 65, "Your work is to discover your world and then with all your heart give yourself to it. - Buddha" },
                { 66, "He who loves 50 people has 50 woes; he who loves no one has no woes. - Buddha" },
                { 67, "The fragrance of flowers spreads only in the direction of the wind, but the goodness of a person spreads in all directions. - Buddha" },
                { 68, "To live a pure unselfish life, one must count nothing as one’s own in the midst of abundance. - Buddha" },
                { 69, "If we could see the miracle of a single flower clearly, our whole life would change. - Buddha" },
                { 70, "Do not look for a sanctuary in anyone except yourself. - Buddha" },
                { 71, "The wise man makes an island of himself that no flood can overwhelm. - Buddha, Dhammapada 25" },
                { 72, "What you are is what you have been. What you’ll be is what you do now. - Buddha" },
                { 73, "Train your mind to see the good in every situation. - Buddha" },
                { 74, "A fool is happy until his mischief turns against him. - Buddha, Dhammapada 69" },
                { 75, "The one who has conquered himself is a far greater hero than he who has defeated a thousand times a thousand men. - Buddha, Dhammapada 103" },
                { 76, "Silence the angry man with love. Silence the ill-natured man with kindness. - Buddha, Dhammapada 223" },
                { 77, "Happiness never decreases by being shared. - Buddha" },
                { 78, "It is a man’s own mind, not his enemy or foe, that lures him to evil ways. - Buddha, Dhammapada 42" },
                { 79, "Just as treasures are uncovered from the earth, so virtue appears from good deeds. - Buddha" },
                { 80, "The wise are controlled in deed, controlled in speech, controlled in thought. - Buddha, Dhammapada 234" },
                { 81, "Be vigilant; guard your mind against negative thoughts. - Buddha" },
                { 82, "If you truly loved yourself, you could never hurt another. - Buddha" },
                { 83, "Ambition is like love, impatient both of delays and rivals. - Buddha" },
                { 84, "One should strive to understand what lies beyond mere words. - Buddha" },
                { 85, "Purity and impurity depend on oneself; no one can purify another. - Buddha, Dhammapada 165" },
                { 86, "As a water bead on a lotus leaf does not adhere, so the wise man does not cling to the seen or the heard. - Buddha" },
                { 87, "In separateness lies the world’s great misery; in compassion lies the world’s true strength. - Buddha" },
                { 88, "A good friend who points out mistakes and imperfections is to be respected. - Buddha" },
                { 89, "Endurance is one of the most difficult disciplines, but it is to the one who endures that the final victory comes. - Buddha" },
                { 90, "There is no fire like passion, no shark like hatred, no snare like folly, no torrent like greed. - Buddha, Dhammapada 251" },
                { 91, "One moment can change a day, one day can change a life, and one life can change the world. - Buddha" },
                { 92, "All experiences are preceded by mind, having mind as their master, created by mind. - Buddha, Dhammapada 1" },
                { 93, "Do not speak harshly to anyone; those who are spoken to will answer you in the same way. - Buddha, Dhammapada 133" },
                { 94, "The wise man does not judge others; he seeks to understand them. - Buddha" },
                { 95, "True love is born from understanding. - Buddha" },
                { 96, "If you are facing in the right direction, all you need to do is keep on walking. - Buddha" },
                { 97, "The secret of health for both mind and body is not to mourn for the past, nor to worry about the future. - Buddha" },
                { 98, "He who envies others does not obtain peace of mind. - Buddha" },
                { 99, "One who conquers himself is greater than another who conquers a thousand times a thousand men. - Buddha" },
                { 100, "Happiness comes when your work and words are of benefit to yourself and others. - Buddha" }
            };
            return messages[id];
        }

        private string GetChineseMessage(int id)
        {
            var messages = new Dictionary<int, string>
            {
                { 1, "我們的一切皆由我們的思想所成。 - 佛陀，法句經 1" },
                { 2, "內心平靜，勿向外求。 - 佛陀" },
                { 3, "仇恨無法止於仇恨，唯愛能止。 - 佛陀，法句經 5" },
                { 4, "心是一切。你想什麼，你就成為什麼。 - 佛陀" },
                { 5, "理解一切即是寬恕一切。 - 佛陀" },
                { 6, "放下憤怒，捨棄傲慢。 - 佛陀，法句經 221" },
                { 7, "一句帶來平靜之言，勝過千言空話。 - 佛陀，法句經 100" },
                { 8, "勿停留於過去，勿夢想未來，專注當下之念。 - 佛陀" },
                { 9, "三物無法久藏：太陽、月亮與真相。 - 佛陀" },
                { 10, "你必須自行努力，諸佛僅指路。 - 佛陀，法句經 276" },
                { 11, "健康是最大之禮，知足是最大之財，忠誠是最佳之關係。 - 佛陀" },
                { 12, "不誠之友比野獸更可怕。 - 佛陀" },
                { 13, "幸福無路可尋，幸福即是路。 - 佛陀" },
                { 14, "唯有自救，無人能救，無人可救。 - 佛陀，法句經 165" },
                { 15, "調伏之心帶來幸福。 - 佛陀，法句經 35" },
                { 16, "冥想帶來智慧，無冥想則無知。 - 佛陀" },
                { 17, "苦之根源在於執著。 - 佛陀" },
                { 18, "天無東西方之分，人心自創分別。 - 佛陀" },
                { 19, "道不在天上，道在心中。 - 佛陀" },
                { 20, "思想塑造我們，我們成為所想。 - 佛陀，法句經 1" },
                { 21, "如燭無火不燃，人無靈性不生。 - 佛陀" },
                { 22, "智者以思想創言，如篩穀般精選。 - 佛陀" },
                { 23, "征服自己，勝過戰場上敗千人。 - 佛陀，法句經 103" },
                { 24, "水罐滴滴成滿。 - 佛陀" },
                { 25, "向全世界散發無限之愛。 - 佛陀" },
                { 26, "旅途之善重於抵達。 - 佛陀" },
                { 27, "執怒如持熱炭欲擲人，終自傷。 - 佛陀" },
                { 28, "每日清晨重生，今日之行最重要。 - 佛陀" },
                { 29, "疑心之習最為可怕。 - 佛陀" },
                { 30, "智者無懼於死。 - 佛陀" },
                { 31, "人生唯一真失敗，是不忠於己知之善。 - 佛陀" },
                { 32, "保持身體健康是責任，否則心難堅清。 - 佛陀" },
                { 33, "舌如利刃，殺人不見血。 - 佛陀" },
                { 34, "勿高估所得，勿妒他人。 - 佛陀，法句經 6" },
                { 35, "自求解脫，勿依他人。 - 佛陀，法句經 160" },
                { 36, "非多言者為智，平和、慈愛、無懼方為真智。 - 佛陀" },
                { 37, "世人少有達彼岸者。 - 佛陀，法句經 85" },
                { 38, "真理之路唯二誤：不走到底，未曾起步。 - 佛陀" },
                { 39, "為他人點燈，亦照亮己路。 - 佛陀" },
                { 40, "人生之目的在於尋目的，全心奉獻。 - 佛陀" },
                { 41, "實踐之念重於空想。 - 佛陀" },
                { 42, "一切有為法皆含混亂，勤勉修行。 - 佛陀" },
                { 43, "無怨念者必得平靜。 - 佛陀" },
                { 44, "最大之祈禱是耐心。 - 佛陀" },
                { 45, "所想即所成。 - 佛陀，法句經 2" },
                { 46, "如巨石不為風動，智者不為譽毀動。 - 佛陀，法句經 81" },
                { 47, "不改方向，或達現行之處。 - 佛陀" },
                { 48, "你以為時日尚多，實則不然。 - 佛陀" },
                { 49, "懶惰通往死亡，勤奮則生之道。 - 佛陀" },
                { 50, "雖少亦施。 - 佛陀" },
                { 51, "怒不罰你，你自受怒罰。 - 佛陀" },
                { 52, "耐心是克服障礙之鑰。 - 佛陀" },
                { 53, "知愚之愚，於此為智。 - 佛陀，法句經 63" },
                { 54, "若見萬物之完美，你將仰天大笑。 - 佛陀" },
                { 55, "無守之心最傷己。 - 佛陀" },
                { 56, "一日之知勝百年無明。 - 佛陀" },
                { 57, "唯說受歡迎之言，溫和之語。 - 佛陀" },
                { 58, "過去已逝，未來未至，唯當下可活。 - 佛陀" },
                { 59, "水罐滴滴滿，智者積善亦然。 - 佛陀，法句經 121" },
                { 60, "依真理而行，於今世後世皆樂。 - 佛陀，法句經 168" },
                { 61, "每行當全心，如最後之行。 - 佛陀" },
                { 62, "最大之禮是分享覺悟。 - 佛陀" },
                { 63, "靈道無伴，獨行可也。 - 佛陀" },
                { 64, "心不為命運波動所擾，最妙。 - 佛陀，法句經 83" },
                { 65, "尋己之世界，全心奉獻之。 - 佛陀" },
                { 66, "愛五十人有五十苦，無愛則無苦。 - 佛陀" },
                { 67, "花香隨風，善人之德四方散。 - 佛陀" },
                { 68, "純淨無私之生，不以豐中之物為己有。 - 佛陀" },
                { 69, "若能清晰見一花之奇，整個人生將變。 - 佛陀" },
                { 70, "勿於他人中尋庇護，唯己可依。 - 佛陀" },
                { 71, "智者自成島，洪水不沒。 - 佛陀，法句經 25" },
                { 72, "今之你由過去成，未來之你由今之行定。 - 佛陀" },
                { 73, "訓練心於每境見善。 - 佛陀" },
                { 74, "愚者樂於惡，直至惡返己。 - 佛陀，法句經 69" },
                { 75, "自勝者大於戰勝千人之英雄。 - 佛陀，法句經 103" },
                { 76, "以愛止怒人，以善止惡人。 - 佛陀，法句經 223" },
                { 77, "幸福因分享不減。 - 佛陀" },
                { 78, "非敵非仇，乃己心引人向惡。 - 佛陀，法句經 42" },
                { 79, "如地中掘寶，善行顯德。 - 佛陀" },
                { 80, "智者於行、言、思皆受控。 - 佛陀，法句經 234" },
                { 81, "警醒，守心防負念。 - 佛陀" },
                { 82, "若真愛己，絕不傷他。 - 佛陀" },
                { 83, "野心如愛，不耐延遲與對手。 - 佛陀" },
                { 84, "當求超越言語之真義。 - 佛陀" },
                { 85, "淨與不淨自定，無人可淨他人。 - 佛陀，法句經 165" },
                { 86, "如水珠於蓮葉不附，智者不執所見所聞。 - 佛陀" },
                { 87, "分離乃世人之大苦，慈悲乃世人之真力。 - 佛陀" },
                { 88, "指出過失之友當敬。 - 佛陀" },
                { 89, "忍最難修，忍者終勝。 - 佛陀" },
                { 90, "無欲火如情，無鯊如恨，無網如愚，無洪如貪。 - 佛陀，法句經 251" },
                { 91, "一瞬可改一日，一日可改一生，一生可改世界。 - 佛陀" },
                { 92, "一切皆由心生，心為主，心所創。 - 佛陀，法句經 1" },
                { 93, "勿苛言於人，彼將以同語回之。 - 佛陀，法句經 133" },
                { 94, "智者不評他人，唯求理解之。 - 佛陀" },
                { 95, "真愛由理解生。 - 佛陀" },
                { 96, "方向若正，唯需前行。 - 佛陀" },
                { 97, "身心之健康秘訣，不悲過去，不憂未來。 - 佛陀" },
                { 98, "嫉他人者不得心安。 - 佛陀" },
                { 99, "自勝者大於戰勝千人之英雄。 - 佛陀" },
                { 100, "幸福來自言行利己利他。 - 佛陀" }
            };
            return messages[id];
        }
    }

    public class Language
    {
        public string MessagePrefix { get; set; } = string.Empty;
        public Message[] Messages { get; set; } = Array.Empty<Message>();
    }

    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}