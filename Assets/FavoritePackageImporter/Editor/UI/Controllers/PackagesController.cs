using System;
using FavoritePackageImporter.Editor.Model;
using UnityEngine.UIElements;

namespace FavoritePackageImporter.Editor.UI.Controllers
{
    public class PackagesController
    {
        private Toggle _checkAllToggle;
        private Button _importAllButton;
        private ListView _listView;
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
            _listView.selectionChanged += selection =>
            {
                var selected = (PackageData)_listView.selectedItem;
                OnSelectedPackageChanged?.Invoke(selected);
            };

            var addButton = root.Q<Button>("packages__add-button");
            addButton.clicked += () => { OnAddButtonClicked?.Invoke(); };

            _checkAllToggle = root.Q<Toggle>("packages__check-all-toggle");
            _checkAllToggle.RegisterValueChangedCallback(evt =>
            {
                OnCheckAllToggleValueChanged?.Invoke(evt.newValue);
            });

            _importAllButton = root.Q<Button>("packages__import-all-button");
            _importAllButton.clicked += () => { OnImportAllButtonClicked?.Invoke(); };
        }

        public void ApplyPackageDataArray(PackageData[] packageDataArray, bool checkAllToggleValue)
        {
            _packageDataArray = packageDataArray;
            _listView.itemsSource = _packageDataArray;
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