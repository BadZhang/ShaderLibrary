using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class BlendModeGrabGUI : ShaderGUI
{
    // OnGuI ���յ���������
    MaterialEditor materialEditor;       //��ǰ�������
    MaterialProperty[] materialProperty; //��ǰshader��properties
    Material targetMat;                  //���ƶ��������

    //private Material mat;

    #region ���ö��
    enum BlendModeCategory
    {
        һ��_Normal = 0,
        �䰵_Darken = 1,
        ����_Lighten = 2,
        �Ա�_Contrast = 3,
        ��ת_Inversion = 4,
        �ϳ�_Component = 5,
    }

    enum NormalBlendMode
    {
        ����_Normal = 0,
        ͸�����_Alphablend = 1,
    }
    enum DarkenBlendMode
    {
        �䰵_Darken = 2,
        ��Ƭ����_Multiply = 3,
        ��ɫ����_ColorBurn = 4,
        ���Լ���_LinearBurn = 5,
        ��ɫ_DarkerColor = 6,
    }

    enum LightenBlendMode
    {
        ����_Lighten = 7,
        ��ɫ_Screen = 8,
        ��ɫ����_ColorDodge = 9,
        ���Լ���_LinearDodge = 10,
        ǳɫ_LighterColor = 11,
    }

    enum ContrastBlendMode
    {
        ����_Overlay = 12,
        ���_SoftLight = 13,
        ǿ��_HardLight = 14,
        ����_VividLight = 15,
        ���Թ�_LinearLight = 16,
        ���_PinLight = 17,
        ʵɫ���_HardMix = 18,
    }

    enum InversionBlendMode
    {
        ��ֵ_Difference = 19,
        �ų�_Exclusion = 20,
        ��ȥ_Subtract = 21,
        ����_Divide = 22,
    }

    enum ComponentBlendMode
    {
        ɫ��_Hue = 23,
        ���Ͷ�_Saturation = 24,
        ��ɫ_Color = 25,
        ����_Luminosity = 26,
    }

    enum BlendModeChoose
    {
        ����_Normal,
        ͸�����_Alphablend,
        �䰵_Darken,
        ��Ƭ����_Multiply,
        ��ɫ����_ColorBurn,
        ���Լ���_LinearBurn,
        ��ɫ_DarkerColor,
        ����_Lighten,
        ��ɫ_Screen,
        ��ɫ����_ColorDodge,
        ���Լ���_LinearDodge,
        ǳɫ_LighterColor,
        ����_Overlay,
        ���_SoftLight,
        ǿ��_HardLight,
        ����_VividLight,
        ���Թ�_LinearLight,
        ���_PinLight,
        ʵɫ���_HardMix,
        ��ֵ_Difference,
        �ų�_Exclusion,
        ��ȥ_Subtract,
        ����_Divide,
        ɫ��_Hue,
        ���Ͷ�_Saturation,
        ��ɫ_Color,
        ����_Luminosity
    }

    #endregion

    #region ��������

    string[] MateritalChoosenames = System.Enum.GetNames(typeof(BlendModeChoose));
    string[] NormalModeChoosenames = System.Enum.GetNames(typeof(NormalBlendMode));
    string[] DarkenModeChoosenames = System.Enum.GetNames(typeof(DarkenBlendMode));
    string[] LightenModeChoosenames = System.Enum.GetNames(typeof(LightenBlendMode));
    string[] ContrastModeChoosenames = System.Enum.GetNames(typeof(ContrastBlendMode));
    string[] InversionModeChoosenames = System.Enum.GetNames(typeof(InversionBlendMode));
    string[] ComponentModeChoosenames = System.Enum.GetNames(typeof(ComponentBlendMode));
    string[] BlendCategoryChoosenames = System.Enum.GetNames(typeof(BlendModeCategory));

    private float choice = 0f;

    #endregion

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        this.materialEditor = materialEditor;               // ��ǰ�༭��
        this.materialProperty = properties;                 // �õ��ı���
        this.targetMat = materialEditor.target as Material; // ��ǰ������

        #region Shader����

        MaterialProperty _BaseColor = FindProperty("_BaseColor", properties);
        MaterialProperty _BaseMap = FindProperty("_BaseMap", properties);
        MaterialProperty _MixColor = FindProperty("_MixColor", properties);
        MaterialProperty _MixMap = FindProperty("_MixMap", properties);

        MaterialProperty ModeID = FindProperty("_ModeID", properties);
        MaterialProperty ModeChooseProps = FindProperty("_IDChoose", properties);
        MaterialProperty CategoryChooseProps = FindProperty("_BlendCategoryChoose", properties);

        MaterialProperty _StyleChoose = FindProperty("_StyleChoose", materialProperty);
        MaterialProperty _EnableCameraTex = FindProperty("_EnableCameraTex", materialProperty);

        GUIContent StyleChoose = new GUIContent("��˫����ʾģʽ�л�");
        GUIContent EnableCameraTex = new GUIContent("�Ƿ�ʹ���������");

        #endregion

        #region �б�ö��
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.Space(10);

        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = _StyleChoose.hasMixedValue;
        EditorGUI.showMixedValue = false;

        var _STYLECHOOSE_ON = EditorGUILayout.Toggle(StyleChoose, _StyleChoose.floatValue == 1);
        if (EditorGUI.EndChangeCheck()) _StyleChoose.floatValue = _STYLECHOOSE_ON ? 1 : 0;  

        //�򿪿���֮���Ч��
        if (_StyleChoose.floatValue == 1)
        {
            ModeChooseProps.floatValue = EditorGUILayout.Popup("ѡ������", (int)ModeChooseProps.floatValue, MateritalChoosenames);
            choice = ModeChooseProps.floatValue;
        }
        else
        {
            
            //ѡ���Ϸ���
            CategoryChooseProps.floatValue = EditorGUILayout.Popup("ѡ������", (int)CategoryChooseProps.floatValue, BlendCategoryChoosenames);

            EditorGUI.indentLevel++;
            //ѡ�񷶳��ѡ�������ģʽ
            switch (CategoryChooseProps.floatValue)
            {
                case (float)BlendModeCategory.һ��_Normal:
                    ModeChooseProps.floatValue = EditorGUILayout.Popup("NormalBlendMode", (int)ModeChooseProps.floatValue, NormalModeChoosenames);
                    choice = ModeChooseProps.floatValue;
                    break;
                case (float)BlendModeCategory.�䰵_Darken:
                    ModeChooseProps.floatValue = EditorGUILayout.Popup("DarkenBlendMode", (int)ModeChooseProps.floatValue, DarkenModeChoosenames);
                    choice = ModeChooseProps.floatValue;
                    choice += 2; 
                    break;
                case (float)BlendModeCategory.����_Lighten:
                    ModeChooseProps.floatValue = EditorGUILayout.Popup("LightenBlendMode", (int)ModeChooseProps.floatValue, LightenModeChoosenames);
                    choice = ModeChooseProps.floatValue;
                    choice += 7;
                    break;
                case (float)BlendModeCategory.�Ա�_Contrast:
                    ModeChooseProps.floatValue = EditorGUILayout.Popup("ContrastBlendMode", (int)ModeChooseProps.floatValue, ContrastModeChoosenames);
                    choice = ModeChooseProps.floatValue;
                    choice += 12;
                    break;
                case (float)BlendModeCategory.��ת_Inversion:
                    ModeChooseProps.floatValue = EditorGUILayout.Popup("InversionBlendMode", (int)ModeChooseProps.floatValue, InversionModeChoosenames);
                    choice = ModeChooseProps.floatValue;
                    choice += 19;
                    break;
                case (float)BlendModeCategory.�ϳ�_Component:
                    ModeChooseProps.floatValue = EditorGUILayout.Popup("ComponentBlendMode", (int)ModeChooseProps.floatValue, ComponentModeChoosenames);
                    choice = ModeChooseProps.floatValue;
                    choice += 23;
                    break;
            }

            EditorGUI.indentLevel--;
        }
        #endregion

        switch (choice)
        {
            case 0:
                ModeID.floatValue = 0;
                break;
            case 1:
                ModeID.floatValue = 1;
                break;
            case 2:
                ModeID.floatValue = 2;
                break;
            case 3:
                ModeID.floatValue = 3;
                break;
            case 4:
                ModeID.floatValue = 4;
                break;
            case 5:
                ModeID.floatValue = 5;
                break;
            case 6:
                ModeID.floatValue = 6;
                break;
            case 7:
                ModeID.floatValue = 7;
                break;
            case 8:
                ModeID.floatValue = 8;
                break;
            case 9:
                ModeID.floatValue = 9;
                break;
            case 10:
                ModeID.floatValue = 10;
                break;
            case 11:
                ModeID.floatValue = 11;
                break;
            case 12:
                ModeID.floatValue = 12;
                break;
            case 13:
                ModeID.floatValue = 13;
                break;
            case 14:
                ModeID.floatValue = 14;
                break;
            case 15:
                ModeID.floatValue = 15;
                break;
            case 16:
                ModeID.floatValue = 16;
                break;
            case 17:
                ModeID.floatValue = 17;
                break;
            case 18:
                ModeID.floatValue = 18;
                break;
            case 19:
                ModeID.floatValue = 19;
                break;
            case 20:
                ModeID.floatValue = 20;
                break;
            case 21:
                ModeID.floatValue = 21;
                break;
            case 22:
                ModeID.floatValue = 22;
                break;
            case 23:
                ModeID.floatValue = 23;
                break;
            case 24:
                ModeID.floatValue = 24;
                break;
            case 25:
                ModeID.floatValue = 25;
                break;
            case 26:
                ModeID.floatValue = 26;
                break;
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(30);

        #region ��ɫͼƬGUI
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.Space(10);
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = _EnableCameraTex.hasMixedValue;
        EditorGUI.showMixedValue = false;

        var _ENABLECAMERATEX_ON = EditorGUILayout.Toggle(EnableCameraTex, _EnableCameraTex.floatValue == 1);
        if (EditorGUI.EndChangeCheck()) _EnableCameraTex.floatValue = _ENABLECAMERATEX_ON ? 1 : 0;

        //�򿪿���֮���Ч��
        if (_EnableCameraTex.floatValue == 1)
        {
            targetMat.EnableKeyword("_ENABLECAMERATEX_ON");
            EditorGUI.indentLevel++;
            GUILayout.Label("����ʹ���������");
            EditorGUI.indentLevel--;
        }
        else
        {
            targetMat.DisableKeyword("_ENABLECAMERATEX_ON");
            materialEditor.ColorProperty(_BaseColor, "������ɫ");
            materialEditor.TextureProperty(_BaseMap, "������ͼ");
        }

        materialEditor.ColorProperty(_MixColor, "�����ɫ");
        materialEditor.TextureProperty(_MixMap, "�����ͼ");

        EditorGUILayout.Space(10);
        EditorGUILayout.EndVertical();

        // Render Queue
        EditorGUILayout.Space(20);

        materialEditor.RenderQueueField();
        materialEditor.EnableInstancingField();
        materialEditor.DoubleSidedGIField();
        #endregion
    }
}