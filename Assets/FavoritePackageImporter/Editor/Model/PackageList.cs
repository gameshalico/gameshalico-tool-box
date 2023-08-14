using System;
using System.Collections.Generic;
using UnityEngine;

namespace FavoritePackageImporter.Editor.Model
{
    [Serializable]
    public class PackageList
    {
        [SerializeField] private List<PackageData> packageDataList;

        public PackageList()
        {
            packageDataList = new List<PackageData>();
        }

        public IList<PackageData> PackageDataList => packageDataList;

        public PackageData[] PackageDataArray => packageDataList.ToArray();

        public void Add(PackageData packageData)
        {
            packageDataList.Add(packageData);
        }

        public void Remove(PackageData packageData)
        {
            packageDataList.Remove(packageData);
        }
    }
}