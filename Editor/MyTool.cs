using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Security.Cryptography;
namespace SilenceJuJi
{
    public class MyTool : MonoBehaviour
    {
        //[MenuItem("MyMenu/Do Something with a Shortcut Key %g")]
        [MenuItem("MyTool/UI/SetAnchors2Corners %g")]
        static void SetAnchors2Corners()
        {
            GameObject[] selectObjs = Selection.gameObjects;
            Undo.IncrementCurrentGroup();
            foreach (var item in selectObjs)
            {

                if (item.hideFlags == HideFlags.NotEditable)
                {
                    return;
                }
                else
                {
                    RectTransform rect = item.GetComponent<RectTransform>();
                    if (rect)
                    {
                        Undo.RecordObject(rect,"SetAnchor");
                        Anchors2Corners(item.GetComponent<RectTransform>());
                    }

                }
            }
        }
        public static void Anchors2Corners(RectTransform RectTran)
        {
            RectTransform ParentRectTran = RectTran.parent as RectTransform;
            if (RectTran == null || ParentRectTran == null)
            {
                return;
            }
            RectTran.localRotation = Quaternion.identity;

            Vector3[] rCorners = new Vector3[4];
            Vector3[] pCorners = new Vector3[4];
            Vector2 r2pLeftBottom = Vector2.zero;
            Vector2 r2pTopRight = Vector2.zero;

            //四个脚点坐标,方向坐下顺时针到右下角
            RectTran.GetWorldCorners(rCorners);
            ParentRectTran.GetWorldCorners(pCorners);

            //当前物体的左下角和右上角
            Vector2 rleftBottom = rCorners[0];
            Vector2 rtopRight = rCorners[2];
            //当前物体父物体的左下角和右上角
            Vector2 pleftBottom = pCorners[0];
            Vector2 ptopRight = pCorners[2];

            //当前物体两个角相对父物体左下角向量
            r2pLeftBottom = rleftBottom - pleftBottom;
            r2pTopRight = rtopRight - pleftBottom;

            float offsetX = pCorners[3].x - pCorners[0].x;
            float offsetY = pCorners[1].y - pCorners[0].y;

            r2pLeftBottom.x /= offsetX;
            r2pLeftBottom.y /= offsetY;

            r2pTopRight.x /= offsetX;
            r2pTopRight.y /= offsetY;
            //归一化向量并设置锚点
            RectTran.anchorMin = r2pLeftBottom;
            RectTran.anchorMax = r2pTopRight;
            RectTran.offsetMin = Vector2.zero;
            RectTran.offsetMax = Vector2.zero;
        }

        [MenuItem("MyTool/Other/ChangeName")]
        public static void ChangeName()
        {
            //string oldName = "Z001500105L1001U67C0300R01";
            //oldName= oldName.Remove(oldName.IndexOf('C'),1);
            //oldName= oldName.Insert(oldName.IndexOf('U')+1,"C");

            GameObject[] selectObjs = Selection.gameObjects;
            var children = selectObjs[0].GetComponentsInChildren<Transform>();
            Undo.IncrementCurrentGroup();
            foreach (var item in children)
            {
                if (item.name.Contains("Z00015"))
                {
                    string oldName = item.name;
                    Undo.RecordObject(item.gameObject,"ChangeName");
                    //oldName = oldName.Remove(oldName.IndexOf("C"), 1);
                    oldName = oldName.Insert(17, "C");
                    //oldName = oldName.Insert(oldName.IndexOf('Z') + 1, "0");
                    item.name = oldName;
                }
            }

            string name = "Z0001500105L1001UC060100U01";
        }

        [MenuItem("MyTool/Other/GetToken")]
        public static void GetToken()
        {
            print(PlayerPrefs.GetString("ZJY-TOKEN"));
        }
        [MenuItem("MyTool/Transform/ResetParentZero")]
        public static void SetParentInChildrenCenter()
        {
            Undo.IncrementCurrentGroup();
            GameObject[] selectObjs = Selection.gameObjects;
            for (int i = 0; i < selectObjs.Length; i++)
            {
                Vector3 center = Vector3.zero;
                Transform parent = selectObjs[i].transform;
                if (parent.childCount == 0)
                {
                    break;
                }
                Undo.RecordObject(parent, "ResetParentZero");
                List<Vector3> children = new List<Vector3>();
                for (int j = 0; j < parent.childCount; j++)
                {
                    center += parent.GetChild(j).position;
                    children.Add(parent.GetChild(j).position);
                    Undo.RecordObject(parent.GetChild(j), "ResetParentZero");
                }
                //计算中心
                center = center / children.Count;
                parent.position = center;
                Vector3 offset = center - parent.position;
                for (int j = 0; j < parent.childCount; j++)
                {
                    parent.GetChild(j).position = children[j];
                }
            }
        }
    }
}

