using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Security.Cryptography;
using JCYH;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

namespace SilenceJuJi
{
    public class MyTool : MonoBehaviour
    {
        public class UITool
        {
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
                            Undo.RecordObject(rect, "SetAnchor");
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
        }
        public class OtherTool
        {
            [MenuItem("MyTool/Other/RefreshEditor _F5")]
            public static void EditorRefresh()
            {
                AssetDatabase.Refresh();
                print("RefreshDown");
            }
            [MenuItem("MyTool/Other/ChangeName")]
            public static void ChangeName()//使用前先测试效果
            {
                //Transform selectParent = Selection.activeTransform;
                //Undo.IncrementCurrentGroup();
                //int count = 0;
                //for (int i = 0; i < selectParent.childCount; i++)
                //{
                //    if (selectParent.GetChild(i).GetComponent<JKJC.SensorImage>())
                //    {
                //        count += 1;
                //        Undo.RecordObject(selectParent.GetChild(i).gameObject, "ChangeName");
                //        selectParent.GetChild(i).name = "SST_P06_03_" + (count).ToString("D2");
                //    }
                //}
                string nameArrayStr = "SST_P05_01_01,SST_P05_01_02,SST_P05_01_03,SST_P05_01_04,SST_P05_01_05,SST_P05_01_06,SST_P05_01_07,SST_P05_01_08,SST_P05_02_01,SST_P05_02_02,SST_P05_02_03,SST_P05_02_04,SST_P05_02_05,SST_P05_02_06,SST_P05_02_07,SST_P05_02_08,SST_P06_03_01,SST_P06_03_02,SST_P06_03_03,SST_P06_03_04,SST_P06_03_05,SST_P06_03_06,SST_P06_03_07,SST_P06_03_08,SST_P20_04_01,SST_P20_04_02,SST_P20_04_03,SST_P20_04_04#SST_P20_04_05,SST_P20_04_06,SST_P20_04_07,SST_P20_04_08,SST_P20_05_01,SST_P20_05_02,SST_P20_05_03,SST_P20_05_04,SST_P20_05_05,SST_P20_05_06,SST_P20_05_07,SST_P20_05_08,SST_P22_06_01,SST_P22_06_02,SST_P22_06_03,SST_P22_06_04,SST_P22_06_05,SST_P22_06_06,SST_P22_06_07,SST_P22_06_08";
                string[] nameArray = nameArrayStr.Split(',');
                Transform[] selectParent = Selection.transforms;
                List<Transform> transforms = new List<Transform>();
                for (int i = 0; i < selectParent.Length; i++)
                {
                    transforms.Add(selectParent[i]);
                }
                transforms.Sort((a, b) => {
                    if (a.GetSiblingIndex() > b.GetSiblingIndex())
                    {
                        return 1;
                    }
                    else if (a.GetSiblingIndex() < b.GetSiblingIndex())
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                });
                Undo.IncrementCurrentGroup();
                int count = 0;
                for (int i = 0; i < transforms.Count; i++)
                {
                    Undo.RecordObject(transforms[i].gameObject, "ChangeName");
                    transforms[i].name = nameArray[count];
                    count += 1;
                }
            }
            [MenuItem("MyTool/Other/ChangeName1")]
            public static void ChangeName1()//使用前先测试效果
            {
                //Transform selectParent = Selection.activeTransform;
                //Undo.IncrementCurrentGroup();
                //int count = 0;
                //for (int i = 0; i < selectParent.childCount; i++)
                //{
                //    if (selectParent.GetChild(i).GetComponent<JKJC.SensorImage>())
                //    {
                //        count += 1;
                //        Undo.RecordObject(selectParent.GetChild(i).gameObject, "ChangeName");
                //        selectParent.GetChild(i).name = "SST_P06_03_" + (count).ToString("D2");
                //    }
                //}
                string nameArrayStr = "SST_P05_01_01,SST_P05_01_02,SST_P05_01_03,SST_P05_01_04,SST_P05_01_05,SST_P05_01_06,SST_P05_01_07,SST_P05_01_08,SST_P05_02_01,SST_P05_02_02,SST_P05_02_03,SST_P05_02_04,SST_P05_02_05,SST_P05_02_06,SST_P05_02_07,SST_P05_02_08,SST_P06_03_01,SST_P06_03_02,SST_P06_03_03,SST_P06_03_04,SST_P06_03_05,SST_P06_03_06,SST_P06_03_07,SST_P06_03_08,SST_P20_04_01,SST_P20_04_02,SST_P20_04_03,SST_P20_04_04,SST_P20_04_05,SST_P20_04_06,SST_P20_04_07,SST_P20_04_08,SST_P20_05_01,SST_P20_05_02,SST_P20_05_03,SST_P20_05_04,SST_P20_05_05,SST_P20_05_06,SST_P20_05_07,SST_P20_05_08,SST_P22_06_01,SST_P22_06_02,SST_P22_06_03,SST_P22_06_04,SST_P22_06_05,SST_P22_06_06,SST_P22_06_07,SST_P22_06_08";
                string[] nameArray = nameArrayStr.Split(',');
                Transform[] selectParent = Selection.transforms;
                List<Transform> transforms = new List<Transform>();
                for (int i = 0; i < selectParent.Length; i++)
                {
                    transforms.Add(selectParent[i]);
                }
                transforms.Sort((a, b) => {
                    if (a.GetSiblingIndex() > b.GetSiblingIndex())
                    {
                        return 1;
                    }
                    else if (a.GetSiblingIndex() < b.GetSiblingIndex())
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                });
                Undo.IncrementCurrentGroup();
                int count = 28;
                for (int i = 0; i < transforms.Count; i++)
                {
                    Undo.RecordObject(transforms[i].gameObject, "ChangeName");
                    transforms[i].name = nameArray[count];
                    count += 1;
                }
            }
            [MenuItem("MyTool/Other/GetToken")]
            public static void GetToken()
            {
                print(PlayerPrefs.GetString("ZJY-TOKEN"));
            }
            [MenuItem("MyTool/Other/CopyTran")]
            public static void CopyTransFromStartToEnd()
            {
                CommonDataManager commonDataManager = FindObjectOfType<CommonDataManager>();
                Transform startTran = commonDataManager.startTran;
                Transform endTran = commonDataManager.endTran;

                for (int i = 0; i < startTran.childCount; i++)
                {
                    //子部件
                    Transform child = startTran.GetChild(i);

                    for (int j = 0; j < child.childCount; j++)
                    {
                        //构件
                        Transform grandChild = child.GetChild(j);

                        if (endTran.Find(child.name) != null)
                        {
                            if (endTran.Find(child.name).Find(grandChild.name) != null)
                            {
                                print(grandChild.name + "已存在");
                                //设置偏移
                                endTran.Find(child.name).Find(grandChild.name).position = grandChild.position + commonDataManager.offset;
                                endTran.Find(child.name).Find(grandChild.name).LookAt(grandChild, Vector3.up);
                            }
                            else
                            {
                                GameObject a = new GameObject();
                                a.name = grandChild.name;
                                a.transform.SetParent(endTran.Find(child.name));
                                a.transform.localPosition = Vector3.zero;
                                print(grandChild.name + "已创建");
                                //设置偏移
                                endTran.Find(child.name).Find(grandChild.name).position = grandChild.position + commonDataManager.offset;
                                endTran.Find(child.name).Find(grandChild.name).LookAt(grandChild, Vector3.up);
                            }
                        }
                        else
                        {
                            GameObject a = new GameObject();
                            a.name = child.name;
                            a.transform.SetParent(endTran);
                            a.transform.localPosition = Vector3.zero;
                            print(child.name + "已创建");
                            if (endTran.Find(child.name).Find(grandChild.name) != null)
                            {
                                print(grandChild.name + "已存在");
                                //设置偏移
                                endTran.Find(child.name).Find(grandChild.name).position = grandChild.position + commonDataManager.offset;
                                endTran.Find(child.name).Find(grandChild.name).LookAt(grandChild, Vector3.up);
                            }
                            else
                            {
                                GameObject b = new GameObject();
                                b.name = grandChild.name;
                                b.transform.SetParent(endTran.Find(child.name));
                                b.transform.localPosition = Vector3.zero;
                                print(grandChild.name + "已创建");
                                //设置偏移
                                endTran.Find(child.name).Find(grandChild.name).position = grandChild.position + commonDataManager.offset;
                                endTran.Find(child.name).Find(grandChild.name).LookAt(grandChild, Vector3.up);
                            }
                        }

                    }
                }
            }
            [MenuItem("MyTool/Other/CopyTranStruct")]
            public static void CopyTranStruct()//当复制对象已有部分结构时需保证同级结构没有相同名称物体，否则将得到错误结构
            {
                CommonDataManager commonDataManager = FindObjectOfType<CommonDataManager>();
                Transform startTran = commonDataManager.startTran;
                Transform endTran = commonDataManager.endTran;
                Undo.IncrementCurrentGroup();
                CopyTransCircle(startTran, endTran);
            }
            public static void CopyTransCircle(Transform startTran, Transform endTran)
            {
                if (startTran.childCount > 0)
                {
                    for (int i = 0; i < startTran.childCount; i++)
                    {
                        Transform findTran = endTran.Find(startTran.GetChild(i).name);
                        if (findTran == null)
                        {
                            GameObject a = new GameObject();
                            a.name = startTran.GetChild(i).name;
                            a.transform.position = startTran.GetChild(i).position;
                            a.transform.rotation = startTran.GetChild(i).rotation;
                            a.transform.parent = endTran;
                            a.transform.SetSiblingIndex(i);
                            Undo.RegisterCreatedObjectUndo(a, "CopyTranTotal");
                            //CopyTransCircle(startTran.GetChild(i), a.transform);
                        }
                        else
                        {
                            Undo.RecordObject(findTran, "CopyTranTotal");
                            findTran.transform.position = startTran.GetChild(i).position;
                            findTran.transform.rotation = startTran.GetChild(i).rotation;
                            findTran.SetSiblingIndex(i);
                            //CopyTransCircle(startTran.GetChild(i), findTran);
                        }
                    }
                }
            }
            [MenuItem("MyTool/Other/OppsiteChildrenIndex")]
            public static void OppsiteChildrenIndex()
            {
                Transform selectTran = Selection.activeTransform;
                Undo.IncrementCurrentGroup();
                Undo.RecordObject(selectTran, "OppsiteChildrenIndex");
                List<Transform> children = new List<Transform>();
                for (int i = 0; i < selectTran.childCount; i++)
                {
                    children.Add(selectTran.GetChild(i));
                }
                for (int i = 0; i < children.Count; i++)
                {
                    children[i].parent = null;
                }
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    children[i].parent = selectTran;
                }
            }
            [MenuItem("MyTool/Other/ShowPath _0")]
            public static void ShowPath()
            {
                CommonDataManager commonDataManager = FindObjectOfType<CommonDataManager>();
                Transform startTran = commonDataManager.startTran;
                Transform endTran = commonDataManager.endTran;
                for (int i = 0; i < startTran.childCount - 1; i++)
                {
                    Debug.DrawLine(startTran.GetChild(i).position, startTran.GetChild(i + 1).position, Color.red, 100);
                }
            }
        }
        public class TransformTool
        {
            [MenuItem("MyTool/Transform/SetParentInChildrenCenter")]
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
                    Undo.RecordObject(parent, "SetParentInChildrenCenter");
                    List<Vector3> children = new List<Vector3>();
                    for (int j = 0; j < parent.childCount; j++)
                    {
                        center += parent.GetChild(j).position;
                        children.Add(parent.GetChild(j).position);
                        Undo.RecordObject(parent.GetChild(j), "SetParentInChildrenCenter");
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
            [MenuItem("MyTool/Transform/AdjustSensor")]
            public static void AdjustSensor()
            {
                //获取选择物体
                GameObject[] selectObjs = Selection.gameObjects;
                Undo.IncrementCurrentGroup();
                for (int i = 0; i < selectObjs.Length; i++)
                {
                    //获取选择物体父对象
                    Transform finallyParent = selectObjs[i].transform.parent;
                    Undo.RecordObject(selectObjs[i].transform.GetChild(0).gameObject,"AdjustSensor");
                    //修改传感器物体名称为父对象名称
                    selectObjs[i].transform.GetChild(0).name = selectObjs[i].name;
                    //
                    Undo.SetTransformParent(selectObjs[i].transform.GetChild(0), finallyParent, "AdjustSensor");
                    Undo.DestroyObjectImmediate(selectObjs[i]);
                }
            }
            [MenuItem("MyTool/Unit/CloseCube")]
            public static void CloseCube()
            {
                Transform selectTran = Selection.activeTransform;
                if (selectTran == null && selectTran.name != "位置点按单元划分")
                {
                    Debug.LogError("选中物体错误");
                    return;
                }
                Transform[] transforms = selectTran.GetComponentsInChildren<Transform>();
                for (int i = 0; i < transforms.Length; i++)
                {
                    if (transforms[i].name == "Cube")
                    {
                        if (transforms[i].GetComponent<MeshRenderer>() != null)
                        {
                            transforms[i].gameObject.SetActive(false);
                        }
                        else
                        {
                            Debug.LogError("未关闭" + transforms[i].gameObject, transforms[i].gameObject);
                        }
                    }
                    else
                    {
                        if (transforms[i].GetComponent<MeshRenderer>() != null)
                        {
                            Debug.LogError("未关闭" + transforms[i].gameObject, transforms[i].gameObject);
                        }
                    }
                }
                Debug.LogError("wancheng");
            }
        }
        public class DocumentTool
        {
            [MenuItem("MyTool/Document/ShowDownloadTxt")]
            public static void ShowDownloadTxt()
            {
                Application.OpenURL(Application.streamingAssetsPath + "/DownloadStr.txt");
            }
            [MenuItem("MyTool/Document/ShowConfigTxt")]
            public static void ShowConfigTxt()
            {
                Application.OpenURL(Application.streamingAssetsPath + "/ZJYDataConfig.json");
            }
        }
        public class XXXTool
        {
            [MenuItem("GameObject/计算当前选中物体的数量(Hierarchy)", false, 0)]
            public static void NameTest()
            {
                print("test");
            }
        }
        public class SceneTool
        {
            [MenuItem("MyTool/Scene/MainScene")]
            public static void MainScene()
            {
                EditorSceneManager.OpenScene(Application.dataPath+"/Scenes/HD_ZJY_0614.unity");
            }
            [MenuItem("MyTool/Scene/Login")]
            public static void LoadScene()
            {
                EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/Login.unity");
            }
        }
        //[MenuItem("MyMenu/Do Something with a Shortcut Key %g")]


