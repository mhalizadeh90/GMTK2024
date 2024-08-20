using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class TextController : MonoBehaviour
    {
        [SerializeField] private Image textBackground;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float fontFadeSpeed = 1;
        [SerializeField] private float backgroundFadeSpeed = 1;
        [SerializeField] private float delayInFadeTextAndBackground = 1;
        [SerializeField] private float thresholdToFade = 0.01f;
        [SerializeField] private float fontDelayInAnimation = 0.1f;
        [SerializeField] private float delayToHideText = 2f;

        private Color _fontBaseColor, _fontTransparentColor, _backgroundBaseColor, _backgroundTransparentColor;
        private WaitForSeconds _delayCharacterAnimation, _delayToHideText;
        public static TextController Instance;

        private Coroutine _currentCoroutine;

        private void Awake()
        {
            Instance = this;
            _fontBaseColor = text.color;
            _fontTransparentColor = text.color;
            _fontTransparentColor.a = 0;
            _delayCharacterAnimation = new WaitForSeconds(fontDelayInAnimation);
            _delayToHideText = new WaitForSeconds(delayToHideText);
            _backgroundBaseColor = textBackground.color;
            _backgroundTransparentColor = textBackground.color;
            _backgroundTransparentColor.a = 0;

            HideText();
        }

        public void HideText(bool smoothFadeOut = false)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            if (smoothFadeOut)
            {
                _currentCoroutine = StartCoroutine(FadeOut());
            }
            else
            {
                textBackground.enabled = false;
                text.enabled = false;
            }
        }

        public void ShowText(string input, bool smoothFadeOut = false, float delayBeforeStart = 0,
            Action callback = null)
        {
            if (_currentCoroutine != null)
            {
                StopCoroutine(_currentCoroutine);
            }

            if (smoothFadeOut)
            {
                _currentCoroutine = StartCoroutine(FadeIn(input, delayBeforeStart, callback));
            }
            else
            {
                textBackground.color = _backgroundBaseColor;
                textBackground.enabled = true;
                text.color = _fontBaseColor;
                text.text = input;
                text.enabled = true;
            }
        }

        private IEnumerator FadeOut()
        {
            // Fade out text
            text.color = _fontBaseColor;
            text.enabled = true;
            while (text.color.a > thresholdToFade)
            {
                text.color = Color.Lerp(text.color, _fontTransparentColor, Time.deltaTime * fontFadeSpeed);
                yield return null;
            }

            text.color = _fontTransparentColor;
            text.enabled = false;

            yield return new WaitForSeconds(delayInFadeTextAndBackground);

            // Fade out background
            textBackground.color = _backgroundBaseColor;
            textBackground.enabled = true;
            while (textBackground.color.a > thresholdToFade)
            {
                textBackground.color = Color.Lerp(textBackground.color, _backgroundTransparentColor,
                    Time.deltaTime * backgroundFadeSpeed);
                yield return null;
            }

            textBackground.color = _backgroundTransparentColor;
            textBackground.enabled = false;

            _currentCoroutine = null;
        }

        private IEnumerator FadeIn(string input, float delayBeforeStart, Action callback = null)
        {
            yield return new WaitForSeconds(delayBeforeStart);

            // Fade in background
            var thresholdToFadeIn = 1 - thresholdToFade;
            textBackground.color = _backgroundTransparentColor;
            textBackground.enabled = true;
            text.text = "";

            while (textBackground.color.a < thresholdToFadeIn)
            {
                textBackground.color = Color.Lerp(textBackground.color, _backgroundBaseColor,
                    Time.deltaTime * backgroundFadeSpeed);
                yield return null;
            }

            textBackground.color = _backgroundBaseColor;

            yield return new WaitForSeconds(delayInFadeTextAndBackground);

            // Animate text
            text.color = _fontBaseColor;
            text.enabled = true;
            for (var i = 0; i < input.Length; i++)
            {
                text.text += input[i];
                yield return _delayCharacterAnimation;
            }

            yield return _delayToHideText;

            _currentCoroutine = StartCoroutine(FadeOut());

            callback?.Invoke();
        }
    }
}