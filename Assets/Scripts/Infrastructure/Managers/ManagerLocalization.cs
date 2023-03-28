using I2.Loc;
using UnityEngine;

namespace LazySoccer.SceneLoading.Infrastructure
{
    public class ManagerLocalization : MonoBehaviour
    {
        public Language currentLanguage { get; private set; }
        
        public enum Language
        {
            English, Russian
        }

        public void SetLanguage(Language language)
        {
            currentLanguage = language;
            
            if(LocalizationManager.HasLanguage(currentLanguage.ToString()))
            { 
                LocalizationManager.CurrentLanguage = currentLanguage.ToString();
                Debug.Log("SetLanguage: " + language);
            }

            ServiceLocator.GetService<ManagerPlayerData>()
                .PlayerData.Language = currentLanguage;
        }
    }
}