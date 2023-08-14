using UnityEditor;
using UnityEngine;

namespace FavoritePackageImporter.Editor.Model
{
    public class PackageRegistry
    {
        private const string PackagePrefsKey = "FavoritePackageImporter.PackageList";
        private PackageList _packageList;

        public PackageData[] PackageDataArray => _packageList.PackageDataArray;

        public PackageData SelectedPackageData { get; set; }

        private void Load()
        {
            var jsonData = EditorPrefs.GetString(PackagePrefsKey, "");
            if (!string.IsNullOrEmpty(jsonData))
                _packageList = JsonUtility.FromJson<PackageList>(jsonData);

            if (_packageList == null) _packageList = new PackageList();
        }

        public void CheckAll(bool value)
        {
            foreach (var packageData in _packageList.PackageDataList) packageData.doImport = value;
            Save();
        }

        public bool IsCheckAny()
        {
            foreach (var packageData in _packageList.PackageDataList)
                if (packageData.doImport)
                    return true;

            return false;
        }

        public void Save()
        {
            var jsonData = JsonUtility.ToJson(_packageList);
            EditorPrefs.SetString(PackagePrefsKey, jsonData);
        }

        public void Initialize()
        {
            Load();
        }

        public void Add(PackageData packageData)
        {
            _packageList.Add(packageData);
            Save();
        }

        public void Remove(PackageData packageData)
        {
            if (packageData == SelectedPackageData)
                SelectedPackageData = null;

            _packageList.Remove(packageData);
            Save();
        }

        public void MoveUp(PackageData packageData)
        {
            var index = _packageList.PackageDataList.IndexOf(packageData);
            if (index <= 0) return;

            _packageList.PackageDataList.RemoveAt(index);
            _packageList.PackageDataList.Insert(index - 1, packageData);

            Save();
        }

        public void MoveDown(PackageData packageData)
        {
            var index = _packageList.PackageDataList.IndexOf(packageData);
            if (index < 0 || index >= _packageList.PackageDataList.Count - 1) return;

            _packageList.PackageDataList.RemoveAt(index);
            _packageList.PackageDataList.Insert(index + 1, packageData);

            Save();
        }
    }
}