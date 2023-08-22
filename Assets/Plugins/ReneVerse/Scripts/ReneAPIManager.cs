using System.Collections.Generic;
using System.Threading.Tasks;
using Rene.Sdk;
using Rene.Sdk.Api.Game.Data;
using ReneVerse.Demo;
using UnityEngine;


namespace ReneVerse
{
    public static class ReneAPIManager
    {
        private static ReneAPICreds _reneAPICreds;

        /// <summary>
        /// The URL for the ReneVerse site.
        /// </summary>
        public const string SiteURL = "https://app.reneverse.io/login";

        /// <summary>
        /// The name of the ReneVerse settings tab.
        /// </summary>
        public const string TabName = "Reneverse Settings";

        /// <summary>
        /// The path to the ReneVerse settings window in the Unity editor.
        /// </summary>
        public const string WindowReneverseSettings = "Window/" + TabName;


        /// <summary>
        /// Initializes the ReneVerse API with the stored API credentials.
        /// Main connection to ReneVerse service. Once called check <see cref="SiteURL"/> for the notification.
        /// Use retrieved <see cref="API"/> to get all the information needed.
        /// Check <see cref="ReneVerseServiceExample"/> in the Demo Scene for implementation
        /// </summary>
        /// <returns></returns>
        public static API API()
        {
            _reneAPICreds ??= (ReneAPICreds)Resources.Load(nameof(ReneAPICreds), typeof(ScriptableObject));
            return Rene.Sdk.API.Init(_reneAPICreds.APIKey, _reneAPICreds.PrivateKey, _reneAPICreds.GameID);
        }

        /// <summary>
        /// Connects to the ReneVerse game with the provided email.
        /// </summary>
        /// <param name="email">The email to connect with.</param>
        /// <param name="reneAPI"></param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains a boolean indicating whether the connection was successful.</returns>
        public static async Task<bool> GameConnect(string email, API reneAPI)
        {
            return await reneAPI.Game().Connect(email);
        }

        /// <summary>
        /// Mints a random asset from the available asset templates.
        /// Check <see cref="ReneVerseServiceExample"/> in the Demo Scene for implementation
        /// </summary>
        /// <param name="reneAPI">The ReneVerse API instance.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task MintRandom(API reneAPI)
        {
            if (reneAPI == null)
            {
                Debug.Log($"{nameof(MintRandom)} is not possible since {nameof(reneAPI)} is null." +
                          $"Connect to ReneVerse first using na{nameof(API)}");
                return;
            }

            var assetTemplates = await reneAPI.Game().AssetTemplates();
            if (assetTemplates?.Items?.Count > 0)
            {
                var random = new System.Random();
                int randomIndex = random.Next(assetTemplates.Items.Count);

                var assetMinted = await reneAPI.Game().AssetMint
                    (assetTemplates.Items[randomIndex].AssetTemplateId);
                Debug.Log($"{nameof(MintRandom)} is performed");
            }
        }
    }
}