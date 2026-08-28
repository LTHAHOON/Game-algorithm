using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace KoiAI.KoiCursor
{
    public enum CursorType
    {
        Base,
        Hover
    }
    public class CursorService : MonoBehaviour
    {
        [SerializeField]
        private Texture2D _baseCursorTex;
        [SerializeField]
        private Texture2D _hoverCursorTex;

        private static Texture2D _baseCursorTex_Instance;
        private static Texture2D _hoverCursorTex_Instance;
        private static readonly Dictionary<(VisualElement element, Type eventType), Delegate> _elementCursorCallbacks = new();
        private void Awake()
        {
            _baseCursorTex_Instance = _baseCursorTex;
            _hoverCursorTex_Instance = _hoverCursorTex;
            Cursor.SetCursor(_baseCursorTex, Vector2.zero, CursorMode.Auto);
        }

        public static void RegisterElementCursor<T>(VisualElement element, CursorType cursorType, CursorMode cursorMode = CursorMode.Auto) where T : PointerEventBase<T>, new()
        {
            if (element == null)
            {
                return;
            }

            UnregisterElementCursor<T>(element);

            Texture2D cursorTex = GetCursorTex(cursorType);
            EventCallback<T> setCursorEvent = _ =>
                {
                    Cursor.SetCursor(cursorTex, Vector2.zero, cursorMode);
                };

            element.RegisterCallback(setCursorEvent);
            _elementCursorCallbacks[(element, typeof(T))] = setCursorEvent;
        }

        public static void UnregisterElementCursor<T>(VisualElement element) where T : PointerEventBase<T>, new()
        {
            if (element == null)
            {
                return;
            }

            var key = (element, typeof(T));
            if (!_elementCursorCallbacks.TryGetValue(key, out Delegate callback))
            {
                return;
            }

            element.UnregisterCallback((EventCallback<T>)callback);
            _elementCursorCallbacks.Remove(key);
        }

        private static Texture2D GetCursorTex(CursorType cursorType)
        {
            Texture2D cursorTex = cursorType switch
            {
                CursorType.Base => _baseCursorTex_Instance,
                CursorType.Hover => _hoverCursorTex_Instance,
                _ => null
            };
            return cursorTex;
        }
    }
}
