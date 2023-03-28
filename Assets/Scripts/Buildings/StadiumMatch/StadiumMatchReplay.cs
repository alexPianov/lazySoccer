using System;
using System.IO;
using Cysharp.Threading.Tasks;
using LazySoccer.Network;
using LazySoccer.Status;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Buildings.StadiumMatch
{
    public class StadiumMatchReplay : MonoBehaviour
    {
        [SerializeField] private Camera cameraMatch;
        [SerializeField] private MatchDataParser matchDataParser;
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private GameObject panelPause;

        [SerializeField] private Button buttonPause;
        [SerializeField] private Button buttonPlay;
        [SerializeField] private Button buttonSpeedOne;
        [SerializeField] private Button buttonSpeedTwo;
        
        [SerializeField] private TMP_Text textTurnCounter;

        public void Awake()
        {
            cameraMatch.enabled = false;
            matchDataParser.ClearMatchData();
            panelPause.SetActive(false);
            turnManager.turnCounter.AddListener(TurnCounterDisplay);
            turnManager.finish.AddListener(Finish);
        }

        private void TurnCounterDisplay(string value)
        {
            textTurnCounter.text = value;
        }

        private void Finish()
        {
            DisableMatch();
            
            ServiceLocator.GetService<StadiumStatusMatchOld>()
                .SetAction(StatusStadiumMatchOld.Statistics);
        }

        public async UniTask ShowMatch(string gameId)
        {
            turnManager.Reboot();
            matchDataParser.ClearMatchData();
            
            var matchJson = await ServiceLocator.GetService<StadiumTypesOfRequests>().GetMatchTime(gameId);

            Debug.Log(gameId);

            cameraMatch.enabled = true;
            matchDataParser.ParseMatchData(matchJson);
            while (matchDataParser == null)
            {
                await UniTask.Delay(1);
            }
            turnManager.StartMatch(0);
            
            ActiveButton(ControlButton.Play);
            
            if (turnManager.IsPaused) Pause(); 
        }

        public enum ControlButton
        {
            Pause, Play, SpeedOne, SpeedTwo
        }
        
        public void DisableMatch()
        {
            cameraMatch.enabled = false;
            Pause(); 
            panelPause.SetActive(false);
        }

        public void EnableMatch()
        {
            cameraMatch.enabled = true;
            Pause(); 
            panelPause.SetActive(turnManager.IsPaused);
            
            //ActiveButton(currentButton);
        }

        public void Exit()
        {
            matchDataParser.ClearMatchData();
            turnManager.PrepareTeamsForReset();
            
            cameraMatch.enabled = false;
            //if (turnManager.IsPaused) Pause(); 
            panelPause.SetActive(false);
            turnManager.CancelAndExit();
        }

        public void Pause()
        {
            turnManager.PauseMatch();
            panelPause.SetActive(turnManager.IsPaused);

            if (turnManager.IsPaused)
            {
                ActiveButton(ControlButton.Pause);
            }
            else
            {
                ActiveButton(currentButton);
            }
        }

        public void Play()
        {
            turnManager.SetMatchAcceleration(0); 
            ActiveButton(ControlButton.Play);
        }

        public void SpeedOne()
        {
            turnManager.SetMatchAcceleration(1); 
            ActiveButton(ControlButton.SpeedOne);
        }

        public void SpeedTwo()
        {
            turnManager.SetMatchAcceleration(2);
            ActiveButton(ControlButton.SpeedTwo);
        }
        
        private ControlButton currentButton;
        private void ActiveButton(ControlButton button)
        {
            if (button == ControlButton.Pause)
            {
                buttonPause.interactable = true;
                buttonPlay.interactable = false;
                buttonSpeedOne.interactable = false;
                buttonSpeedTwo.interactable = false;
                return;
            }
            
            currentButton = button; 
            
            if (button == ControlButton.Play)
            {
                buttonPause.interactable = true;
                buttonPlay.interactable = false;
                buttonSpeedOne.interactable = true;
                buttonSpeedTwo.interactable = true;
            }
            
            if (button == ControlButton.SpeedOne)
            {
                buttonPause.interactable = true;
                buttonPlay.interactable = true;
                buttonSpeedOne.interactable = false;
                buttonSpeedTwo.interactable = true;
            }
            
            if (button == ControlButton.SpeedTwo)
            {
                buttonPause.interactable = true;
                buttonPlay.interactable = true;
                buttonSpeedOne.interactable = true;
                buttonSpeedTwo.interactable = false;
            }
        }
    }
}