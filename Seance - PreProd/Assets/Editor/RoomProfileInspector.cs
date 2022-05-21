using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    [CustomEditor(typeof(RoomProfile))]
    public class RoomProfileInspector : Editor
    {
        /// <summary>
        /// TODO List :
        /// - OnEnable()
        /// - OnInspectorGUI()
        /// - OpenWindow()
        /// </summary>


        SerializedProperty _roomID, _xLength, _yLength, _tilesColor, _tileRotationSave, _otherTileRotationSave;

        private void OnEnable()
        {
            _roomID = serializedObject.FindProperty("_roomID");
            _xLength = serializedObject.FindProperty("_xLength");
            _yLength = serializedObject.FindProperty("_yLength");
            _tilesColor = serializedObject.FindProperty("_tilesColor");

            //show save lists
            _tileRotationSave = serializedObject.FindProperty("_tileRotationSave");
            _otherTileRotationSave = serializedObject.FindProperty("_otherTileRotationSave");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Open Window")) OpenWindow();
            EditorGUILayout.PropertyField(_roomID);
            EditorGUILayout.PropertyField(_xLength);
            EditorGUILayout.PropertyField(_yLength);
            EditorGUILayout.PropertyField(_tilesColor);

            //show save lists
            EditorGUILayout.PropertyField(_tileRotationSave);
            EditorGUILayout.PropertyField(_otherTileRotationSave);

            serializedObject.ApplyModifiedProperties();
        }

        private void OpenWindow()
        {
            RoomGeneratorWindow myWindow = EditorWindow.GetWindow(typeof(RoomGeneratorWindow)) as RoomGeneratorWindow;

            myWindow.InitializeWindow(target as RoomProfile); //fonction a créé
            myWindow.Show(); //fonction de base d'unity
        }

    }
}
