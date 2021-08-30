using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {

        [SerializeField] private GameObject modalDialog;
        [SerializeField] private TMP_Text msgText;
        [SerializeField] private Animator modalAnim;
        [SerializeField] private Animator teamPanelAnim;
        [SerializeField] private Animator searchPanelAnim;
        
        private static readonly int ShowTeam = Animator.StringToHash("ShowTeam");
        private static readonly int HideTeam = Animator.StringToHash("HideTeam");
        private static readonly int OpenSearch = Animator.StringToHash("OpenSearch");
        private static readonly int CloseSearch = Animator.StringToHash("CloseSearch");
        
        public void ShowMessage(string message)
        {
            StartCoroutine(ShowModal(message));
        }

        private IEnumerator ShowModal(string message)
        {
            msgText.text = message;
            modalAnim.Play("ShowModal");
        
            yield return new WaitForSeconds(2);
        
            modalAnim.Play("HideModal");
        }

        public void ShowTeamPanel()
        {
            StartCoroutine(TeamPanelAnim(ShowTeam));
            //teamPanelAnim.Play("ShowTeam");
        }

        public void HideTeamPanel()
        {
            StartCoroutine(TeamPanelAnim(HideTeam));
        }

        public void ShowSearchPanel()
        {
            StartCoroutine(SearchPanelAnim(OpenSearch));
        }

        public void HideSearchPanel()
        {
            StartCoroutine(SearchPanelAnim(CloseSearch));
        }

        private IEnumerator TeamPanelAnim(int anim)
        {
            teamPanelAnim.SetTrigger(anim);
            yield return new WaitForSeconds(1);
        }

        private IEnumerator SearchPanelAnim(int anim)
        {
            searchPanelAnim.SetTrigger(anim);
            yield return new WaitForSeconds(1);
        }
    }
}