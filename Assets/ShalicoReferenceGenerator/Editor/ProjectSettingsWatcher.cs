using UnityEditor;

namespace ShalicoReferenceGenerator.Editor
{
    public class ProjectSettingsWatcher : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] _, string[] _0, string[] _1)
        {
            foreach (var asset in importedAssets)
                if (asset.StartsWith("ProjectSettings/"))
                {
                    OnProjectSettingsChanged();
                    break;
                }
        }

        private static void OnProjectSettingsChanged()
        {
            ScriptBatchCreator.GenerateAll();
        }
    }
}