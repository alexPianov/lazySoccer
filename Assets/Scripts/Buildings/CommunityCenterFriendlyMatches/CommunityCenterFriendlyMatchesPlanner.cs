using System;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Network.Get;
using LazySoccer.Popup;
using LazySoccer.SceneLoading.Buildings.CommunityCenter;
using LazySoccer.Status;
using LazySoccer.Windows;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.CommunityCenterFriendlyMatches
{
    public class CommunityCenterFriendlyMatchesPlanner : MonoBehaviour
    {
        private CommunityCenterTypesOfRequest _communityCenterTypesOfRequest;

        [Title("Input")]
        [SerializeField] private TMP_InputField inputBet;
        [SerializeField] private CalendarController calendarController;

        [Title("Refs")] 
        [SerializeField] private CommunityCenterFriend.CommunityCenterFriend Friend;

        [Title("Button")]
        [SerializeField] private Button buttonSendRequest;
        
        private ManagerPlayerData _managerPlayerData;

        private void Awake()
        {
            _managerPlayerData = ServiceLocator.GetService<ManagerPlayerData>();
            
            _communityCenterTypesOfRequest = ServiceLocator
                .GetService<CommunityCenterTypesOfRequest>();
                
            buttonSendRequest.onClick.AddListener(Ready);
            
            inputBet.onValueChanged.AddListener(SetBet);
            
            calendarController.DayIsSelected.AddListener(DayIsPicked);
        }

        public void UpdateCalendar()
        {
            calendarController.UpdateCalendar();
        }

        private bool _dayIsPicked;
        private void DayIsPicked(bool state)
        {
            _dayIsPicked = state;
            CheckButton();
        }

        private int _bet;
        private void SetBet(string value)
        {
            Debug.Log("Bet value: " + value);
            
            var access = value.Length < 10;
            
            buttonSendRequest.interactable = access;

            if (!access) return;

            if (string.IsNullOrEmpty(value))
            {
                _bet = 0; 
            }
            else
            {
                var parsedValue = int.Parse(value);
                
                var inBalance = _managerPlayerData.PlayerHUDs
                    .Balance.Value > parsedValue;
                
                buttonSendRequest.interactable = inBalance;
                
                if (inBalance)
                {
                    _bet = parsedValue;
                
                    Debug.Log("Current bet: " + _bet);
                }
            }

            CheckButton();
        }

        private void CheckButton()
        {
            buttonSendRequest.interactable = _dayIsPicked && _bet > 1000;
        }

        public async void Ready()
        {
            buttonSendRequest.interactable = false;
            
            var success = await MatchRequest(Friend.CurrentUser);
            
            if (success)
            {
                ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo
                    ($"FRIEND is got you'r game request. See more in Friendly matches", Param1: Friend.CurrentUser.userName);
                
                await Friend.ShowFriendData(Friend.CurrentUser);
                
                buttonSendRequest.interactable = false;
                
                ServiceLocator.GetService<CommunityCenterPopupStatus>()
                    .SetAction(StatusCommunityCenterPopup.None);
            }
            
            buttonSendRequest.interactable = true;
        }
        
        private async UniTask<bool> MatchRequest(GeneralClassGETRequest.User friend)
        {
            Debug.Log("MatchRequest with: " + friend.userId);

            if (_bet == 0)
            {
                var result = await ServiceLocator.GetService<QuestionPopup>().OpenQuestion
                    ("You can place a bet on the game, or continue without it", 
                        "Match without bet", "Send a request");
                
                if (!result) return false;
            }
            else
            {
                var result = await ServiceLocator.GetService<QuestionPopup>().OpenQuestion
                    ($"You have set the bet on the match in the amount of BET", 
                        "Match with bet", "Send a request", Param1: _bet.ToString());

                if (!result) return false;
            }

            return await _communityCenterTypesOfRequest
                .POST_MatchRequest(friend.userId, 
                    calendarController.GetDate(), _bet);
        }
    }
}