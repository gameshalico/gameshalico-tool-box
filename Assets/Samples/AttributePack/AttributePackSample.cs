using AttributePack;
using UnityEngine;

namespace Samples.AttributePack
{
    public class AttributePackSample : MonoBehaviour
    {
        [Readonly] [SerializeField] private int readonlyInt;

        [SerializeField] private bool isEnable;

        [EnableIf(nameof(isEnable))] [SerializeField]
        private int enableIfInt;

        [DisableIf(nameof(isEnable))] [SerializeField]
        private int disableIfInt;

        [SerializeField] private bool isShow;

        [ShowIf(nameof(isShow))] [SerializeField]
        private int showIfInt;

        [HideIf(nameof(isShow))] [SerializeField]
        private int hideIfInt;

        [Preview] [SerializeField] private Texture2D previewTexture;
    }
}