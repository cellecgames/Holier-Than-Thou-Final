﻿using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.CustomizationMenu
{
    public class CustomizationItem1 : FancyScrollViewCell<ItemData, Context>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;
        [SerializeField] Text messageLarge = default;
        [SerializeField] Image image = default;
        [SerializeField] Image imageLarge = default;
        [SerializeField] Button button = default;

		//private ScrollView scrollview;

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        void Start()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        }

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Message;
            messageLarge.text = Index.ToString();

            image.sprite = transform.parent.parent.gameObject.GetComponent<ScrollView>().Covers[Index];

            var selected = Context.SelectedIndex == Index;
            imageLarge.color = selected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
            image.color = selected
                ? new Color32(255, 255, 255, 255)
                : new Color32(255, 255, 255, 77);
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }

            animator.speed = 0;
        }

        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
