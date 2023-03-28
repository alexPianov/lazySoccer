using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using LazySoccer.SceneLoading.PlayerData.Enum;
using Scripts.Infrastructure.Managers;
using Scripts.Infrastructure.ScriptableStore;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LazySoccer.Network.Get.GeneralClassGETRequest;

namespace LazySoccer.Table
{
    public class SlotPlayer : MonoBehaviour
    {
        [Title("Main")] 
        [SerializeField] private Image playerAvatar;
        [SerializeField] private TMP_Text playerNumber;
        [SerializeField] private TMP_Text playerName;
        [SerializeField] private TMP_Text playerAge;
        [SerializeField] private TMP_Text playerStatus;
        [SerializeField] private TMP_Text playerSalary;
        [SerializeField] private TMP_Text playerPower;
        [SerializeField] private Image playerStatusImage;
        [SerializeField] private Slider playerStatusSlider;

        [TitleGroup("Conditions")]
        [SerializeField] private TMP_Text playerGoals;
        [SerializeField] private TMP_Text playerPosition;
        [SerializeField] private TMP_Text playerForm;
        [SerializeField] private TMP_Text playerPositionShort;
        [SerializeField] private TMP_Text playerTrauma;
        [SerializeField] private Color[] traumaTextColor;
        
        [Title("Form")]
        [SerializeField] private Image playerFormImage;
        [SerializeField] private SlotPlayerColor slotColor;
        
        [Title("Trait")]
        [SerializeField] private Transform traitContainer;
        [SerializeField] private GameObject traitPrefab;

        [Title("Trauma")] 
        [SerializeField] private Image playerTraumaImage;
        [SerializeField] private SlotPlayerMed playerTraumaSlider;
        
        [Title("Training")]
        [SerializeField] private SlotPlayerTraining playerTrainingSlider;
        
        [Title("Charge")] 
        [SerializeField] private SlotPlayerForm playerFormSlider;

        [HideInInspector] public int numberInList;

        private TeamPlayer _teamPlayer;
        public TeamPlayer GetTeamPlayer()
        {
            return _teamPlayer;
        }

        public void SetInfo(TeamPlayer teamPlayer, int number = 0, Sprite avatar = null)
        {
            _teamPlayer = teamPlayer;
            numberInList = number;

            if (avatar) playerAvatar.sprite = avatar;
            if (playerNumber) playerNumber.text = $"{number}";
            if (playerName) playerName.text = teamPlayer.name;
            
            if (playerAge)
            {
                if (playerAge.GetComponent<LocalizationParamsManager>())
                {
                    playerAge.GetComponent<LocalizationParamsManager>()
                        .SetParameterValue("param", teamPlayer.age.ToString());
                }
                else
                {
                    playerAge.text = teamPlayer.age.ToString();
                }
            }
            
            if (playerStatus) SetStatus(teamPlayer);
            if (playerStatusImage) SetStatusImage(teamPlayer);
            if (playerGoals) playerGoals.text = teamPlayer.goalCount.ToString();
            if (playerForm) playerForm.text = $"{teamPlayer.form}%";
            if (playerFormImage) playerFormImage.sprite = GetFormImage(teamPlayer.form);
            if (playerTraumaImage) SetTraumaImage(teamPlayer.status); 
            if (slotColor) slotColor.SetTeamPlayer(teamPlayer);
            if (playerPosition) playerPosition.text = GetPositionAbbreviation(teamPlayer);
            if (playerPositionShort) playerPositionShort.text = GetPosition(teamPlayer);
            if (playerSalary) playerSalary.text = $"{teamPlayer.dailySalary} LAZY";
            if (playerPower) playerPower.text = teamPlayer.power.ToString();
            
            if (teamPlayer.injuries != null && teamPlayer.injuries.Count != 0)
            {
                SetInjures(teamPlayer);
            }
            else
            {
                SetTraining();
            }

            if (traitContainer) CreateTraitIcons(teamPlayer);
            
            if (playerStatusSlider) SetStatusSlider(teamPlayer);
            if (playerTrainingSlider)playerTrainingSlider.SetValue(teamPlayer);
            //if (playerFormSlider) playerFormSlider.SetValue(teamPlayer.form);
            if (playerFormSlider) playerFormSlider.SetValue(teamPlayer);
        }

        private void SetInjures(TeamPlayer teamPlayer)
        {
            var mainInjury = GetMainInjury(teamPlayer.injuries);

            if (mainInjury == null)
            {
                SetTraining();
                return;
            }

            if (playerTrauma)
            {
                playerTrauma.GetComponent<Localize>().SetTerm("TeamPlayer-Injury-" + mainInjury.trauma);
                //playerTrauma.text = PlayerStatus.GetTraumaAsText(mainInjures.trauma);
                playerTrauma.color = traumaTextColor[0];

                if (slotColor)
                {
                    //slotColor.SetColorScheme(0);
                    slotColor.SetTrauma(true);
                }
            }
            else
            {
                playerTrauma.enabled = false;
            }

            var healPeriod = GetHealPeriod(teamPlayer.injuries);

            if (playerTraumaSlider) playerTraumaSlider.SetTrauma(healPeriod, _teamPlayer);
        }

