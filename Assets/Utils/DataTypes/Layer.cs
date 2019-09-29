using UnityEngine;

namespace Assets
{
    public class Layer
    {
        public static string Ground => "Ground";
        public static int GroundMask => LayerMask.GetMask(Ground);

        public static string Selectable => "Selectable";
        public static int SelectableMask => LayerMask.GetMask(Selectable);
    }
}
