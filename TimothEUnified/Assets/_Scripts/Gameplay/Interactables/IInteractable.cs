namespace TimothE.Gameplay.Interactables
{
    //////////////////////////////////////////////////
    [System.Serializable]
    public enum InteractDirection
    {
        None = -1,
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }

    //////////////////////////////////////////////////
    public interface IInteractable
    {
        //////////////////////////////////////////////////
        public abstract void OnUse(PlayerController controller);
    }
}
