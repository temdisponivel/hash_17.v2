using HASH.GUI;
using UnityEngine;

namespace HASH.Window
{
    public class WindowComponent : MonoBehaviour
    {
        public UIWidget WindowWidget;
        public UIPanel ContentParent;
        public UILabel TitleLabel;

        public bool Maximized;
        public WindowResizingProperties MaximizedProperties;
        
        public bool Minimized;
        public WindowResizingProperties MinimizedProperties;

        public WindowResizer[] Resizers;
        public WindowControlBar ControlBar;

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