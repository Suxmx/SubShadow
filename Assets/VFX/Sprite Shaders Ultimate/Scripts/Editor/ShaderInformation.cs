using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteShadersUltimate
{
    [CreateAssetMenu(fileName = "ShaderName",menuName = "Shader/Sprite Shaders Ultimate Info File")]
    public class ShaderInformation : ScriptableObject
    {
        [Header("Main:")]
        [TextArea(6,10)]
        public string shaderDescription;
        public List<ShaderTitle> titles = new List<ShaderTitle>();
        public List<ShaderSuffx> suffixInformation = new List<ShaderSuffx>();
        public List<ShowIfToggled> showIfs = new List<ShowIfToggled>();
        public List<string> requiredComponents = new List<string>();
    }

    [System.Serializable]
    public class ShaderSuffx
    {
        public string property = "";
        [TextArea]
        public string information = "";
    }

    [System.Serializable]
    public class ShaderTitle
    {
        public string property = "";
        public string title = "";
    }

    [System.Serializable]
    public class ShowIfToggled
    {
        public string toggleVariable;
        public List<string> shownVariables;
        public bool reverse = false;
    }
}

