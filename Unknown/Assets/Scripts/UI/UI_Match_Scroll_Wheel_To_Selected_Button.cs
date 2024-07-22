using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SG
{
    public class UI_Match_Scroll_Wheel_To_Selected_Button : MonoBehaviour
    {
        [SerializeField] private GameObject currentSelected;
        [SerializeField] private GameObject previousSelected;
        [SerializeField] private RectTransform currentSelectedTransform;

        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private ScrollRect scrollRect;


        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            if (currentSelected != null)
            {
                if (currentSelected != previousSelected)
                {
                    previousSelected = currentSelected;
                    currentSelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currentSelectedTransform);
                }
            }
        }

        private void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

            // we only want to lock the position on the y axis (up and down)
            newPosition.x = 0;

            contentPanel.anchoredPosition = newPosition;
        }
    }
}
