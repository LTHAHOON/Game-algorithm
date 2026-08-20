using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace KoiAI.Core
{
    [Serializable]
    public abstract class SaveDTO { }

    public interface ISaveDTOHandler
    {
        public void SetFromSaveDTO(SaveDTO saveDTO);
        public void SetToSaveDTO();
        public SaveDTO GetSaveDTO();  
    }
    
    [Serializable]
    public class PlayerDTO : SaveDTO
    {
        [JsonProperty]
        private ColorSaveData _curColorFace;
        [JsonProperty]
        private ColorSaveData _curColorBody;

        [JsonProperty]
        private List<Guid> _wearingCostumeGUIDs;

        public PlayerDTO(Color curColorFace, Color curColorBody, List<Guid> wearingCostumeGUIDs)
        {
            _curColorFace = new(curColorFace);
            _curColorBody = new(curColorBody);
            _wearingCostumeGUIDs = wearingCostumeGUIDs;
        }

        public void SetPlayerDTO(Color curColor_Face, Color curColor_Body, List<Guid> wearingCostumeGUIDs)
        {
            _curColorFace = new(curColor_Face);
            _curColorBody = new(curColor_Body);
            _wearingCostumeGUIDs = wearingCostumeGUIDs;
        }
        
        public Color CurColor_Face => _curColorFace.ToColor();
        public Color CurColor_Body => _curColorBody.ToColor();
        public List<Guid> WearingCostumeGUIDs => _wearingCostumeGUIDs;
    } 
    
    [Serializable]
    public struct ColorSaveData
    {
        public float R;
        public float G;
        public float B;
        public float A;

        public ColorSaveData(Color color)
        {
            R = color.r;
            G = color.g;
            B = color.b;
            A = color.a;
        }

        public Color ToColor()
        {
            return new Color(R, G, B, A);
        }
    }
}
