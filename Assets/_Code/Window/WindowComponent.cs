using UnityEngine;

namespace HASH.Window
{
    public class WindowComponent : MonoBehaviour
    {
        public UIWidget WindowWidget;
        public UIPanel ContentParent;
        public UILabel TitleLabel;

        public UIDragObject DragObject;

        public bool Maximized;
        public WindowMaximizeProperties MaximizeProperties;

        public void OnCloseClicked()
        {
            WindowUtil.CloseWindowComponent(this);
        }

        public void OnMaximiedClicked()
        {
            WindowUtil.MaximizeWindowComponent(this);
        }
        
        public void OnMinimizeClicked()
        {
            WindowUtil.MinimizeWindowComponent(this);
        }
    }
}