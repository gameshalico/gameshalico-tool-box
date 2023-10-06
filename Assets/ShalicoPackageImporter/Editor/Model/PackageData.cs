using System;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace FavoritePackageImporter.Editor.Model
{
    [Serializable]
    public class PackageData
    {
        public string name;
        public string path;
        public string memo;
        public bool doImport;

        public PackageData(string name, string path, string memo)
        {
            this.name = name;
            this.path = path;
            this.memo = memo;
        }

        public async Task<AddRequest> ImportAsync()
        {
            CheckPath();

            var request = Client.Add(path);

            while (!request.IsCompleted)
                await Task.Delay(100);

            return request;
        }

        public async Task<RemoveRequest> RemoveAsync()
        {
            CheckPath();

            var request = Client.Remove(path);

            while (!request.IsCompleted)
                await Task.Delay(100);

            return request;
        }

        private void CheckPath()
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path is null or empty: " + name);
        }
    }
}