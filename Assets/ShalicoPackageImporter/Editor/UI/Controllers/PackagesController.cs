using System;
using ShalicoPackageImporter.Editor.Model;
using UnityEngine.UIElements;

namespace ShalicoPackageImporter.Editor.UI.Controllers
{
    public class PackagesController
    {
        private Toggle _checkAllToggle;
        private Button _importAllButton;
        private ListView _listView;
        private ProgressBar _progressBar;
        private PackageData[] _packageDataArray;

        private bool[] _packageDataSelectedArray;
        public Action OnAddButtonClicked;
        public Action<bool> OnCheckAllToggleValueChanged;
        public Action<PackageData> OnDeleteButtonClicked;
        public Action OnImportAllButtonClicked;
        public Action<PackageData> OnMoveDownButtonClicked;
        public Action<PackageData> OnMoveUpButtonClicked;
        public Action<PackageData, bool> OnPackageToggleValueChanged;
        public Action<PackageData> OnSelectedPackageChanged;

        public void Initialize(VisualElement root, VisualTreeAsset entryVisualTreeAsset)
        {
            _listView = root.Q<ListView>("packages__list-view");
            _listView.selectionType = SelectionType.Single;
            _listView.makeItem = () =>
            {
                var entry = entryVisualTreeAsset.Instantiate();
                var entryController = new PackagesEntryController();
                entryController.SetVisualElement(entry);

                entry.userData = entryController;

                return entry;
            };
            _listView.bindItem = (element, index) =>
            {
                var entryController = (PackagesEntryController)element.userData;
                entryController.SetData(_packageDataArray[index]);

                entryController.OnDeleteButtonClicked = () =>
                {
                    OnDeleteButtonClicked?.Invoke(_packageDataArray[index]);
                };
                entryController.OnCheckToggleValueChanged = value =>
                {
                    OnPackageToggleValueChanged?.Invoke(_packageDataArray[index], value);
                };
                entryController.OnMoveUpButtonClicked = () =>
                {
                    OnMoveUpButtonClicked?.Invoke(_packageDataArray[index]);
                };
                entryController.OnMoveDownButtonClicked = () =>
                {
                    OnMoveDownButtonClicked?.Invoke(_packageDataArray[index]);
                };
            };

# if UNITY_2022_3_OR_NEWER
            _listView.selectionChanged += selection =>
            {
                var selected = (PackageData)_listView.selectedItem;
                OnSelectedPackageChanged?.Invoke(selected);
            };
# else
            _listView.onSelectionChange += selection =>
            {
                var selected = (PackageData)_listView.selectedItem;
                OnSelectedPackageChanged?.Invoke(selected);
            };
# endif

            var addButton = root.Q<Button>("packages__add-button");
            addButton.clicked += () => { OnAddButtonClicked?.Invoke(); };

            _checkAllToggle = root.Q<Toggle>("packages__check-all-toggle");
            _checkAllToggle.RegisterValueChangedCallback(evt =>
            {
                OnCheckAllToggleValueChanged?.Invoke(evt.newValue);
            });

            _importAllButton = root.Q<Button>("packages__import-all-button");
            _importAllButton.clicked += () => { OnImportAllButtonClicked?.Invoke(); };

            _progressBar = root.Q<ProgressBar>("packages__import-progress-bar");
            _progressBar.value = 0;
            _progressBar.title = "";
            _progressBar.SetEnabled(false);
        }

        public void CompleteProgressBar()
        {
            _progressBar.value = 1;
            _progressBar.highValue = 1;
            _progressBar.title = "Import Complete";
        }

        public void BeginImportAll(int count)
        {
            _progressBar.SetEnabled(true);
            _importAllButton.SetEnabled(false);
            _progressBar.title = $"Importing 0/{count}";
        }

        public void EndImportAll()
        {
            _progressBar.SetEnabled(false);
            _importAllButton.SetEnabled(true);
            _progressBar.title = "";
        }

        public void ApplyProgress(PackageData data, int progress, int max)
        {
            _progressBar.value = progress;
            _progressBar.highValue = max;

            _progressBar.title = $"Importing {data.name} {progress}/{max}";
        }

        public void ApplyPackageDataArray(PackageData[] packageDataArray, PackageData selectedItem,
            bool checkAllToggleValue)
        {
            _packageDataArray = packageDataArray;
            _listView.itemsSource = _packageDataArray;
            _listView.SetSelection(Array.IndexOf(_packageDataArray, selectedItem));
            _listView.RefreshItems();


            _checkAllToggle.SetValueWithoutNotify(checkAllToggleValue);

            var checkedCount = 0;
            foreach (var packageData in _packageDataArray)
                if (packageData.doImport)
                    checkedCount++;
            _importAllButton.SetEnabled(checkedCount > 0);
            _importAllButton.text = $"Import All Checked Packages ({checkedCount})";
        }
    }
}