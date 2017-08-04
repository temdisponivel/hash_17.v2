namespace HASH.Game
{
    /// <summary>
    /// Interface used to initilize components at the start of the game.
    /// Use this in order to be initialized regardless of unity event.
    /// You have to be child of a game holder or another component that used this interface to initilize its children.
    /// </summary>
    public interface IInitializable
    {
        /// <summary>
        /// Should return the order by which this should be initialized.
        /// A best approach would be to use the sibiling index of the game object's transform
        /// so that you are initialized in the same order you appear on your scene hierarchy.
        /// Try your best to keep this orders unique, so that the sorting can go smoothly.
        /// </summary>
        int GetOrder();

        /// <summary>
        /// Called when the object should be initialized.
        /// This method will be called in order, accordingly to the GetOrder result.
        /// You should perform any operation here, including calling Initilize in your children, if that's the case.
        /// You can also enable a inactive game object here.
        /// </summary>
        void Initialize();
    }
}