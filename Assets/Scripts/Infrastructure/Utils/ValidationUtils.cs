using System.Collections.Generic;

namespace Scripts.Infrastructure.Utils
{
    public static class ValidationUtils
    {
        public static bool StringNicknameValid(string text) 
            => text != null && text.Length >= 2 && text.Length <= 32;
        public static bool StringEmailValid(string text) 
            => text != null && text.Length >= 2 && text.Length <= 64;
        public static bool StringPasswordValid(string text) 
            => text != null && text.Length >= 2 && text.Length <= 64;
        public static bool StringCodeValid(string text) 
            => text != null && text.Length >= 2 && text.Length <= 16;
        public static bool StringTeamNameValid(string text) 
            => text != null && text.Length >= 2 && text.Length <= 32;
        public static bool StringTeamNameShortValid(string text) 
            => text != null && text.Length == 3;
        
        public static bool StringPinValid(string text) 
            => text != null && text.Length == 6;

        private static List<string> offensiveShortcuts = new List<string> { 
            "ASS", "CUM", "HUY", "HUI", "XEP", "XER", "XYI", "XUI", 
            "FAG", "GAY", "TIT", "FAG", "HER", "HEP" } ;
        
        public static bool OffensiveShortcuts(string text)
        {
            return offensiveShortcuts.Contains(text);
        }
    }
}