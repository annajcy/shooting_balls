using System.Collections.Generic;
using UnityEngine;

namespace MaterialApplier
{
    public class RandomizeColor : MonoBehaviour
    {
        private static List<Color> colorList = new List<Color>()
        {
            Color.cyan,
            Color.magenta,
            Color.yellow,
            Color.green,
            Color.red,
            Color.white,
        };

        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        private void GenerateColor(GameObject cube)
        {
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            cube.GetComponent<Renderer>().GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(BaseColor, colorList[Random.Range(0, colorList.Count)]);
            cube.GetComponent<Renderer>().SetPropertyBlock(propertyBlock);
        }

        public void Work()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GenerateColor(this.transform.GetChild(i).gameObject);
                this.transform.GetChild(i).gameObject.tag = "Tower";
            }
        }

        private void Start() { Work(); }

    }
}
