using System;
using System.Linq;
using LazySoccer.Network.Error;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;
using static LazySoccer.Network.Get.AdditionClassGetRequest;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Utils
{
    public static class DataUtils
    {
        public static T CreateFromJSON<T>(string jsonString)
        {
            return JsonUtility.FromJson<T>(jsonString);
        }

        public static T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string Serialize<T>(T type)
        {
            return JsonConvert.SerializeObject(type);
        }
        
        public static Color GetColorFromHex(string hexString)
        {
            if (ColorUtility.TryParseHtmlString("#" + hexString, out Color color))
            {
                return color;
            }

            Debug.Log("Failed to create color from hex string: " + hexString);
            
            return Color.clear;
        }

        public static ErrorRequest.ValidationError WebResultValidationError(UnityWebRequest result)
        {
            if(string.IsNullOrEmpty(result.downloadHandler.text)) { Debug.Log("Download handler data is null"); return null; }
            
            var a = Deserialize<ErrorRequest.ValidationErrors>(result.downloadHandler.text);

            if (a == null) { Debug.Log("Result is null"); return null; }

            var validationError = new ErrorRequest.ValidationError();
            validationError.titleEnum = ErrorRequest.ErrorName.None;
            validationError.description = "";
            
            if (a.detailedError != null)
            {
                Debug.LogError("Error message: " + a.detailedError.message 
                                             + " | Stack Trace: " + a.detailedError.stackTrace);
            }
            
            if (a.validationErrors == null || a.validationErrors.Count == 0) 
            { 
                Debug.Log("Validation errors is null");

                if (a.message == null)
                {
                    Debug.Log("Message is null");
                    validationError.description = ServerErrors(result);
                    return validationError;
                }
                
                Debug.Log("Error msg: " + a.message);
                
                validationError.description = a.message;
                return validationError;
            }
            
            if (a.validationErrors == null || a.validationErrors.Count == 0) 
            { 
                Debug.Log("Validation errors is null");
                if (a.message == null) return validationError;
                validationError.description = a.message;
                return validationError;
            }
            
            return a.validationErrors[0];
        }

        private static string ServerErrors(UnityWebRequest result)
        {
            var a = Deserialize<ErrorRequest.ServerError>(result.downloadHandler.text);
            
            if (a.errors == null || a.errors.Code == null || a.errors.Code.Count == 0) 
                return "Unknown error";
            
            return a.errors.Code[0];
        }
        
        public static string GetDivisionDetailsInfo(Division division, int divisionPlace = 0)
        {
            if (division == null)
            {
                Debug.Log("Failed to find division info");
                return "Forestria, D1, 1st";
            }
            
            var region = TeamStatus.GetRegion(division.divisionId);

            if (divisionPlace == 0)
            {
                return string.Format
                    ("{0}, D{1}", region, division.rank);
            }
            
            return string.Format
                ("{0}, D{1}, {2}st", region, division.rank, divisionPlace);
        }
        
        public static string GetDate(DateTime? gameDate)
        {
            return GetDateFull(gameDate);
            
            if (gameDate == null || gameDate.Value == null)
            {
                return "";
            }

            DateTime date = gameDate.Value;

            IFormatProvider culture = new System.Globalization.CultureInfo("fr-FR", true);

            string[] matchTime28Formats = date.GetDateTimeFormats(culture);
            
            Debug.Log("Date: " + gameDate + " | Value " + date + " | Converted: " + matchTime28Formats[3]);
            
            return matchTime28Formats[3];
        }

        public static string GetDateFull(DateTime? gameDate)
        {
            if (gameDate == null || gameDate.Value == null)
            {
                return "";
            }
            
            var parsedTime = DateTime.Parse(gameDate.ToString());
            var localDate = parsedTime.ToLocalTime();
            var yearShort = localDate.Year.ToString().Split('0')[1];
            var day = localDate.Day.ToString();
            var month = localDate.Month.ToString();
            var hour = localDate.Hour.ToString();
            var minutes = localDate.Minute.ToString();
            
            if (day.ToCharArray().Length == 1) day = $"0{day}";
            if (month.ToCharArray().Length == 1) month = $"0{month}";
            if (hour.ToCharArray().Length == 1) hour = $"0{hour}";
            if (minutes.ToCharArray().Length == 1) minutes = $"0{minutes}";
            
            return $"{day}.{month}.{yearShort} {hour}:{minutes}";
        }

        public static string GetLootName(LootName lootName)
        {
            switch (lootName)
            {
                case LootName.StaffIncentivesProgramme:
                    return "Staff Incentives";
                    break;
                case LootName.StaffTrainingProgramme:
                    return "Staff Training";
                    break;
                case LootName.SumOfMoney:
                    return "Money";
                    break;
                case LootName.MiracleMedicine:
                    return "Miracle medicine";
                    break;
                case LootName.IntensiveTrainingProgramme:
                    return "Intensive training";
                    break;
                case LootName.ScoutInsight:
                    return "Scout insight";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lootName), lootName, null);
            }
        }

        public static string GetLootType(LootType lootType)
        {
            switch (lootType)
            {
                case LootType.Little:
                    return "Little";
                    break;
                case LootType.Moderate:
                    return "Moderate";
                case LootType.Quite:
                    return "Quite";
                case LootType.Big:
                    return "Big";
                default:
                    throw new ArgumentOutOfRangeException(nameof(lootType), lootType, null);
            }
        }

        public static string GetLootChance(DropChanceType dropChanceType)
        {
            switch (dropChanceType)
            {
                case DropChanceType.Common:
                    return "Common";
                case DropChanceType.Rare:
                    return "Rare";
                case DropChanceType.Very_Rare:
                    return "Very Rare";
                default:
                    throw new ArgumentOutOfRangeException(nameof(dropChanceType), dropChanceType, null);
            }
        }
        
        public static string GetSkillName(SkillName skillName)
        {
            switch (skillName)
            {
                case SkillName.Defense:
                    return "Defense";
                    break;
                case SkillName.Pass:
                    return "Pass";
                    break;
                case SkillName.Speed:
                    return "Speed";
                    break;
                case SkillName.Technique:
                    return "Technique";
                    break;
                case SkillName.HeadPlay:
                    return "Head Play";
                    break;
                case SkillName.Strike:
                    return "Strike";
                    break;
                case SkillName.Clearance:
                    return "Clearance";
                    break;
                case SkillName.Intuition:
                    return "Intuition";
                    break;
                case SkillName.Reflex:
                    return "Reflex";
                    break;
                case SkillName.Saves:
                    return "Saves";
                    break;
                case SkillName.CrossesAndHighBalls:
                    return "Crosses and high balls";
                    break;
                case SkillName.Positioning:
                    return "Positioning";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(skillName), skillName, null);
            }
        }

        public static string GetSpeciality(Position playerPosition)
        {
            if (playerPosition == null) return null;
            
            var fieldLocation = playerPosition.fieldLocation;
            var position = playerPosition.position;
            var abbreviation = position.First().ToString();

            if (abbreviation == "G")
            {
                abbreviation = "GK";
            }
            
            if (fieldLocation != null)
            {
                abbreviation = fieldLocation.First().ToString() + position.First();
            }

            return string.Format("<b>{0}</b>: {1} {2}", abbreviation, fieldLocation, position);
        }

        public static string GetSpecialityAbbreviation(Position playerPosition)
        {
            var fieldLocation = playerPosition.fieldLocation;
            var position = playerPosition.position;
            var abbreviation = position.First().ToString();

            if (abbreviation == "G")
            {
                abbreviation = "GK";
            }
            
            if (fieldLocation != null)
            {
                abbreviation = fieldLocation.First().ToString() + position.First();
            }

            return abbreviation;
        }
        
        public static Texture Base64ToTexture(string b64_string, int textureSize = 10)
        {
            Debug.Log(b64_string);
            var b64_bytes = Convert.FromBase64String(b64_string);
 
            var tex = new Texture2D(textureSize, textureSize);
            tex.LoadImage(b64_bytes);

            return tex;
        }
        
        public static Texture2D Base64ToTexture2D(string b64_string, int textureSize = 10)
        {
            Debug.Log(b64_string);
            var b64_bytes = Convert.FromBase64String(b64_string);
 
            var tex = new Texture2D(textureSize, textureSize);
            tex.LoadImage(b64_bytes);

            return tex;
        }
    }
}