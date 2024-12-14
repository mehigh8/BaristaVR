using NaughtyAttributes;
using UnityEngine;

namespace ProjectSetup.CustomXRPluginProvider.Scripts.Editor
{
    /// <summary>
    /// Scriptable object used to persist selected TargetVR.
    /// </summary>
    [CreateAssetMenu(
        menuName = "ProjectSetup/XR Plugin Provider Controller Data",
        fileName = "XRPluginProviderControllerData")]
    public class XRPluginProviderControllerData : ScriptableObject
    {
        [field: ReadOnly]
        [field: SerializeField]
        public Utils.TargetVR TargetVR { get; set; }
    }
}
