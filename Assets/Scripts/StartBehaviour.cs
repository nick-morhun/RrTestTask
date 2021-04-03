using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RrTestTask
{
    public class StartBehaviour : MonoBehaviour
    {
        [SerializeField] private int textureRequestsTimeoutSec = 5;
        [SerializeField] private uint cardIconHeight = 100;
        [SerializeField] private uint cardIconWidth = 100;
        [SerializeField] private int minCardsCount = 4;
        [SerializeField] private int maxCardsCount = 6;

        private List<Texture2D> loadedTextures = new List<Texture2D>();

        private async void Start()
        {
            int cardsCount = Random.Range(minCardsCount, maxCardsCount + 1);
            var textureLoader = new WebTextureLoader(textureRequestsTimeoutSec);
            var randomTextureLoader = new RandomTextureLoader(textureLoader, "https://picsum.photos/{1}/{0}");
            IEnumerable<Texture2D> textures = await LoadTextures(randomTextureLoader, cardsCount, CancellationToken.None);
            loadedTextures.AddRange(textures);
        }

        private async Task<IEnumerable<Texture2D>> LoadTextures(RandomTextureLoader randomTextureLoader, int count, CancellationToken cancellationToken)
        {
            var textureTasks = new List<Task<Texture2D>>();

            for (var i = 0; i < count; i++)
            {
                Task<Texture2D> loadTask = randomTextureLoader.Load(cardIconHeight, cardIconWidth, cancellationToken);
                textureTasks.Add(loadTask);
            }

            await Task.WhenAll(textureTasks);

            return textureTasks.Where(task => !task.IsCanceled && !task.IsFaulted)
                .Select(task => task.Result)
                .ToArray();
        }
    }
}