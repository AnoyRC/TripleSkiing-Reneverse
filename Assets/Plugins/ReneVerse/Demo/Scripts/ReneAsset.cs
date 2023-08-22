using System.Collections;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ReneVerse.Demo
{
    public class ReneAsset: MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI assetDescriptioin;
        [SerializeField] private Image assetImage;

        public async Task SetProperties(string assetDescription, string assetImageURL, ICoroutineRunner coroutineRunner)
        {
            assetDescriptioin.text = assetDescription;
            await LoadImageAsync(assetImageURL);
        }
        
        
        private IEnumerator LoadImage(string imageUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                assetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
        
        public async Task LoadImageAsync(string imageUrl)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
            await request.SendWebRequestAsync();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                assetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }

    }
    

}