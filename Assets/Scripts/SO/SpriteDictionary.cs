using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "SO/SpriteDict_", menuName = "Data/Sprite Dictionary", order = 0)]
    public class SpriteDictionary : SerializedScriptableObject
    {
        public Dictionary<string, Sprite> sprites;
    }
}