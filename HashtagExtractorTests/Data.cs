using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashtagExtractorTests
{
    internal static class Data
    {
        private static string[] _tweetsWithoutHashtags = new string[]
        {
@"está sendo praticado um crime contra o basquete agora em quadra. Os 2 times",
@"ya me estaba dando cargo de conciencia pero me sigue llamando por mi nombre",
@"@amjuster @betterthanstar2 Thank you! Looks like I'm just inside the submission window for THINK. I'll start there, then do the others if they accept simultaneous submissions.",
@"@StealthBombM13 Bro were all going down this week man. Grab liquid iv. Its been a help for me",
@"RT @cuteasstyy: i will silently Shazam yo shit in the car before i ask you what song this is😂😂😂😂😂😂😂😂😂😂😂😂",
@"コレクレー→サーフゴーって陰キャが急に陽キャになったイメージだったけど徒歩フォルムのコレクレーよく見ると普通に陽キャで草生える",
@"ぉそぜ",
@"RT @vaiiyie: january me would not believe everything that happened this year",
@"びっくりしたTLに推しがいた",
@"@RaZeldris @7DS_en I hope to see Freya in 2023",
@"@GenuineHeart11 Balikin lagi ajah ke jalan ah 😬",
@"@TCTPokemon Autumnnnnnn",
@"@luvsicas I was rlly confused that’s why I posted",
@"@FrankMikeDavis1 Yeah, they should have sent them too sunny Mexico a definite mistake",
@"@AnaDeBsAs3 @JMilei Coincido. Yo no les perdono que hayan avalado el genocidio del aborto.",
@"@ericareport Figures",
@"RT @str8bois1: And they were roommates! 😅🍆💦",
@"@miseya6324 มอนิ่งนะครับ ขอให้เป็นเช้าที่สดใสเป็นวันที่ดีนะครับ ☁️🌷",
        };

        public static IEnumerable<object[]> TweetsWithoutHashtags =>
            MakeObjectArrayFromStrings(_tweetsWithoutHashtags);

        private static IEnumerable<object[]> MakeObjectArrayFromStrings(IEnumerable<string> strings) =>
            strings.Select(x => new object[] { x });
    }
}
