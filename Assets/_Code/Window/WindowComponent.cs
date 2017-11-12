using HASH.GUI;
using SimpleCollections.Lists;
using SimpleCollections.Util;
using UnityEngine;

namespace HASH
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

        public SimpleList<UIWidget> Widgets;
        public SimpleList<UIPanel> Panels;

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

        // Called by WindowClickListener
        public void OnClick()
        {
            WindowUtil.Focus(this);
        }
    }
}