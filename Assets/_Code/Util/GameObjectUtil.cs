using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Utility class for handling stuff related to game objects.
    /// Such as: getting childs of transform, getting components, etc.
    /// </summary>
    public static class GameObjectUtil
    {
        public static void AnchorToParentKeepingValues(GameObject parent, UIWidget child)
        {
            child.topAnchor.target = parent.transform;
            child.topAnchor.absolute = 0;
            
            child.rightAnchor.target = parent.transform;
            child.rightAnchor.absolute = 0;
            
            child.bottomAnchor.target = parent.transform;
            child.bottomAnchor.absolute = 0;
            
            child.leftAnchor.target = parent.transform;
            child.leftAnchor.absolute = 0;
        }

        public static Bounds GetDragObjectBounds(UIDragObject dragObject)
        {
            Bounds bounds;
            if (dragObject.contentRect)
            {
                var t = dragObject.panelRegion.cachedTransform;
                var toLocal = t.worldToLocalMatrix;
                var corners = dragObject.contentRect.worldCorners;
                for (var i = 0; i < 4; ++i) corners[i] = toLocal.MultiplyPoint3x4(corners[i]);
                bounds = new Bounds(corners[0], Vector3.zero);
                for (var i = 1; i < 4; ++i) bounds.Encapsulate(corners[i]);
            }
            else
            {
                bounds = NGUIMath.CalculateRelativeWidgetBounds(dragObject.panelRegion.cachedTransform, dragObject.target);
            }

            return bounds;
        }
    }
}