using System;
using DG.Tweening;
using UnityEngine;

namespace HN.Code.Entities
{
    public class EntityRenderer : MonoBehaviour
    {
        [SerializeField] private float dissolveDelay = 0.5f;
        [SerializeField] private float fadeDelay = 0.5f;
        
        private SpriteRenderer _renderer;
        private Material _targetMat;
        private int _dissolveHash = Shader.PropertyToID("_Fade");
        private int _fadeSliderHash = Shader.PropertyToID("_FadeSlider");
        private int _alphaOffsetHash = Shader.PropertyToID("_FadeAlphaOffset");

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _targetMat = _renderer.material;
        }

        public void Dissolve(float targetValue, Action onComplete)
        {
            _targetMat.DOFloat(targetValue, _dissolveHash, dissolveDelay).OnComplete(() => onComplete?.Invoke());
        }

        public void Fade(float targetValue, float targetAlphaValue, Action onComplete)
        {
            _targetMat.DOFloat(targetValue, _fadeSliderHash, fadeDelay).OnComplete(() => onComplete?.Invoke());
            _targetMat.DOFloat(targetAlphaValue, _alphaOffsetHash, fadeDelay);
        }
    }
}