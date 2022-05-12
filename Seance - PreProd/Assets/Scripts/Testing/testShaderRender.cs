using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.Testing
{
    public class testShaderRender : MonoBehaviour
    {

        //variables
        private MeshRenderer _mr;

        // Start is called before the first frame update
        void Start()
        {
            _mr = GetComponent<MeshRenderer>();

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ActiveHighlight();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                DesactivateHighlight();
            }
        }

        public void ActiveHighlight()
        {
            //_mr.material.SetFloat("Highlight",1);

            //_mr.materials[0].SetFloat("floatHighlight", 1);

            // srat increase 
            /*float highlightValue = 0;
            float incrementRatio = 0.01f;

            //when float high=1 stop
            while (highlightValue < 100)
            {
                //every seconds =-> +float highlight
                highlightValue += incrementRatio * Time.deltaTime;*/
            //_mr.materials[0].SetFloat("floatHighlight", highlightValue);
            _mr.materials[0].SetFloat("floatHighlight", 1);
            //}

        }

        public void DesactivateHighlight()
        {
            _mr.materials[0].SetFloat("floatHighlight", 0);
        }
    }
}
