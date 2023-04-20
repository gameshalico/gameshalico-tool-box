using System.Diagnostics;
using System.IO;
using Shalico.ToolBox.Editor;
using UnityEditor;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    public class ProjectSettingsWatcher : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] _, string[] _0, string[] _1)
        {
            foreach (string asset in importedAssets)
            {
                if (asset.StartsWith("ProjectSettings/"))
                {
                    OnProjectSettingsChanged();
                    break;
                }
            }
        }

        private static void OnProjectSettingsChanged()
        {
            ScriptBatchCreator.GenerateAll();
        }
    }
}