using System;
using UnityEngine;

namespace Story.GraphToolkit.Runtime
{
    [Serializable]
    public struct StorySpriteSheetInfo
    {
        public Texture2D SheetToFind;
        public Sprite[] Frames;
        public float FrameRate;

        public bool IsValid() => Frames != null && Frames.Length > 0;
    }
}
