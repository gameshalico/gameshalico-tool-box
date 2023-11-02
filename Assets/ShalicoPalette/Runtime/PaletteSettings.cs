using UnityEditor;
using UnityEngine;

namespace ShalicoPalette
{
    [CreateAssetMenu(fileName = "PaletteSettings", menuName = "ShalicoPalette/PaletteSettings", order = 0)]
    public class PaletteSettings : ScriptableObject
    {
        private static PaletteSettings s_instance;

        [SerializeField] private PaletteBase defaultPalette;

        public static PaletteSettings Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = Resources.Load<PaletteSettings>(nameof(PaletteSettings));
                    if (s_instance)
                        return s_instance;

                    s_instance = CreateInstance<PaletteSettings>();

#if UNITY_EDITOR
                    if (!EditorApplication.isPlaying)
                    {
                        AssetDatabase.CreateAsset(s_instance,
                            "Assets/Resources/" + nameof(PaletteSettings) + ".asset");
                        Debug.Log("Created PaletteSettings asset at Assets/Resources/" + nameof(PaletteSettings) +
                                  ".asset");
                        AssetDatabase.SaveAssets();
                    }
#endif
                }

                return s_instance;
            }
        }

        public static PaletteBase DefaultPalette => Instance.defaultPalette;
    }
}