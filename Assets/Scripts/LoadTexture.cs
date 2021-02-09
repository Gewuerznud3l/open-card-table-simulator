using UnityEngine;
using System.Collections;
using System.IO;

public class IMG2Sprite
{

    // This script loads a PNG or JPEG image from disk and returns it as a Sprite
    // Drop it on any GameObject/Camera in your scene (singleton implementation)
    //
    // Usage from any other script:
    // MySprite = IMG2Sprite.instance.LoadNewSprite(FilePath, [PixelsPerUnit (optional)])

    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference


        //Texture2D SpriteTexture = LoadTexture(FilePath);
        byte[] data = File.ReadAllBytes(FilePath);
        Texture2D SpriteTexture = new Texture2D(966, 1500, TextureFormat.ARGB32, false);
        SpriteTexture.LoadImage(data);
        SpriteTexture.name = Path.GetFileNameWithoutExtension(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, 966, 1500/*SpriteTexture.width, SpriteTexture.height*/),
            new Vector2(/*SpriteTexture.width / 2, SpriteTexture.height / 2*/ 0.5f, 0.5f), PixelsPerUnit);

        return NewSprite;
    }

    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
