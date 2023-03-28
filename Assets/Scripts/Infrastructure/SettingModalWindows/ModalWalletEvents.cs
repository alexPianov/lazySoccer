using System;
using MetaMask;
using MetaMask.Transports.Unity.UI;
using MetaMask.Unity;
using MetaMask.Unity.Samples;
using TMPro;
using UnityEngine;

namespace LazySoccer.SceneLoading.Infrastructure.ModalWindows
{
    public class ModalWalletEvents : MonoBehaviour
    {
        [SerializeField] private MetaMaskDemo metaMaskTest;
        [SerializeField] private ModalWallet modalWallet;
        
        private void Start()
        {
            this.metaMaskTest.onWalletConnected += OnWalletConnected;
            this.metaMaskTest.onWalletDisconnected += OnWalletDisconnected;
            this.metaMaskTest.onWalletPaused += OnWalletPaused;
            this.metaMaskTest.onWalletReady += OnWalletReady;
            this.metaMaskTest.onSignSend += OnSignSend;
            this.metaMaskTest.onTransactionSent += OnTransactionSent;
            this.metaMaskTest.onTransactionResult += OnTransactionResult;
            this.metaMaskTest.onTransactionFailed += OnTransactionFailed;
        }

        private void OnDisable()
        {
            this.metaMaskTest.onWalletConnected -= OnWalletConnected;
            this.metaMaskTest.onWalletDisconnected += OnWalletDisconnected;
            this.metaMaskTest.onWalletPaused += OnWalletPaused;
            this.metaMaskTest.onWalletReady -= OnWalletReady;
            this.metaMaskTest.onSignSend -= OnSignSend;
            this.metaMaskTest.onTransactionSent -= OnTransactionSent;
            this.metaMaskTest.onTransactionResult -= OnTransactionResult;
            this.metaMaskTest.onTransactionFailed -= OnTransactionFailed;
        }
        
        private void OnWalletReady(object sender, EventArgs e)
        {
            WalletStartVisuals();
        }
        private void OnWalletPaused(object sender, EventArgs e)
        {
            if(Application.platform == RuntimePlatform.Android && MetaMaskUnityUITransport.DefaultInstance.IsDeeplinkAvailable() || Application.platform == RuntimePlatform.IPhonePlayer && MetaMaskUnityUITransport.DefaultInstance.IsDeeplinkAvailable())
            {
                MetaMaskUnity.Instance.Wallet.Dispose();
            }
            
            WalletStopVisuals();
        }

        private void OnWalletConnected(object sender, EventArgs e)
        {
            WalletStartVisuals();
        }

        private void OnWalletDisconnected(object sender, EventArgs e)
        {
            WalletStopVisuals();
        }

        private void WalletStartVisuals()
        {
            ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Wallet Ready");
        }

        private void WalletStopVisuals()
        {
            //ServiceLocator.GetService<GeneralPopupMessage>().ShowInfo("Scan the QR in your MetaMask app");
        }

        private void OnSignSend(object sender, EventArgs e)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("Sign has been sent to your wallet");
            
            modalWallet.CloseMessagePanel();
        }

        private void OnTransactionSent(object sender, EventArgs e)
        {
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("Transaction Sent has been sent to your wallet");
        }

        private void OnTransactionResult(object sender, MetaMaskEthereumRequestResultEventArgs e)
        {
            var result = string.Format("<b>Method Name:</b><br> {0} <br> <br> <b>Transaction Details:</b><br>{1}", 
                e.Request.Method, e.Result.ToString());
            
            Debug.Log("OnTransactionResult: " + e.Request.Method);
            
            if (e.Request.Method == "metamask_getProviderState")
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("Wallet is found");
                
                return;
            }

            if (e.Request.Method == "eth_requestAccounts")
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("Wallet is connected");
                
                return;
            }
            
            if (e.Request.Method == "eth_signTypedData_v4")
            {
                ServiceLocator.GetService<GeneralPopupMessage>()
                    .ShowInfo("Transaction is signed");
                
                return;
            }
            
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("Transaction is finished");
        }
        
        private void OnTransactionFailed(object sender, MetaMaskEthereumRequestFailedEventArgs e)
        {
            Debug.Log("OnTransactionFailed: " + e.Error);
            
            ServiceLocator.GetService<GeneralPopupMessage>()
                .ShowInfo("Transaction is failed", false);
        }
    }
}