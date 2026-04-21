using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HN.Code.Reference.Texts
{
    [CreateAssetMenu(fileName = "TextContainer", menuName = "SO/Text/Container", order = 0)]
    public class TextContainerSO : ScriptableObject
    {
        [SerializeField] private List<TextDataSO> textDataList;

        private Dictionary<object, TextDataSO> _textPairs = new Dictionary<object, TextDataSO>();

        private void OnEnable()
        {
            _textPairs = textDataList?.ToDictionary
            (
                data => data.GetKey(),
                data => data
            );
        }

        public TextDataSO GetTextData(object key) => _textPairs.GetValueOrDefault(key);
    }
}