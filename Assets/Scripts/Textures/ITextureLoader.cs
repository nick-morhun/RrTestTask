using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace RrTestTask
{
    public interface ITextureLoader
    {
        /// <exception cref="TextureLoadException"></exception>
        Task<Texture2D> Load([NotNull] string address, CancellationToken cancellationToken);
    }
}