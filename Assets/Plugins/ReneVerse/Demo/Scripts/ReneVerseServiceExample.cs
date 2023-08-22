using System;
using System.Threading;
using System.Threading.Tasks;
using Rene.Sdk;
using ReneVerse;
using UnityEngine;

namespace ReneVerse.Demo
{
    public class ReneVerseServiceExample : MonoBehaviour
    {
        [Header("Amount of seconds to perform ReneVerse Connect")] [SerializeField] [Range(0, 60)]
        private int secondsToWait = 45;

        private Coroutine _connectReneServiceCoroutine;
        private API reneApi;

        public event Action<int> OnReneConnectSecondPassed;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void ReneVerseCancel()
        {
            if (_connectReneServiceCoroutine != null) StopCoroutine(_connectReneServiceCoroutine);
        }

        public async void ReneVerseConnectClicked(string email, Action<API> onConnectedReneverse = null,
            Action onConnectionExpired = null, CancellationToken cancellationToken = default)
        {
            reneApi = ReneAPIManager.API();
            await reneApi.Game().Connect(email);
            await ConnectReneService(reneApi, onConnectedReneverse, onConnectionExpired, cancellationToken);
        }


        private async Task ConnectReneService(API reneApi, Action<API> onConnectedReneverse = null,
            Action onConnectionExpired = null, CancellationToken cancellationToken = default)
        {
            var counter = 0;
            var userConnected = false;
            var secondsToIncrement = 1;
            while (counter < secondsToWait && !userConnected && !cancellationToken.IsCancellationRequested)
            {
                OnReneConnectSecondPassed?.Invoke(secondsToWait - counter);
                if (reneApi.IsAuthorized())
                {
                    onConnectedReneverse?.Invoke(reneApi);
                    //await GetUserAssetsAsync(reneApi, onConnectedReneverse);
                    userConnected = true;
                }

                await Task.Delay(secondsToIncrement * 1000, cancellationToken); // Wait for secondsToIncrement seconds
                counter += secondsToIncrement;
            }

            if (!userConnected && !cancellationToken.IsCancellationRequested) onConnectionExpired?.Invoke();
        }

        public async void MintRandomAsset() => await ReneAPIManager.MintRandom(reneApi);
    }
}