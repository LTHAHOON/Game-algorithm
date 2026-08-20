using UnityEngine.UIElements;

namespace KoiAI.UI
{
    public abstract class VisualView<TInfo> where TInfo : VisualViewInfo
    {
        public VisualElement _root;
        public VisualView(VisualElement root, TInfo info)
        {
            _root = root;
            if (IsValid(root, info))
            {
                Initalize(root, info);
            }
        }

        protected abstract void Initalize(VisualElement root, TInfo info);

        private bool IsValid(VisualElement root, TInfo info) => root != null && info != null;
        public VisualElement Root => _root;
    }
}
