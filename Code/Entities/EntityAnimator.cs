using System.Collections.Generic;
using UnityEngine;

namespace HN.Code.Entities
{
    public class EntityAnimator : MonoBehaviour
    {
        [SerializeField]private Animator _anim;
        private Dictionary<string, int> _hashPairs = new Dictionary<string, int>();

        private void Awake()
        {
            if(_anim == null)
                _anim = GetComponent<Animator>();
        }

        public void PlayAnimation(string animName, bool value)
        {
            _anim.SetBool(GetHash(animName), value);
        }

        public void PlayAnimation(string animName, float value) => _anim.SetFloat(GetHash(animName), value);
        public void PlayAnimation(string animName, int value) => _anim.SetInteger(GetHash(animName), value);
        public void PlayAnimation(string animName) => _anim.SetTrigger(GetHash(animName));

        private int GetHash(string animName)
        {
            _anim ??= GetComponent<Animator>();
            
            if (_hashPairs.TryGetValue(animName, out int hash))
            {
                return hash;
            }

            hash = Animator.StringToHash(animName);
            _hashPairs.Add(animName, hash);
            return hash;
        }
    }
}