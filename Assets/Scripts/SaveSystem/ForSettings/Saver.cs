using System;
using System.IO;
using UnityEngine;

namespace DC_ARPG
{
    [Serializable]
    public class Saver<T>
    {
        public T Data;

        public static void TryLoad(string filename, ref T data)
        {
            var path = FileHandler.Path(filename);
            if (File.Exists(path))
            {
                //Debug.Log($"Loading save from file");
                //Debug.Log($"Loading from {path}");
                var dataString = File.ReadAllText(path);
                var saver = JsonUtility.FromJson<Saver<T>>(dataString);
                data = saver.Data;
            }
            else
            {
                //Debug.Log($"Save file not found");
                //Debug.Log($"No file at {path}");
            }
        }

        public static void Save(string filename, T data)
        {
            var wrapper = new Saver<T> { Data = data };
            var dataString = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FileHandler.Path(filename), dataString);
        }
    }

    public static class FileHandler
    {
        public static event Action<string> EventOnReset;

        public static string Path(string filename)
        {
            return $"{Application.persistentDataPath}/{filename}";
        }

        public static void Reset(string filename)
        {
            var path = Path(filename);
            if (File.Exists(path)) File.Delete(path);

            EventOnReset?.Invoke(filename);
        }

        public static bool TryGetFile(string filename)
        {
            return File.Exists(Path(filename));
        }
    }
}