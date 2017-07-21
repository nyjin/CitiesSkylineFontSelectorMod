using System;
using System.IO;
using System.Linq;
using ColossalFramework.IO;
using CSLMusicMod.LitJson;
using UnityEngine;

namespace FontSelector
{
    public class FontConfig
    {

        private FontItem _item;
        public class FontItem
        {
            public string Family { get; set; }
            public int Size { get; set; }
        }

        public const string ConfigFileName = "FileSelector.json";

        public static string[] Families = Font.GetOSInstalledFontNames().ToArray();

        public static string[] Sizes =
        {
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"
        };

        private string _path;

        internal FontConfig(string path, FontItem item)
        {
            _path = path;
            _item = item;
        }

        public FontConfig(string path)
        {
            _path = path;
            _item = new FontItem();
        }

        public string Family
        {
            get { return _item.Family; }
            set { _item.Family = value; }
        }

        public int Size
        {
            get { return _item.Size; }
            set { _item.Size = value; }
        }

        public int GetFamilyIndex()
        {
            var index = -1;
            for (var i = 0; i < Families.Length; i++)
                if (string.Compare(Families[i], Family, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    index = i;
                    break;
                }
            return index;
        }

        public int GetSizeIndex()
        {
            var index = -1;
            var sizeString = Size.ToString();
            for (var i = 0; i < Sizes.Length; i++)
                if (string.Compare(Sizes[i], sizeString, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    index = i;
                    break;
                }
            return index;
        }

        public void SetFamilyByIndex(int index)
        {
            Family = Families[index];
        }

        public void SetSizeByIndex(int index)
        {
            Size = int.Parse(Sizes[index]);
        }

        public void Save()
        {
            Debug.Log($"Family: {_item.Family} Size: {_item.Size}");
            try
            {
                using (var writer = new StringWriter())
                {
                    var jsonWriter = new JsonWriter(writer) {PrettyPrint = true};
                    JsonMapper.ToJson(this._item, jsonWriter);
                    File.WriteAllText(_path, writer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message}");
            }
            
        }

        public static FontConfig Load(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("FileSelector Config file not found");
                throw new FileNotFoundException("Config file not found");
            }

            var data = File.ReadAllText(path);
            var item = JsonMapper.ToObject<FontItem>(data);
            return new FontConfig(path, item);
        }
    }
}