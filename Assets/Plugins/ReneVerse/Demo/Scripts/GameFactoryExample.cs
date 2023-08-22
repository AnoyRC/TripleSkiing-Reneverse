using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using Rene.Sdk.Api.Game.Data;
using UnityEngine;

namespace ReneVerse.Demo
{
    public class GameFactoryExample : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private ReneAsset reneAssetPrefab;
        private static readonly List<ReneAsset> _reneAssets = new List<ReneAsset>();
        public static GameFactoryExample Instance { get; private set; }


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public async Task<ReneAsset> CreateReneAssetUI(AssetsResponse.AssetsData.Asset asset, Transform parentToInstantiate = null)
        {
            var reneAsset = Instantiate(reneAssetPrefab, parentToInstantiate);
            _reneAssets.Add(reneAsset);
            string assetDescription = "";

            foreach (var assetAttribute in asset.Metadata.Attributes)
            {
                assetDescription += $"The attribute {assetAttribute.TraitType} is {assetAttribute.Value}\n";
            }

            await reneAsset.SetProperties(assetDescription, asset.Metadata.Image, this);

            return reneAsset;
        }

        public static void DestroyInstantiatedReneAssets()
        {
            _reneAssets.ForEach(reneAsset => Destroy(reneAsset.gameObject));
            _reneAssets.Clear();
        }
         
    }
}