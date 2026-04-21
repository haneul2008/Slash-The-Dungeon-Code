using UnityEngine;
using UnityEngine.Serialization;

namespace HN.Code.Players
{
    public enum BuffType
    {
        Recovery,
        Upgrade
    }
    
    public class BuffEffect : MonoBehaviour
    {
        [SerializeField] private Animator effectAnimator;
        [SerializeField] private SpriteRenderer spriteRenderer;

        private readonly int _recoveryHash = Animator.StringToHash("Recovery");
        private readonly int _upgradeHash = Animator.StringToHash("Upgrade");
        
        public void PlayEffect(BuffType buffType)
        {
            switch (buffType)
            {
                case BuffType.Recovery:
                    effectAnimator.SetTrigger(_recoveryHash);
                    break;
                case BuffType.Upgrade:
                    effectAnimator.SetTrigger(_upgradeHash);
                    break;
            }
        }

        public void AnimationEnd() => spriteRenderer.sprite = null;
    }
}