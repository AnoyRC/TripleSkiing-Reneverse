using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Rene.Sdk;
using Rene.Sdk.Api.Game.Data;
using UnityEngine.Networking;

namespace ReneVerse
{
    public static class Extensions
    {
        #region ReneApi

        /// <summary>
        /// Retrieves all asset attributes for the user's assets from the ReneVerse API.
        /// </summary>
        /// <param name="api">The ReneVerse API instance.</param>
        /// <returns>A list of asset attributes.</returns>
        public static async Task<List<AssetsResponse.AssetsData.Asset.AssetMetadata.AssetAttribute>>
            GetAssetAttributesAsync(this API api)
        {
            var attributes = new List<AssetsResponse.AssetsData.Asset.AssetMetadata.AssetAttribute>();

            AssetsResponse.AssetsData userAssets = await api.Game().Assets();

            userAssets?.Items.ForEach(asset =>
            {
                asset.Metadata.Attributes.ForEach(attribute => { attributes.Add(attribute); });
            });

            return attributes;
        }

        /// <summary>
        /// Retrieves all asset items for the user from the ReneVerse API.
        /// </summary>
        /// <param name="api">The ReneVerse API instance.</param>
        /// <returns>A list of asset items.</returns>
        public static async Task<List<AssetsResponse.AssetsData.Asset>> GetAssetItemsAsync(this API api)
        {
            AssetsResponse.AssetsData userAssets = await api.Game().Assets();
            return userAssets?.Items;
        }

        #endregion

        /// <summary>
        /// Sends a UnityWebRequest and returns a task that completes when the request is finished.
        /// </summary>
        /// <param name="webRequest">The UnityWebRequest to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static Task<UnityWebRequest> SendWebRequestAsync(this UnityWebRequest webRequest)
        {
            var tcs = new TaskCompletionSource<UnityWebRequest>();

            webRequest.SendWebRequest().completed += operation => { tcs.SetResult(webRequest); };

            return tcs.Task;
        }

        /// <summary>
        /// Adds a period at the end of the string.
        /// </summary>
        /// <param name="str">The string to modify.</param>
        /// <returns>The modified string.</returns>
        public static string AddPoint(this string str)
        {
            return str + ".";
        }

        /// <summary>
        /// Checks if the string is a valid email address.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string is a valid email address, false otherwise.</returns>
        public static bool IsEmail(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }

            try
            {
                var address = new System.Net.Mail.MailAddress(str);
                return address.Address == str;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Separates a camel case string with spaces.
        /// </summary>
        /// <param name="str">The camel case string to separate.</param>
        /// <returns>The separated string.</returns>
        public static string SeparateCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(str[0]);

            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    builder.Append(' ');
                }

                builder.Append(str[i]);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Replaces underscores in a string with spaces.
        /// </summary>
        /// <param name="str">The string to modify.</param>
        /// <returns>The modified string.</returns>
        public static string UnderscoresToSpaces(this string str)
        {
            return str.Replace("_", " ");
        }

        /// <summary>
        /// Converts an API URL to a login URL.
        /// </summary>
        /// <param name="input">The API URL to convert.</param>
        /// <returns>The converted login URL.</returns>
        public static string ToLoginUrl(this string input)
        {
            input = input.Replace("api", "app");
            return "http://" + input.Trim() + "/login";
        }

        /// <summary>
        /// Prepends "Enter your " to a string.
        /// </summary>
        /// <param name="input">The string to modify.</param>
        /// <returns>The modified string.</returns>
        public static string AddEnterYour(this string input)
        {
            return "Enter your " + input;
        }

        /// <summary>
        /// Gets the name of the member that calls this method.
        /// </summary>
        /// <param name="instance">The instance of the calling member.</param>
        /// <param name="memberName">The name of the calling member. This parameter is automatically filled by the compiler.</param>
        /// <returns>The name of the calling member.</returns>
        public static string GetName<T>(this T instance, [CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}