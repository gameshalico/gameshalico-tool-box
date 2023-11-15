using ShalicoAttributePack;
using UnityEngine;

namespace Samples.AttributePack
{
    public class AttributePackSample : MonoBehaviour
    {
        [Readonly] [SerializeField] private int readonlyInt;

        [SerializeField] private bool isEnable;

        [EnableIf(nameof(isEnable), true)] [SerializeField]
        private int enableIfInt;

        [EnableIf(nameof(isEnable), false)] [SerializeField]
        private int disableIfInt;

        [SerializeField] private bool isShow;

        [ShowIf(nameof(isShow), true)] [SerializeField]
        private int showIfInt;

        [ShowIf(nameof(isShow), false)] [SerializeField]
        private int hideIfInt;

        [Preview] [SerializeField] private Texture2D previewTexture;

        [SubclassSelector] [SerializeReference]
        private IEntity[] entities;


        [SubclassSelector] [SerializeReference]
        private IEntity entity = new Slime();
    }
}