using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace AutoImporterForAseprite
{
    [ScriptedImporter(1, new[] {"ase", "aseprite"})]
    public class AsepriteAutoImporter : ScriptedImporter
    {
        public ImportOptions importOptions;
        public SpriteImportOptions spriteImportOptions;
        public TextureCreationOptions textureCreationOptions;

        [HideInInspector] public List<NamedAnimationOption> animationOptions;
        [HideInInspector] public AseSheetData sheetData;

        private static string _tempFolder;


        public override void OnImportAsset(AssetImportContext ctx)
        {
            try
            {
                var aseWorker = new AseImportContextWorkerPro(ctx);

                //Check if this is a ase file. If it's not, we have no business here
                if (!aseWorker.ContextFileIsAseFile())
                    return;

                var ase = new AsepriteCli(AsepriteAutoImporterSettings.GetSettings().pathToAsepriteExecutable,
                    importOptions);

                var exportDir = GetTempFolder();

                var sheetFile = $"{exportDir}/{aseWorker.AseFileNoExt}_sheet.png";
                var sheetDataFile = $"{exportDir}/{aseWorker.AseFileNoExt}_data.json";

                ase.ExportSpriteSheet(aseWorker.AseFile, sheetFile, sheetDataFile);

                aseWorker.TextureCreationOptions = textureCreationOptions;
                aseWorker.SpriteImportOptions = spriteImportOptions;

                aseWorker.AddMainTextureToContext(sheetFile);

                sheetData = AseSheetData.Create(sheetDataFile, aseWorker.AseFile);

                RefreshAnimationOptions();

                aseWorker.AddIndividualSpritesToContext(sheetData);
                aseWorker.AddAnimationsToContext(sheetData,
                    animationOptions.ToDictionary(e => e.tagName, e => e.animationOptions));

                Directory.Delete(exportDir, true);
                AssetDatabase.SaveAssets();
            }
            catch (Exception e)
            {
                ctx.LogImportError($"AsepriteAutoImporter: Error while importing file '{ctx.assetPath}'. {e.Message}");
            }
        }


        private void RefreshAnimationOptions()
        {
            if (sheetData.meta.frameTags == null) return;
            var sheetDataFrameTags = new HashSet<string>();
            if (animationOptions == null)
                animationOptions = new List<NamedAnimationOption>();

            foreach (var metaFrameTag in sheetData.meta.frameTags)
            {
                sheetDataFrameTags.Add(metaFrameTag.name);
                var options = animationOptions.FirstOrDefault(a => a.tagName == metaFrameTag.name);
                if (options == null)
                {
                    options = new NamedAnimationOption
                    {
                        animationOptions = AseImportContextWorker.GetDefaultAnimationOptions,
                        tagName = metaFrameTag.name
                    };
                    animationOptions.Add(options);
                }

                if (options.animationOptions.overrideDirection == false)
                {
                    if (metaFrameTag.direction == FrameTag.DirForward)
                        options.animationOptions.direction = Direction.Forward;
                    else if (metaFrameTag.direction == FrameTag.DirReverse)
                    {
                        options.animationOptions.direction = Direction.Reverse;
                    }
                    else if (metaFrameTag.direction == FrameTag.DirPingPong)
                    {
                        options.animationOptions.direction = Direction.PingPong;
                    }
                }
            }


            var toRemove = animationOptions.Where(key => sheetDataFrameTags.Contains(key.tagName) == false).ToList();
            foreach (var key in toRemove)
                animationOptions.Remove(key);
        }


        private static string GetTempFolder()
        {
            var dir = Path.Combine(AseUtils.TempFolder, "asetempexports").Replace("\\", "/");
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            return dir;
        }
    }

    [Serializable]
    public class NamedAnimationOption
    {
        public string tagName;
        public AnimationOptions animationOptions;
    }
}