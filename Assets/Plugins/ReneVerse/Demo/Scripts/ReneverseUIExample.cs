using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rene.Sdk;
using ReneVerse;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ReneVerse.Demo
{
    public class ReneverseUIExample : MonoBehaviour, IPointerClickHandler
    {
        private const string EmailFail = "Type Correct Email";
        private const string Connect = "Connect";
        private const string Connected = "Cancel Connection";
        private CancellationTokenSource _cancellationTokenSource;

        [SerializeField] private ReneVerseServiceExample _reneVerseServiceExample;

        [Header("ReneVerse Connect Button")] [SerializeField]
        private Button reneVerseConnect;

        private const string CancelConnectionButtonText =
            "Cancel Connection";


        private string ReneWebsiteHyperlink =
            $"<link=\"{ReneAPIManager.SiteURL}\"><color=#0000FF>website</color></link>";

        private string ReneEditorWindow =
            $"<link=\"{ReneAPIManager.WindowReneverseSettings}\"><color=#7200FF>{ReneAPIManager.WindowReneverseSettings}</color></link>";

        [SerializeField] private TextMeshProUGUI connectButtonText;
        [SerializeField] private TextMeshProUGUI reneOutput;


        [Header("ReneVerse Email Input Field")] [SerializeField]
        private TMP_InputField reneVerseEmail;

        [FormerlySerializedAs("reneVerseTimer")] [SerializeField]
        private TextMeshProUGUI reneTextBeforeConnection;

        
        [Header("User Connected UI")] 
        
        [SerializeField] private ContentSizeFitter assetsUIPlaceHolder;
        [SerializeField] private RectTransform connectedUserUI;
        [SerializeField] private Button mintButton;

        private Coroutine _connectingDotsCoroutine;

        private bool IsNotValidEmail => !reneVerseEmail.text.IsEmail();
        private bool EmptyEmailField => string.IsNullOrEmpty(reneVerseEmail.text);


        #region Enable/Disable

        private void OnEnable()
        {
            reneVerseConnect.onClick.AddListener(ReneVerseConnectClicked);
            mintButton.onClick.AddListener(() => _reneVerseServiceExample.MintRandomAsset());
            _reneVerseServiceExample.OnReneConnectSecondPassed += ShowConnectSecondsPassed;
        }

        private void OnDisable()
        {
            _reneVerseServiceExample.OnReneConnectSecondPassed -= ShowConnectSecondsPassed;
        }

        private void ShowConnectSecondsPassed(int secondsLeft)
        {
            string text = $"You have {secondsLeft} seconds to accept entry on the {ReneWebsiteHyperlink}";
            reneTextBeforeConnection.text = text;
        }

        #endregion


        private void Start()
        {
            InitialConnectState();
        }

        private void InitialConnectState()
        {
            EnableRegistrationUI(true);
            
            
            reneTextBeforeConnection.text =
                $"To perform the connection feel free to log in on our website: {ReneWebsiteHyperlink} " +
                $"Don't forget to populate your credentials: {ReneEditorWindow}";
            connectButtonText.text = "Connect";
            GameFactoryExample.DestroyInstantiatedReneAssets();
        }

        private void DestroyChildren<T>(ContentSizeFitter parentObject) where T: Behaviour
        {
            var componentsInChildren = parentObject.GetComponentsInChildren<T>();
            foreach (var reneAsset in componentsInChildren)
            {
                Destroy(reneAsset.gameObject);
            }
        }

        private void ReneVerseConnectClicked()
        {
            if (IsNotValidEmail)
            {
                ChangeText(connectButtonText, EmailFail);
                return;
            }

            if (IsCancelConnectionAvailable)
            {
                CancelReneConnection();
                return;
            }
            

            connectButtonText.text = CancelConnectionButtonText;
            _cancellationTokenSource = new CancellationTokenSource();
            //Here happens the connection itself
            _reneVerseServiceExample.ReneVerseConnectClicked
                (reneVerseEmail.text, OnReneConnected, OnReneConnectionExpired, _cancellationTokenSource.Token);
        }

        private void CancelReneConnection()
        {
            _cancellationTokenSource?.Cancel();
            _reneVerseServiceExample.ReneVerseCancel();
            _cancellationTokenSource?.Dispose();
            InitialConnectState();
        }

        private bool IsCancelConnectionAvailable => connectButtonText.text == CancelConnectionButtonText;
        private void EnableRegistrationUI(bool isEnabled)
        {
            reneVerseEmail.interactable = isEnabled;
            reneTextBeforeConnection.gameObject.SetActive(isEnabled);
            connectedUserUI.gameObject.SetActive(!isEnabled);
        }
        private void EnableConnectedUI(bool isEnabled)
        {
            reneVerseEmail.interactable = !isEnabled;
            connectedUserUI.gameObject.SetActive(isEnabled);
            reneTextBeforeConnection.gameObject.SetActive(!isEnabled);
        }

        private void OnReneConnectionExpired()
        {
            connectButtonText.text = Connect;
            reneTextBeforeConnection.text =
                $"Time is run out. Try reconnecting via registered email. Here is our {ReneWebsiteHyperlink}";
            EnableRegistrationUI(true);
        }

        private async void OnReneConnected(API api)
        {
            var items = await api.GetAssetItemsAsync();

            
            var tasks = new List<Task<ReneAsset>>();
            foreach (var item in items)
            {
                tasks.Add(GameFactoryExample.Instance.CreateReneAssetUI(item, assetsUIPlaceHolder.transform));
            }

            await Task.WhenAll(tasks);

            EnableConnectedUI(true);

            connectButtonText.text = CancelConnectionButtonText;
        }

        private void ChangeText(TextMeshProUGUI textMeshProUGUI, string text) => textMeshProUGUI.text = text;


        public void OnPointerClick(PointerEventData eventData)
        {
            int linkIndex =
                TMP_TextUtilities.FindIntersectingLink(reneTextBeforeConnection, Input.mousePosition, Camera.main);

            if (linkIndex != -1)
            {
                string linkID = reneTextBeforeConnection.textInfo.linkInfo[linkIndex].GetLinkID();
                if (linkID.Contains("https"))
                {
                    Application.OpenURL(linkID); // Replace with the URL of your hyperlink
                }
#if UNITY_EDITOR
                else if (linkID.Contains("Window"))
                {
                    EditorApplication.ExecuteMenuItem(linkID);
                }
#endif
                else
                {
                    print($"{linkID} is not found check {reneTextBeforeConnection.name} text field");
                }
            }
        }
    }
}