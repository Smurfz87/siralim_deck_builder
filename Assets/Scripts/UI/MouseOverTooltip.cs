using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class MouseOverTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Vector2 positionRelative;
        [SerializeField] private Vector2 pivot;
        [SerializeField] private Vector2 anchorMin;
        [SerializeField] private Vector2 anchorMax;

        [SerializeField] private GameObject panel;

        [SerializeField, TextArea(4, 10)] private string text;

        private Animator animator;

        private Image background;
        private TMP_Text textContainer;

        public MouseOverTooltip(
            Vector2 positionRelative,
            Vector2 pivot,
            Vector2 anchorMin,
            Vector2 anchorMax,
            GameObject panel)
        {
            this.positionRelative = positionRelative;
            this.pivot = pivot;
            this.anchorMin = anchorMin;
            this.anchorMax = anchorMax;
            this.panel = panel;
        }

        public void SetText(string message)
        {
            text = message;
        }

        private void Awake()
        {
            if (panel.SafeIsUnityNull())
            {
                panel = GameObject.FindGameObjectWithTag("Popup");
            }
            
            animator = panel.GetComponent<Animator>();
            background = panel.transform.Find("Background").GetComponent<Image>();
            textContainer = panel.transform.GetChild(0).Find("Text").GetComponent<TMP_Text>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            textContainer.SetText(text);
            panel.GetComponent<RectTransform>().pivot = pivot;
            panel.transform.SetParent(transform, false);
            // panel.transform.parent = transform;
            var position = transform.position;
            panel.GetComponent<RectTransform>().anchorMin = anchorMin;
            panel.GetComponent<RectTransform>().anchorMax = anchorMax;
            panel.GetComponent<RectTransform>().anchoredPosition = positionRelative;


            // panel.transform.GetComponent<RectTransform>().an
            // panel.transform.position = new Vector3(
            //     position.x + positionRelative.x, 
            //     position.y + positionRelative.y);
            animator.Play("ShowModal");
            //textContainer.ForceMeshUpdate();
            Debug.Log(position);
            //StartCoroutine(TriggerPopup());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animator.Play("HideModal");
        }

        private IEnumerator TriggerPopup()
        {
            animator.Play("ShowModal");

            yield return new WaitForSeconds(2);

            animator.Play("HideModal");
        }
    }
}