using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace HN.Code.Core.Editor
{
    public abstract class EnumGenerator<T> : ScriptableObject where T : System.Enum
    {
        [SerializeField] protected string enumPath;

        [ContextMenu("Generate Enum")]
        protected void GenerateEnum()
        {
            StringBuilder codeBuilder = GetCodeBuilder();

            Type enumType = typeof(T);
            
            string code = 
                string.Format(EnumFormat.FormatString, enumType.Namespace, enumType.Name, codeBuilder);
            
            File.WriteAllText(enumPath, code);
        }

        protected abstract StringBuilder GetCodeBuilder();
    }

    internal static class EnumFormat
    {
        public static string FormatString { get; private set; } =
            @"namespace {0}
    {{
        public enum {1}
        {{
            {2}
        }}
    }}";
    }
}