using System.Collections;
using frame8.Logic.Misc.Other.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject modalDialog;
    [SerializeField] private TMP_Text msgText;
    [SerializeField] private Animator animator;

    public void ShowMessage(string message)
    {
        StartCoroutine(ShowModal(message));
    }

    private IEnumerator ShowModal(string message)
    {
        msgText.text = message;
        animator.Play("ShowModal");
        
        yield return new WaitForSeconds(2);
        
        animator.Play("HideModal");
    }
}