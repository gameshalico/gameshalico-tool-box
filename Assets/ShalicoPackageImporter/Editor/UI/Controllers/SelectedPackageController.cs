using System;
using ShalicoPackageImporter.Editor.Model;
using UnityEngine.UIElements;

namespace ShalicoPackageImporter.Editor.UI.Controllers
{
    public class SelectedPackageController
    {
        private Button _importButton;
        private TextField _memoTextField;
        private TextField _nameTextField;
        private TextField _pathTextField;
        private Button _removeButton;
        public Action OnImportButtonClicked;
        public Action<string> OnMemoChanged;
        public Action<string> OnNameChanged;
        public Action<string> OnPathChanged;
        public Action OnRemoveButtonClicked;


        public void Initialize(VisualElement root)
        {
            _nameTextField = root.Q<TextField>("selected-package__name-field");
            _pathTextField = root.Q<TextField>("selected-package__path-field");
            _memoTextField = root.Q<TextField>("selected-package__memo-field");
            _importButton = root.Q<Button>("selected-package__import-button");
            _removeButton = root.Q<Button>("selected-package__remove-button");

            _nameTextField.RegisterValueChangedCallback(evt => { OnNameChanged?.Invoke(evt.newValue); });
            _pathTextField.RegisterValueChangedCallback(evt => { OnPathChanged?.Invoke(evt.newValue); });
            _memoTextField.RegisterValueChangedCallback(evt => { OnMemoChanged?.Invoke(evt.newValue); });

            _importButton.clicked += () => { OnImportButtonClicked?.Invoke(); };
            _removeButton.clicked += () => { OnRemoveButtonClicked?.Invoke(); };
        }

        public void SetPackageData(PackageData packageData)
        {
            var isSelected = packageData != null;

            _nameTextField.SetEnabled(isSelected);
            _pathTextField.SetEnabled(isSelected);
            _memoTextField.SetEnabled(isSelected);
            _importButton.SetEnabled(isSelected);
            _removeButton.SetEnabled(isSelected);

            if (!isSelected)
            {
                _nameTextField.SetValueWithoutNotify(string.Empty);
                _pathTextField.SetValueWithoutNotify(string.Empty);
                _memoTextField.SetValueWithoutNotify(string.Empty);
                return;
            }

            _nameTextField.SetValueWithoutNotify(packageData.name);
            _pathTextField.SetValueWithoutNotify(packageData.path);
            _memoTextField.SetValueWithoutNotify(packageData.memo);
        }
    }
}