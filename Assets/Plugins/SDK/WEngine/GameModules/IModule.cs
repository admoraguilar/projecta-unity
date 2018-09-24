using UnityEngine;


public interface IInitializable {
    /// <summary>
    /// Used for manual ordering of module initialization. Useful if some module
    /// needs to be initialized first before some other module.
    /// </summary>
    int Priority { get; }

    void Initialize();
}


public interface IDeinitializable {
    void Deinitialize();
}