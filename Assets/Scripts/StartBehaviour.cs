using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
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
        [SerializeField] private uint pixelsPerUnit = 100;
        [SerializeField] private CardView cardViewPrefab;
        [SerializeField] private CardSettings cardSettings;
        [SerializeField] private PlayerHandView playerHandView;
        [SerializeField] private StatRandomizerView statRandomizerView;
        [SerializeField] private DropTarget dropTarget;
        private PlayerHand hand;
        private IStatRandomizer statRandomizer;

        private async void Start()
        {
            Assert.IsNotNull(cardViewPrefab);
            Assert.IsNotNull(playerHandView);
            Assert.IsNotNull(statRandomizerView);
            Assert.IsNotNull(dropTarget);

            int cardsCount = Random.Range(minCardsCount, maxCardsCount + 1);
            var textureLoader = new WebTextureLoader(textureRequestsTimeoutSec);
            var randomTextureLoader = new RandomTextureLoader(textureLoader, "https://picsum.photos/{1}/{0}");
            IEnumerable<Texture2D> textures = await LoadTextures(randomTextureLoader, cardsCount, CancellationToken.None);
            var cardSpriteFactory = new CardSpriteFactory(cardIconHeight, cardIconWidth, pixelsPerUnit);
            IReadOnlyList<Sprite> icons = textures.Select(cardSpriteFactory.Create).ToList();
            var cardGenerator = new CardGenerator(cardSettings);
            IEnumerable<Card> cards = cardGenerator.Create(icons.Count);
            var cardViewFactory = new CardViewFactory(cardViewPrefab, icons);
            hand = new PlayerHand(cards, cardSettings);
            playerHandView.SetModel(hand.Cards, cardViewFactory);

            statRandomizer = new UnityStatRandomizer(cardSettings, hand.Cards);
            statRandomizerView.SetModel(statRandomizer);
            dropTarget.SetCardViews(playerHandView.CardViews);
        }

        private void OnDestroy()
        {
            hand?.Dispose();
            statRandomizer?.Dispose();
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