        #region 注释
        //[MenuItem("MyTool/Other/RenameByIndex")]
        //public static void RenameByIndex()
        //{
        //    Transform selectparent = Selection.activeTransform;
        //    for (int i = 0; i < selectparent.childCount; i++)
        //    {
        //        selectparent.GetChild(i).name = "人民路连接线（隧道）洞身第" + NumberToChinese(i + 1) + "单元";
        //    }
        //}

        ///// <summary>
        ///// 数字转中文
        ///// </summary>
        ///// <param name="number">eg: 22</param>
        ///// <returns></returns>
        //public static string NumberToChinese(int number)
        //{
        //    string res = string.Empty;
        //    string str = number.ToString();
        //    string schar = str.Substring(0, 1);
        //    switch (schar)
        //    {
        //        case "1":
        //            res = "一";
        //            break;
        //        case "2":
        //            res = "二";
        //            break;
        //        case "3":
        //            res = "三";
        //            break;
        //        case "4":
        //            res = "四";
        //            break;
        //        case "5":
        //            res = "五";
        //            break;
        //        case "6":
        //            res = "六";
        //            break;
        //        case "7":
        //            res = "七";
        //            break;
        //        case "8":
        //            res = "八";
        //            break;
        //        case "9":
        //            res = "九";
        //            break;
        //        default:
        //            res = "零";
        //            break;
        //    }
        //    if (str.Length > 1)
        //    {
        //        switch (str.Length)
        //        {
        //            case 2:
        //            case 6:
        //                res += "十";
        //                break;
        //            case 3:
        //            case 7:
        //                res += "百";
        //                break;
        //            case 4:
        //                res += "千";
        //                break;
        //            case 5:
        //                res += "万";
        //                break;
        //            default:
        //                res += "";
        //                break;
        //        }
        //        res += NumberToChinese(int.Parse(str.Substring(1, str.Length - 1)));
        //    }
        //    return res;
        //}
        #endregion


    }
}

