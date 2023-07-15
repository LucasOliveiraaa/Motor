using UnityEngine;

namespace Motor
{
    [Icon(gizmos.main)]
    public static class MotorProperties
    {
        public const string m_base_url = "https://bios-technologies.com/Features/Unity/Motor/";

        public class gizmos
        {
            public const string base_gizmos = "Assets/Motor/Editor/Resources/Gizmos";
            public const string ppl = "m_gizmo";
            public const string main = base_gizmos + "/" + ppl + "-main.png";
            public const string comp = base_gizmos+"/"+ppl+"-comp.png";
            public const string camera = base_gizmos+"/"+ppl+"-camera.png";
            public const string controller = base_gizmos+"/"+ppl+"-controller.png";
            public const string cine = base_gizmos+"/"+ppl+"-cine.png";
        }

        public class images
        {
            public const string base_images = "Assets/Motor/Editor/Resources";
            public const string ppl = "m_image";
            public const string icon = base_images + "/m_image-icon.png";
            public const string logo = base_images + "/m_image-logo.png";
            public const string full_logo = base_images + "/m_image-full-logo.png";
        }
    }
}