using UnityEngine;

namespace MaterialApplier
{
    public class TransparentMaterial
    {
        public static void Attach(GameObject gameObject)
        {
            UnityEngine.Material transparentMaterial = new UnityEngine.Material(Shader.Find("Universal Render Pipeline/Lit"));
            Color color = new Color(1f, 1f, 1f, 0.5f);
            transparentMaterial.SetColor("_BaseColor", color);
            transparentMaterial.SetFloat("_Surface", 1);
            transparentMaterial.SetFloat("_Blend", 0);
            gameObject.GetComponent<Renderer>().material = transparentMaterial;
        }
    }
}