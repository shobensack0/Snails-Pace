﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AutoImporterForAseprite
{
    public class AseImportContextWorker
    {
        public static AnimationOptions GetDefaultAnimationOptions => new AnimationOptions
        {
            useCustomCurve = false,
            customCurve = null,
            componentType = ComponentType.SpriteRenderer,
            relativePath = "",
            direction = Direction.Forward,
            loopTime = false,
            overrideDirection = false
        };

        private AssetImportContext ctx;
        public AssetImportContext Context => ctx;

        public string AseFile => ctx.assetPath;
        public string AseFileNoExt => Path.GetFileNameWithoutExtension(AseFile);

        public Texture2D MainTexture { get; private set; }
        public List<Sprite> IndividualSprites { get; private set; }

        public SpriteImportOptions SpriteImportOptions;
        public TextureCreationOptions TextureCreationOptions;


        public AseImportContextWorker(AssetImportContext ctx)
        {
            this.ctx = ctx;
        }

        public bool ContextFileIsAseFile()
        {
            return AseUtils.IsAseFile(ctx.assetPath);
        }


        /// <summary>
        /// Generates a 2D texture from an image file
        /// </summary>
        /// <param name="imagePath">The path to the png or jpg file</param>
        /// <param name="name">The name of the texture object. Defaults to the ase file name + Texture</param>
        /// <returns></returns>
        public Texture2D GenerateTexture(string imagePath, string name = null)
        {
            if (TextureCreationOptions == null)
                TextureCreationOptions = TextureCreationOptions.Default;

            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);

            if (!texture.LoadImage(File.ReadAllBytes(imagePath)))
                ctx.LogImportWarning("Texture loading not successful");

            texture.wrapMode = TextureCreationOptions.textureWrapMode;
            texture.filterMode = TextureCreationOptions.filterMode;
            texture.alphaIsTransparency = TextureCreationOptions.alphaIsTransparency;
            texture.name = string.IsNullOrWhiteSpace(name) ? AseFileNoExt + "Texture" : name;
            return texture;
        }

        public void GenerateMainTextureFromSheetFile(string sheetFile)
        {
            if (MainTexture)
                Object.DestroyImmediate(MainTexture);
            MainTexture = GenerateTexture(sheetFile, "sheet_text");
        }

        public void GenerateIndividualSprites(AseSheetData sheetData, bool addToContext = false)
        {
            if (SpriteImportOptions == null)
                SpriteImportOptions = SpriteImportOptions.Default;

            if (MainTexture == null)
                GenerateMainTextureFromSheetFile(Path.Combine(
                    Path.GetDirectoryName(sheetData.pathToSheetData) ??
                    throw new InvalidOperationException("Error while processing data path"),
                    sheetData.meta.image));

            if (IndividualSprites != null)
            {
                foreach (var individualSprite in IndividualSprites)
                    Object.DestroyImmediate(individualSprite);
                IndividualSprites.Clear();
            }
            else
            {
                IndividualSprites = new List<Sprite>();
            }

            var addedObjects = new Dictionary<string, int>();

            var shouldExportSlices = sheetData.meta.slices != null && sheetData.meta.slices.Length > 0 &&
                                     (sheetData.meta.frameTags == null || sheetData.meta.frameTags.Length == 0);

            if (Config.IsLiteVersion == false && sheetData.meta.slices != null && sheetData.meta.slices.Length > 0 &&
                (sheetData.meta.frameTags != null && sheetData.meta.frameTags.Length > 0))
            {
                Debug.LogWarning(
                    "It seems that you have enabled the 'Split slices' option and have animations. This is not well supported and is usually only for static images. No slices will be exported now");
            }


            foreach (var sprite in EnumerateSprites(sheetData, shouldExportSlices))
            {
                //Make sure we prevent import of two assets with the same name
                if (addedObjects.ContainsKey(sprite.name))
                {
                    var newCount = addedObjects[sprite.name] + 1;
                    addedObjects[sprite.name] = newCount;
                    sprite.name = sprite.name + "_" + newCount;
                }
                else
                    addedObjects.Add(sprite.name, 1);

                if (addToContext)
                    Context.AddObjectToAsset(sprite.name, sprite);

                IndividualSprites.Add(sprite);
            }
        }

        private IEnumerable<Sprite> EnumerateSprites(AseSheetData sheetData, bool shouldExportSlices)
        {
            if (shouldExportSlices)
            {
                foreach (var slice in sheetData.meta.slices)
                {
                    foreach (var sliceKeyData in slice.keys)
                    {
                        var frame = sheetData.frames[sliceKeyData.frame];
                        var rect = new Rect(frame.frame.x + sliceKeyData.bounds.x,
                            frame.frame.h - sliceKeyData.bounds.y - sliceKeyData.bounds.h, sliceKeyData.bounds.w,
                            sliceKeyData.bounds.h);

                        var sprite = Sprite.Create(MainTexture, rect,
                            SpriteImportOptions.pivot,
                            SpriteImportOptions.pixelsPerUnit, SpriteImportOptions.extrude,
                            SpriteImportOptions.meshType,
                            SpriteImportOptions.border);
                        sprite.name = frame.filename + "_" + slice.name;
                 
                        yield return sprite;
                    }
                }
            }
            else
            {
                foreach (var sheetDataFrame in sheetData.frames)
                {
                    var sprite = Sprite.Create(MainTexture, sheetDataFrame.frame.ToRect(), SpriteImportOptions.pivot,
                        SpriteImportOptions.pixelsPerUnit, SpriteImportOptions.extrude, SpriteImportOptions.meshType,
                        SpriteImportOptions.border);
                    sprite.name = sheetDataFrame.filename;
                    yield return sprite;
                }
            }
        }

        public Texture2D AddMainTextureToContext(string sheetFile)
        {
            GenerateMainTextureFromSheetFile(sheetFile);
            var objects = new List<Object>();
            Context.GetObjects(objects);
            var found = objects.FirstOrDefault(o => o.name == "AseSheetMainTexture");
            if (found)
                Object.DestroyImmediate(found);
            Context.AddObjectToAsset("AseSheetMainTexture", MainTexture, AseUtils.GetPixelArtIcon());
            Context.SetMainObject(MainTexture);
            return MainTexture;
        }

        public List<Sprite> AddIndividualSpritesToContext(AseSheetData sheetData)
        {
            GenerateIndividualSprites(sheetData, true);
            return IndividualSprites;
        }

        public void AddAnimationsToContext(AseSheetData aseSheetData,
            Dictionary<string, AnimationOptions> animationOptionsByTagName = null
        )
        {
            if (aseSheetData.meta.frameTags == null || aseSheetData.meta.frameTags.Length == 0) return;
            if (IndividualSprites == null)
                GenerateIndividualSprites(aseSheetData, true);

            foreach (var metaFrameTag in aseSheetData.meta.frameTags)
            {
                GenerateAnimation(metaFrameTag, aseSheetData.frames, IndividualSprites,
                    animationOptionsByTagName != null && animationOptionsByTagName.ContainsKey(metaFrameTag.name)
                        ? animationOptionsByTagName[metaFrameTag.name]
                        : GetDefaultAnimationOptions);
            }
        }


        private static bool displayedWarning;

        public virtual void GenerateAnimation(FrameTag frameTag, IList<FrameElement> allFrames,
            IList<Sprite> allSprites, AnimationOptions options)
        {
            if (!displayedWarning)
            {
                Debug.LogWarning(
                    "It seems that you have animations in your aseprite file. If you get the PRO version of Auto Importer For Unity you will have animation importing too!");
                displayedWarning = true;
            }
        }
    }

    [Serializable]
    public class AnimationOptions
    {
        /// <summary>
        /// Path to the game object that contains the Sprite/Image component.
        /// The relativePath is formatted similar to a pathname, e.g. "root/spine/leftArm".
        /// If relativePath is empty it refers to the game object the animation clip is attached to. 
        /// </summary>
        public string relativePath;

        /// <summary>
        /// Weather the target component is an Image or a Sprite Renderer
        /// </summary>
        public ComponentType componentType;

        /// <summary>
        /// Set this to true in order to use the wrap mode and reverse direction from these options. Otherwise the settings will be implied from the animation settings in the ase file
        /// </summary>
        public bool overrideDirection;

        public Direction direction;

        public bool loopTime;

        public bool useCustomCurve;

        public AnimationCurve customCurve;
    }

    public enum ComponentType
    {
        SpriteRenderer,
        Image
    }

    public enum Direction
    {
        Forward,
        Reverse,
        PingPong
    }

    [Serializable]
    public class SpriteImportOptions
    {
        public static SpriteImportOptions Default => new SpriteImportOptions();
        public float pixelsPerUnit;
        public Vector2 pivot;
        public uint extrude;
        public SpriteMeshType meshType;
        public Vector4 border;

        public SpriteImportOptions()
        {
            pivot = new Vector2(0.5f, 0.5f);
            pixelsPerUnit = 100f;
            border = Vector4.zero;
            meshType = SpriteMeshType.Tight;
            extrude = 0U;
        }
    }

    [Serializable]
    public class TextureCreationOptions
    {
        public static TextureCreationOptions Default => new TextureCreationOptions();

        public TextureWrapMode textureWrapMode;
        public FilterMode filterMode;
        public bool alphaIsTransparency;

        public TextureCreationOptions()
        {
            textureWrapMode = TextureWrapMode.Clamp;
            filterMode = FilterMode.Point;
            alphaIsTransparency = true;
        }
    }
}