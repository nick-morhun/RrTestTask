using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace RrTestTask
{
    public class RandomTextureLoader
    {
        private readonly ITextureLoader textureLoader;
        private readonly string addressFormat;

        /// <param name="textureLoader"></param>
        /// <param name="addressFormat">"Should have parrameter 0 - width, parameter 1 - height"</param>
        public RandomTextureLoader([NotNull] ITextureLoader textureLoader, [NotNull] string addressFormat)
        {
            this.textureLoader = textureLoader ?? throw new ArgumentNullException(nameof(textureLoader));
            this.addressFormat = addressFormat ?? throw new ArgumentNullException(nameof(addressFormat));
        }

        /// <exception cref="TextureLoadException"></exception>
        public async Task<Texture2D> Load(uint height, uint width, CancellationToken cancellationToken)
        {
            string address = string.Format(addressFormat, height, width);
            return await textureLoader.Load(address, cancellationToken);
        }
    }
}