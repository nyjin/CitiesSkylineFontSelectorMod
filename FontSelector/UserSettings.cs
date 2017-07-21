using System;
using System.IO;
using ColossalFramework.UI;
using ICities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FontSelector
{
    public class UserSettings
    {
        public FontConfig Config { get; private set; }

        private FontConfig CreateDefaultConfig(string path)
        {
            var components = Object.FindObjectsOfType<UITextComponent>();

            var textComponent = components[0];
            var font = textComponent.font as UIDynamicFont;

            Debug.Log($"Default Font: {font.baseFont.fontNames[0]} Size: {font.size}");

            return new FontConfig(path)
            {
                Family = font.baseFont.fontNames[0],
                Size = font.size
            };
        }

        public void Populate(UIHelperBase helper, string path)
        {
            if (!File.Exists(path)) Config = CreateDefaultConfig(path);
            else
            {
                Config = FontConfig.Load(path);
                Apply();
            }

            var group = helper.AddGroup("FontSelector");
            group.AddDropdown("Font Family", FontConfig.Families, Config.GetFamilyIndex(), sel => { Config.SetFamilyByIndex(sel); });
            group.AddDropdown("Font Size", FontConfig.Sizes, Config.GetSizeIndex(), sel => { Config.SetSizeByIndex(sel); });
            group.AddButton("Apply", Confirm);
        }


        private void Apply()
        {
            var selectedFontFamilies = new[] {Config.Family};
            var selectedFontSize = Config.Size;

            var textComponents = Object.FindObjectsOfType<UITextComponent>();
            foreach (var c in textComponents)
            {
                var font = c.font as UIDynamicFont;
                if (font.size == selectedFontSize && font.baseFont != null && font.baseFont.fontNames.Length > 0 &&
                    string.Compare(font.baseFont.fontNames[0], Config.Family, StringComparison.OrdinalIgnoreCase) == 0)
                    continue;
                font.size = selectedFontSize;
                font.baseFont.fontNames = selectedFontFamilies;
            }
            UIView.RefreshAll();
        }


        private void Confirm()
        {
            Apply();
            Config.Save();
        }
    }
}