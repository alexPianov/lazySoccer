using System;
using Cysharp.Threading.Tasks;
using LazySoccer.Status;
using MetaMask.Unity.Samples;
using Scripts.Infrastructure.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazySoccer.SceneLoading.Infrastructure.ModalWindows
{
    public class ModalWallet : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private TMP_InputField inputName;
        [SerializeField] private TMP_InputField inputMessage;

        [Header("Balance")] 
        [SerializeField] private TMP_Text textUSDTBalance;
        [SerializeField] private TMP_Text textSolanaBalance;

        [Header("Button")] 
        [SerializeField] private Button buttonOpenDeepLink;
        [SerializeField] private Button buttonOpenMessager;
        [SerializeField] private Button buttonSendMessage;
        [SerializeField] private Button buttonSendTransaction;

        [Header("Panel")] 
        [SerializeField] private GameObject panelMessages;
        
        [Header("Refs")] 
        [SerializeField] private MetaMaskDemo MetaMaskDemo;

        private bool isConnected;

        public void ConnectWallet()
        {
            if(isConnected) return;

            buttonOpenMessager.onClick.AddListener(OpenMessagePanel);
            buttonSendMessage.onClick.AddListener(SendMessage);

            inputName.onValueChanged.AddListener(InputName);
            inputMessage.onValueChanged.AddListener(InputMessage);
            
            buttonSendTransaction.onClick.AddListener(SendTransaction);
            
            ActiveButtons(false);
            
            if (buttonOpenDeepLink)
            {
                buttonOpenDeepLink.onClick.AddListener(OpenDeepLink);
                buttonOpenMessager.gameObject.SetActive(false);
                buttonSendTransaction.gameObject.SetActive(false);
            }

            MetaMaskDemo.onWalletConnected += Ready;
            
            MetaMaskDemo.onTransactionResult += Result;
            
            MetaMaskDemo.onTransactionFailed += Failed;
            
            MetaMaskDemo.Connect();

            isConnected = true;
        }

        public void OpenDeepLink()
        {
            MetaMaskDemo.OpenDeepLink();
        }

        private bool IsReady;
        private void Ready(object obj, EventArgs args)
        {
            Debug.Log("Wallet is Ready");
            
            if (buttonOpenDeepLink)
            {
                buttonOpenMessager.gameObject.SetActive(true);
                buttonSendTransaction.gameObject.SetActive(true);
                buttonOpenDeepLink.gameObject.SetActive(false);
            }
            
            IsReady = true;
            ActiveButtons(true);
        }
        
        private void Result(object obj, EventArgs args)
        {
            Debug.Log("Transaction finished");
            if(IsReady) ActiveButtons(true);
        }
        
        private void Failed(object obj, EventArgs args)
        {
            Debug.Log("Transaction failed");
            if(IsReady) ActiveButtons(true);
        }
        
        public async void SendMessage()
        {
            ActiveButtons(false);
            CloseMessagePanel();
            MetaMaskDemo.Sign(userName, userMessage);
            await UniTask.Delay(4000);
            ActiveButtons(true);
        }

        private async void SendTransaction()
        {
            ActiveButtons(false);
            MetaMaskDemo.SendTransaction();
            await UniTask.Delay(4000);
            ActiveButtons(true);
        }

        private void ActiveButtons(bool state)
        {
            buttonOpenMessager.interactable = state;
            buttonSendTransaction.interactable = state;
        }
        
        private string userName = "";
        private void InputName(string value)
        {
            userName = value;
            CheckForm();
        }
        
        private string userMessage = "";
        private void InputMessage(string value)
        {
            userMessage = value;
            CheckForm();
        }

        private void CheckForm()
        {
            if(userName != null && userMessage != null && userName.Length > 1 && userMessage.Length > 1)
            {
                buttonSendMessage.interactable = true;
            }
            else
            {
                buttonSendMessage.interactable = false;
            }
        }

        private void OpenMessagePanel()
        {
            inputMessage.text = "";
            inputName.text = "";
            panelMessages.SetActive(true);
            CheckForm();
        }

        public void CloseMessagePanel()
        {
            panelMessages.SetActive(false);
        }

        public void CloseWallet()
        {
            ServiceLocator.GetService<ModalWindowStatus>().SetAction(StatusModalWindows.None);
        }
    }
}