        private void SetTraining()
        {
            if (playerTrauma)
            {
                //playerTrauma.text = "Healthy"; // set healty loc from status
                playerTrauma.GetComponent<Localize>().SetTerm("TeamPlayer-Status-Healthy");
                playerTrauma.color = traumaTextColor[1];
            }
                
            if (playerTraumaSlider) playerTraumaSlider.SetTraining(_teamPlayer);
            //if (playerTraumaSlider) playerTraumaSlider.SetTrainingDay();
        }

        public void SetStatus(TeamPlayer player)
        {
            if (playerStatus.GetComponent<Localize>())
            {
                playerStatus.GetComponent<Localize>().SetTerm("TeamPlayer-Status-" + player.status);
            }
            else
            {
                playerStatus.text = PlayerStatus.GetStatusAsText(player.status);
            }
        }

        private void SetStatusImage(TeamPlayer player)
        {
            var sprite = GetStatusSprites().Get(player.status);
            
            if (sprite) playerStatusImage.sprite = sprite;
        }
        
        private void SetStatusSlider(TeamPlayer teamPlayer)
        {
            Debug.Log("SetStatusSlider");
            ActiveStatusSlider(true);
            
            if (teamPlayer.status == TeamPlayerStatus.Healing)
            {
                playerStatusSlider.maxValue = 100;
                playerStatusSlider.value = 20;
                
                // var injury = GetMainInjuries(teamPlayer.injuries);
                //
                // var timeRemain =  DateTime.Now.ToUniversalTime() - injury.startDate;
                // var daysPast = timeRemain.Value.Days;
                //
                // playerStatusSlider.maxValue = injury.healPeriod;
                // playerStatusSlider.value = daysPast;
                return;
            }
            
            if (teamPlayer.status == TeamPlayerStatus.OnExamination)
            {
                playerStatusSlider.maxValue = 100;
                playerStatusSlider.value = 20;
                return;
            }
            
            if (teamPlayer.status == TeamPlayerStatus.RestoringForm)
            {
                playerStatusSlider.maxValue = 104;
                playerStatusSlider.value = teamPlayer.form;
                return;
            }
            
            if (teamPlayer.status == TeamPlayerStatus.Training)
            {
                playerStatusSlider.maxValue = 100;
                playerStatusSlider.value = 20;
                return;
            }

            ActiveStatusSlider(false);
        }

        private void ActiveStatusSlider(bool state)
        {
            if(playerStatusSlider) playerStatusSlider.gameObject.SetActive(state);
        }

        private string GetPositionAbbreviation(TeamPlayer player)
        {
            var fieldPosition = player.position.position.First().ToString();

            if (fieldPosition == "G")
            {
                fieldPosition = "GK";
            }
            
            if (player.position.fieldLocation == null)
            {
                return fieldPosition;
            }
            
            var fieldLocation = player.position.fieldLocation.First().ToString();
            return fieldLocation + fieldPosition;
        }
        
        private string GetPosition(TeamPlayer player)
        {
            return player.position.position;
        }

        private Injury GetMainInjury(List<Injury> injuries)
        {
            if (injuries == null || injuries.Count == 0) return null;
            
            var injuriesOrder = injuries
                .OrderByDescending(injuries => injuries.playerTraumaId).ToList();

            return injuriesOrder[0];
        }

        private int GetHealPeriod(List<Injury> injuries)
        {
            if (injuries == null || injuries.Count == 0) return 0;

            int period = 0;

            foreach (var injury in injuries)
            {
                period += injury.healPeriod;
            }

            return period;
        }
        
        private void CreateTraitIcons(TeamPlayer player)
        {
            if (player.traits == null || player.traits.Count == 0)
            {
                Debug.Log("Failed to find traits for: " + player.name);
                return;
            }

            for (var i = 0; i < traitContainer.childCount; i++)
            {
                Destroy(traitContainer.GetChild(i).gameObject);
            }

            int traitIconCount = 0;
            int maxTraitIconCount = 4;
            
            foreach (var trait in player.traits)
            {
                traitIconCount++;
                
                if(traitIconCount > maxTraitIconCount) return;
                
                var traitIcon = Instantiate(traitPrefab, traitContainer);
                
                if (traitIcon.TryGetComponent(out SlotPlayerTrait slotTrait))
                {
                    var result = slotTrait.SetTraitInfo(trait.name);
                    if (result == null) { Destroy(traitIcon); }
                }
            }
        }

        private Sprite GetFormImage(int formValue)
        {
            var sprites = GetStatusSprites();
            
            if (formValue < 50) return sprites.Get(TeamPlayerStatus.Exhausted); 

            return sprites.Get(TeamPlayerStatus.Charged); 
        }
        
        private void SetTraumaImage(TeamPlayerStatus playerStatus)
        {
            var sprites = GetStatusSprites();

            if (playerStatus == TeamPlayerStatus.NotHealing)
            {
                playerTraumaImage.enabled = true;
                playerTraumaImage.sprite = sprites.Get(TeamPlayerStatus.None);
                return;
            }

            playerTraumaImage.enabled = false;
        }

        private StatusSprites GetStatusSprites()
        {
            return ServiceLocator.GetService<ManagerSprites>().GetStatusSprites();
        }
        
        public void Active(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
