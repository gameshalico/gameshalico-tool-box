using System;
using FavoritePackageImporter.Editor.Model;
using UnityEngine.UIElements;

namespace FavoritePackageImporter.Editor.UI.Controllers
{
    public class PackagesEntryController
    {
        private Toggle _checkToggle;
        private Button _deleteButton;
        private Button _moveDownButton;
        private Button _moveUpButton;
        private Label _nameLabel;

        public Action<bool> OnCheckToggleValueChanged;
        public Action OnDeleteButtonClicked;
        public Action OnMoveDownButtonClicked;
        public Action OnMoveUpButtonClicked;

        public void SetVisualElement(VisualElement root)
        {
            _nameLabel = root.Q<Label>("packages-entry__name-label");
            _deleteButton = root.Q<Button>("packages-entry__delete-button");
            _deleteButton.clicked += () => { OnDeleteButtonClicked?.Invoke(); };
            _moveUpButton = root.Q<Button>("packages-entry__up-button");
            _moveUpButton.clicked += () => { OnMoveUpButtonClicked?.Invoke(); };
            _moveDownButton = root.Q<Button>("packages-entry__down-button");
            _moveDownButton.clicked += () => { OnMoveDownButtonClicked?.Invoke(); };

            _checkToggle = root.Q<Toggle>("packages-entry__check-toggle");
            _checkToggle.RegisterValueChangedCallback(evt => { OnCheckToggleValueChanged?.Invoke(evt.newValue); });
        }

        public void SetData(PackageData packageData)
        {
            _nameLabel.text = packageData.name;

            _checkToggle.SetValueWithoutNotify(packageData.doImport);
        }
    }
}