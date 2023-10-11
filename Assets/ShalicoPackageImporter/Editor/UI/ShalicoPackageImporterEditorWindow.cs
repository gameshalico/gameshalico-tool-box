using System;
using System.Threading.Tasks;
using ShalicoPackageImporter.Editor.Model;
using ShalicoPackageImporter.Editor.UI.Controllers;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoPackageImporter.Editor.UI
{
    public class ShalicoPackageImporterEditorWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset visualTreeAsset;
        [SerializeField] private VisualTreeAsset packagesEntryVisualTreeAsset;

        private Task _importOrRemoveTask;

        private PackageRegistry _packageRegistry;
        private PackagesController _packagesController;
        private SelectedPackageController _selectedPackageController;

        public void CreateGUI()
        {
            var root = rootVisualElement;

            VisualElement visualTree = visualTreeAsset.Instantiate();
            root.Add(visualTree);

            _packageRegistry = new PackageRegistry();
            _packageRegistry.Initialize();

            InitializePackagesController(root);
            InitializeSelectedPackageController(root);
            ApplyPackageDataArray();
            ApplySelectedPackageData();
        }


        private void InitializePackagesController(VisualElement root)
        {
            _packagesController = new PackagesController();
            _packagesController.Initialize(root, packagesEntryVisualTreeAsset);

            _packagesController.OnAddButtonClicked = () =>
            {
                var packageData = new PackageData(
                    "New Package",
                    "",
                    ""
                );
                _packageRegistry.Add(packageData);
                ApplyPackageDataArray();
            };

            _packagesController.OnDeleteButtonClicked = packageData =>
            {
                _packageRegistry.Remove(packageData);
                ApplyPackageDataArray();
                ApplySelectedPackageData();
            };

            _packagesController.OnPackageToggleValueChanged = (packageData, value) =>
            {
                packageData.doImport = value;
                _packageRegistry.Save();
                ApplyPackageDataArray();
            };

            _packagesController.OnCheckAllToggleValueChanged = value =>
            {
                _packageRegistry.CheckAll(value);
                ApplyPackageDataArray();
            };

            _packagesController.OnSelectedPackageChanged = packageData =>
            {
                _packageRegistry.SelectedPackageData = packageData;
                ApplySelectedPackageData();
            };

            _packagesController.OnImportAllButtonClicked = async () =>
            {
                if (_importOrRemoveTask?.IsCompleted == false)
                {
                    Debug.Log("[Shalico Package Importer] Other process is running");
                    return;
                }

                Debug.Log("[Shalico Package Importer] Importing all packages...");
                _importOrRemoveTask = ImportAllAsync();
                await _importOrRemoveTask;
                Debug.Log("[Shalico Package Importer] All packages imported.");
            };

            _packagesController.OnMoveUpButtonClicked = packageData =>
            {
                _packageRegistry.MoveUp(packageData);
                ApplyPackageDataArray();
                ApplySelectedPackageData();
            };
            _packagesController.OnMoveDownButtonClicked = packageData =>
            {
                _packageRegistry.MoveDown(packageData);
                ApplyPackageDataArray();
                ApplySelectedPackageData();
            };
        }

        private void InitializeSelectedPackageController(VisualElement root)
        {
            _selectedPackageController = new SelectedPackageController();
            _selectedPackageController.Initialize(root);

            _selectedPackageController.OnNameChanged = packageName =>
            {
                _packageRegistry.SelectedPackageData.name = packageName;
                _packageRegistry.Save();
                ApplyPackageDataArray();
                ApplySelectedPackageData();
            };

            _selectedPackageController.OnPathChanged = path =>
            {
                _packageRegistry.SelectedPackageData.path = path;
                _packageRegistry.Save();
                ApplySelectedPackageData();
            };

            _selectedPackageController.OnMemoChanged = memo =>
            {
                _packageRegistry.SelectedPackageData.memo = memo;
                _packageRegistry.Save();
                ApplySelectedPackageData();
            };

            _selectedPackageController.OnImportButtonClicked = () =>
            {
                if (_importOrRemoveTask?.IsCompleted == false)
                {
                    Debug.Log("[Shalico Package Importer] Other process is running");
                    return;
                }

                _importOrRemoveTask = ImportPackageAsync(_packageRegistry.SelectedPackageData);
            };

            _selectedPackageController.OnRemoveButtonClicked = () =>
            {
                if (_importOrRemoveTask?.IsCompleted == false)
                {
                    Debug.Log("[Shalico Package Importer] Other process is running");
                    return;
                }

                _importOrRemoveTask = RemovePackageAsync(_packageRegistry.SelectedPackageData);
            };
        }

        private async Task ImportPackageAsync(PackageData packageData)
        {
            Debug.Log($"[Shalico Package Importer] Importing package: {packageData.name}");
            try
            {
                var request = await packageData.ImportAsync();

                if (request.Status == StatusCode.Success)
                    Debug.Log($"[Shalico Package Importer] Package imported: {packageData.name}");
                else
                    Debug.LogError($"[Shalico Package Importer] Failed to import package: {packageData.name}");
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}");
            }
        }

        private async Task RemovePackageAsync(PackageData packageData)
        {
            Debug.Log($"[Shalico Package Importer] Removing package: {packageData.name}");
            try
            {
                var request = await packageData.RemoveAsync();
                if (request.Status == StatusCode.Success)
                    Debug.Log($"[Shalico Package Importer] Remove package data: {packageData.name}");
                else
                    Debug.LogWarning($"[Shalico Package Importer] Failed to remove package data: {packageData.name}");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[Shalico Package Importer] {e.Message}");
            }
        }

        private async Task ImportAllAsync()
        {
            var count = 0;
            var max = _packageRegistry.CountDoImport();
            _packagesController.BeginImportAll(max);
            foreach (var packageData in _packageRegistry.PackageDataArray)
            {
                if (!packageData.doImport) continue;
                _packagesController.ApplyProgress(packageData, count, max);
                await ImportPackageAsync(packageData);
                count++;
            }

            _packagesController.CompleteProgressBar();
            await Task.Delay(200);
            _packagesController.EndImportAll();
        }

        private void ApplyPackageDataArray()
        {
            var isCheckAny = _packageRegistry.IsCheckAny();
            _packagesController.ApplyPackageDataArray(_packageRegistry.PackageDataArray,
                _packageRegistry.SelectedPackageData, isCheckAny);
        }

        private void ApplySelectedPackageData()
        {
            var packageData = _packageRegistry.SelectedPackageData;
            _selectedPackageController.SetPackageData(packageData);
        }

        [MenuItem("Tools/Shalico/PackageImporter")]
        public static void Open()
        {
            var wnd = GetWindow<ShalicoPackageImporterEditorWindow>();
            wnd.titleContent = new GUIContent("Shalico Package Importer");
        }
    }
}