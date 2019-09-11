using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    
    
    public class MeshVertManager : MonoBehaviour
    {
        public Object[] vertModels;
        //this data really shouldn't be here, and classes shouldn't communicate through the static class
        public static Dictionary<int, VertsObject> vertModelDict;
        public static BouncyShoot bouncyShootRef;
        

        void Start()
        {
            vertModelDict = new Dictionary<int, VertsObject>();
            vertModels = Resources.LoadAll("VertModels", typeof(GameObject));
            Debug.Log(vertModels.Length);
            int i = 0;
            foreach (Object o in vertModels)
            {
              
                //trying to fix a different error
                GameObject g = (GameObject) o;
                vertModelDict[i] = new VertsObject(g.name, g.GetComponent<MeshFilter>().sharedMesh.vertices,
                    //only uses x scaler because wont need all axis
                    g.transform.localScale.x);
                i++;
            }
        }

        public static List<BallClass> fireVertModel(GameObject modelParent)
        {
            List<BallClass> ballStructs = new List<BallClass>();
            VertsObject vertObj = vertModelDict[Static.vertModelIndex % vertModelDict.Count];

            for (int i = 0; i < vertObj.verts.Length; i++)
            {

                //launches multiplied by localscale.x for all of it... wont need to do each axis individually
                if (i % Static.imageDivideBy == 0)
                {
                    BallClass ballS = bouncyShootRef.ballFire(bouncyShootRef.gameObject,
                        vertObj.verts[i] * vertObj.scale * Static.ballSeperatness, Color.clear);
                    ballS.ball.transform.parent = modelParent.transform;
                    ballStructs.Add(ballS);
                }

            }
            return ballStructs;
        }
     

    }

    public class VertsObject
    {
        public VertsObject(string n, Vector3[] vs, float s = 1)
        {
         
            name = n;
            scale = s;
            verts = vs;
        }
        //ignore saying it can be private
        public readonly string name;

        public readonly float scale;
        public readonly Vector3[] verts;
    }
}