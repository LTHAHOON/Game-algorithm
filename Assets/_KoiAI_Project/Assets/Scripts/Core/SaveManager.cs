
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace KoiAI.Core
{
    [Serializable]
    public class SaveWrapper<T> where T : SaveDTO
    {
        [SerializeField]
        private T[] _values;

        public SaveWrapper(T[] values)
        {
            _values = values;
        }
        public T[] Values => _values;
    }
    
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private JsonSerializerSettings _settings;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                //SaveDTO가 추상 클래스이기 때문에 실제 자식 클래스를 찾아 역직렬화를 시켜줍니다.
                TypeNameHandling = TypeNameHandling.Auto,
                //Vector의 normalized 무한 반환을 무시(중단)시켜줍니다.(Vector의 normalized은 같은 Vector 타입을 반환하기 때문에 무한 루프에 걸립니다.)
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //Unity 객체 내부 프로퍼티를 깊게 탐색하지 않도록 필드 중심으로 직렬화합니다.
                ContractResolver = new DefaultContractResolver
                {
                    IgnoreSerializableAttribute = false,
                    IgnoreSerializableInterface = true
                },
                //오브젝트 그래프가 너무 깊어지는 것을 방지합니다.
                MaxDepth = 16,
                //직렬화 중 문제가 되는 멤버는 저장을 중단하고 넘어갑니다.
                Error = (_, args) =>
                {
                    Debug.LogWarning($"세이브 직렬화 중 일부 데이터가 무시되었습니다: {args.ErrorContext.Error.Message}");
                    args.ErrorContext.Handled = true;
                }
            };
        }

        /// <summary>
        /// 세이브 파일 저장하기
        /// </summary>
        public void SaveToJson<THandler>(List<THandler> objs, string savePath) where THandler : ISaveDTOHandler
        {
            if (string.IsNullOrEmpty(savePath))
            {
                return;
            }
            if (objs == null || objs.Count <= 0)
            {
                return;
            }
            
            SaveDTO[] saveDTOs = objs.Select(handler =>
            {
                handler.SetToSaveDTO();
                return handler.GetSaveDTO();
            }).ToArray();
            SaveWrapper<SaveDTO> wrapper = new SaveWrapper<SaveDTO>(saveDTOs);
            
            string serializedJson = JsonConvert.SerializeObject(wrapper, _settings);
            FileStream fileStream = new(savePath, FileMode.Create);
            using (StreamWriter writer = new(fileStream))
            {
                writer.Write(serializedJson);
            }
            Debug.Log($"세이브 파일 저장 및 생성완료: {savePath}");
        }
        
        /// <summary>
        /// 세이브 파일 불러오기
        /// </summary>
        public void LoadFromJson<THandler>(List<THandler> objs, string savePath) where THandler : ISaveDTOHandler
        {
            if (string.IsNullOrEmpty(savePath) || !File.Exists(savePath))
            {
                return;
            }
            if (objs == null || objs.Count <= 0)
            {
                return;
            }
            
            string serializedJson = ReadSaveFile(savePath);
            SaveWrapper<SaveDTO> wrapper = JsonConvert.DeserializeObject<SaveWrapper<SaveDTO>>(serializedJson, _settings);
            if (wrapper == null)
            {
                return;
            }
            if (objs.Count == wrapper.Values.Length)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    SaveDTO saveDTO = wrapper.Values[i];
                    objs[i].SetFromSaveDTO(saveDTO);
                }
                Debug.Log($"세이브 파일 불러오기 완료: {savePath}");
            }

        }

        private string ReadSaveFile(string savePath)
        {
            if (File.Exists(savePath))
            {
                using (StreamReader reader = new(savePath))
                {
                    string serializedJson = reader.ReadToEnd();
                    return serializedJson;
                }
            }
            return null;
        }

    }
}
