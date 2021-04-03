using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace RrTestTask
{
    public class WebTextureLoader : ITextureLoader
    {
        private readonly int timeoutSec;

        /// <param name="timeoutSec">The time after the request start when it will be cancelled</param>
        public WebTextureLoader(int timeoutSec)
        {
            this.timeoutSec = timeoutSec;
        }

        /// <exception cref="TextureLoadException"></exception>
        public async Task<Texture2D> Load(string address, CancellationToken cancellationToken)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(address);
            request.timeout = timeoutSec;
            await request.SendWebRequest().WithCancellation(cancellationToken);

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new TextureLoadException(request.result);
            }

            if (!request.downloadHandler.isDone)
            {
                throw new TextureLoadException(request.downloadHandler.error);
            }

            return DownloadHandlerTexture.GetContent(request);
        }
    }
}