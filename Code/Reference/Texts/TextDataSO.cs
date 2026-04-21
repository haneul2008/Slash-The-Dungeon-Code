using System.Collections.Generic;
using UnityEngine;

namespace HN.Code.Reference.Texts
{
    [CreateAssetMenu(fileName = "TextData", menuName = "SO/Text/TextData", order = 0)]
    public class TextDataSO : ScriptableObject
    {
        [SerializeField] private string key;

        public List<string> text;
        public string color;
        
        public virtual object GetKey()
        {
            return key;
        }
    }
}