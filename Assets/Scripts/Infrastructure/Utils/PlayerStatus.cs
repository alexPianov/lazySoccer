using System;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.SceneLoading.PlayerData.Enum
{
    public static class PlayerStatus
    {
        public static string GetTraumaAsText(string number)
        {
            switch (number)
            {
                case "Microtrauma": return "Microtrauma"; 
                case "StrainedMuscle": return "Strained Muscle"; 
                case "LimbDislocation": return "Limb Dislocation";
                case "TornLigament": return "Torn Ligament"; 
                case "DamagedJoint": return "Damaged Joint"; 
                case "BrokenBone": return "Broken Bone"; 
                
                default: return "None";
            }
        }

        public static string GetStatusAsText(TeamPlayerStatus number)
        {
            switch (number)
            {
                case TeamPlayerStatus.SentOnMatch: return "Sent On Match"; 
                case TeamPlayerStatus.Healing: return "Healing"; 
                case TeamPlayerStatus.NotHealing: return "Not Healing";
                case TeamPlayerStatus.Exhausted: return "Exhausted"; 
                case TeamPlayerStatus.RestoringForm: return "Restoring Form"; 
                case TeamPlayerStatus.Training: return "Training"; 
                case TeamPlayerStatus.OnExamination: return "On Examination"; 
                case TeamPlayerStatus.OnTransferMarket: return "On Transfer Market"; 
                case TeamPlayerStatus.Charged: return "Charged";
                case TeamPlayerStatus.Healthy: return "Healthy";
                case TeamPlayerStatus.OnTrainingAndMatch: return "On Training And Match";
                
                default: return "None";
            }
        }
        
        public static string GetAgeAsText(int number)
        {
            switch (number)
            {
                case > 14: return "Old"; 
                case > 12: return "Mature";
                default: return "Young";
            }
        }

    }
}