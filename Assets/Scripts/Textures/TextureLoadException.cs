using System;
using UnityEngine.Networking;

namespace RrTestTask
{
    public sealed class TextureLoadException : Exception
    {
        private const string MessageString = "Failed to load a texture.";
        public UnityWebRequest.Result? RequestResult { get; }

        public TextureLoadException(string downloadError) : base($"{MessageString} Download error: {downloadError}")
        {
        }
        
        public TextureLoadException(UnityWebRequest.Result requestResult)
        {
            RequestResult = requestResult;
        }

        public TextureLoadException(Exception inner) : base(MessageString, inner)
        {
        }
    }
}