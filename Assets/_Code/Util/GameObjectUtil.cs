namespace HASH
{
    /// <summary>
    /// Utility class for handling stuff related to game objects.
    /// Such as: getting childs of transform, getting components, etc.
    /// </summary>
    public static class GameObjectUtil
    {
        public static void AnchorToParentKeepingValues(UIWidget parent, UIWidget child)
        {
            child.topAnchor.target = parent.transform;
            child.rightAnchor.target = parent.transform;
            child.bottomAnchor.target = parent.transform;
            child.leftAnchor.target = parent.transform;
        }
    }
}