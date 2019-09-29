using UnityEngine;

namespace Assets
{
    public class Layer
    {
        public static int Ground => LayerMask.NameToLayer("Ground");
        public static int GroundMask => LayerMask.GetMask("Ground");
    }
}
