using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReneVerse.Demo
{
    public class HyperlinkHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        public void OnPointerClick(PointerEventData eventData)
        {
            print("clicked");
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshProUGUI, Input.mousePosition, Camera.main);

            if (linkIndex != -1)
            {
                print("Intersected!");
                string linkID = _textMeshProUGUI.textInfo.linkInfo[linkIndex].GetLinkID();

                Application.OpenURL(linkID); // Replace with the URL of your hyperlink
            }
        }

    }
}