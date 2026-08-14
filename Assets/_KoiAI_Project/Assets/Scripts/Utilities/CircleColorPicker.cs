using UnityEngine;
using UnityEngine.UIElements;

namespace KoiAI.Utilities
{
    using KoiAI.UI;

    public interface ICircleColorPickerHandler 
    {
        public void OnColorChanged(CircleColorPicker circleColorPicker, Color newColor);
    }

    public class CircleColorPicker
    {
        private VisualElement _root;
        private VisualElement _circlePalette;
        private VisualElement _picker;
        private bool _isDragging = false;

        private Color SelectedColor { get; set; } = Color.white;
        private ICircleColorPickerHandler _colorPickerHandler;

        public CircleColorPicker(ICircleColorPickerHandler colorPickerHandler, VisualElement root, string circlePaletteName, string pickerName)
        {
            if (root == null || colorPickerHandler == null)
            {
                return;
            }
            Init(colorPickerHandler, root, circlePaletteName, pickerName);
        }
        

        private void Init(ICircleColorPickerHandler colorPickerHandler, VisualElement root, string circlePaletteName, string pickerName)
        {
            _root = root;
            _colorPickerHandler = colorPickerHandler;
            _circlePalette = _root.Q<VisualElement>(circlePaletteName);
            _picker = _root.Q<VisualElement>(pickerName);

            if (_circlePalette == null || _picker == null)
            {
                Debug.LogError("UXML에서 'CirclePalette' 또는 'Picker' 요소를 찾을 수 없습니다.");
            }
        }
        
        public void RegisterAllCallBack(Color curColor)
        {
            if (_circlePalette == null)
            {
                return;
            }
            _circlePalette.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _circlePalette.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _circlePalette.RegisterCallback<PointerUpEvent>(OnPointerUp);

            Vector2 curColorPosition = GetColorPosition(curColor);
            UpdatePickerAndColor(curColorPosition);
        }

        public void UnregisterAllCallBack(Color curColor)
        {
            if (_circlePalette == null)
            {
                return;
            }
            _circlePalette.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            _circlePalette.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            _circlePalette.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            _circlePalette.ReleaseMouse();

            Vector2 curColorPosition = GetColorPosition(curColor);
            UpdatePickerAndColor(curColorPosition);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            _isDragging = true;
            _circlePalette.CapturePointer(evt.pointerId); // 마우스가 원판 밖으로 나가도 드래그 유지
            UpdatePickerAndColor(evt.position);
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (_isDragging && _circlePalette.HasPointerCapture(evt.pointerId))
            {
                UpdatePickerAndColor(evt.position);
            }
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (_isDragging)
            {
                _circlePalette.ReleasePointer(evt.pointerId);
                _isDragging = false;
            }
        }


        private void UpdatePickerAndColor(Vector2 screenPosition)
        {
            // 1. 전역 스크린 좌표를 원판 중심 기준의 로컬 좌표로 변환
            Rect bounds = _circlePalette.worldBound;
            Vector2 center = bounds.center;
            Vector2 localOffset = screenPosition - center;

            float maxRadius = bounds.width * 0.5f;
            float currentRadius = localOffset.magnitude;

            // 2. 피커가 원판 밖으로 나가지 못하도록 반지름 제한 (Clamp)
            if (currentRadius > maxRadius)
            {
                localOffset = localOffset.normalized * maxRadius;
                currentRadius = maxRadius;
            }

            // 3. 피커 UI 위치 업데이트 (원판 내부 좌표계 기준 계산)
            // UI Toolkit의 Absolute 포지션은 좌측 상단(0,0) 기준이므로 변환 필요
            float pickerX = (bounds.width * 0.5f) + localOffset.x - (_picker.layout.width * 0.5f);
            float pickerY = (bounds.height * 0.5f) + localOffset.y - (_picker.layout.height * 0.5f);

            _picker.style.left = pickerX;
            _picker.style.top = pickerY;

            // 4. 수학적 HSV 색상 계산
            // Atan2 결과를 0~360도로 변환 후 0~1 값으로 정규화 (Hue)
            float angle = Mathf.Atan2(-localOffset.y, localOffset.x) * Mathf.Rad2Deg; // Y축 반전 반영
            if (angle < 0) angle += 360f;
            float hue = angle / 360f;

            // 중심에서 멀어질수록 채도가 높아짐 (Saturation)
            float saturation = Mathf.Clamp01(currentRadius / maxRadius);

            // 명도(Value)는 기본값 1f (필요시 외부 슬라이더로 변경 가능)
            float value = 1f;

            // 5. 최종 색상 추출 및 이벤트 발송
            SelectedColor = Color.HSVToRGB(hue, saturation, value);

            _colorPickerHandler?.OnColorChanged(this, SelectedColor);
        }

        private Vector3 GetColorPosition(Color color)
        {
            Color.RGBToHSV(color, out float hue, out float saturation, out float value);

            Rect bounds = _circlePalette.worldBound;
            Vector2 center = bounds.center;
            float maxRadius = bounds.width * 0.5f;

            // 3. Hue(0~1)를 라디안 각도로 역산 (Atan2 역산)
            // 원래 코드에서 Y축을 반전시켰으므로(-localOffset.y), 사인 값에 마이너스를 붙여줍니다.
            float angle = hue * 360f;
            float rad = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), -Mathf.Sin(rad));

            // 4. Saturation(0~1)을 이용해 실제 반지름 거리 구하기
            float currentRadius = saturation * maxRadius;

            // 5. 중심으로부터의 오프셋 벡터 계산
            Vector2 localOffset = direction * currentRadius;

            // 6. 최종 전역 스크린 좌표(screenPosition) 도출
            Vector2 screenPosition = center + localOffset;

            return screenPosition;
        }
        
        public void MovePickerToCenter()
        {
            UpdatePickerAndColor(_circlePalette.worldBound.center);
        }
    }
}
