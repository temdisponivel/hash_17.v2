using UnityEngine;

namespace HASH.Window
{
    public class WindowComponent : MonoBehaviour
    {
        public UIWidget ContentParent;
        public UILabel TitleLabel;

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