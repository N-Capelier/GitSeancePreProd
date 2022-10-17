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


        SerializedProperty _roomID, _name, _xLength, _yLength, _tilesColor, _tileRotationSave, _otherTileRotationSave, _newTileRotation, _newOtherTileRotation;

        private void OnEnable()
        {
            _roomID = serializedObject.FindProperty("_roomID");
            _name = serializedObject.FindProperty("_name");
            _xLength = serializedObject.FindProperty("_xLength");
            _yLength = serializedObject.FindProperty("_yLength");
            _tilesColor = serializedObject.FindProperty("_tilesColor");

            //show save lists
            _tileRotationSave = serializedObject.FindProperty("_tileRotationSave");
            _otherTileRotationSave = serializedObject.FindProperty("_otherTileRotationSave");

            _newTileRotation = serializedObject.FindProperty("_newTileRotation");
            _newOtherTileRotation = serializedObject.FindProperty("_newOtherTileRotation");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Open Window")) OpenWindow();
            EditorGUILayout.PropertyField(_roomID);
            EditorGUILayout.PropertyField(_name);
            EditorGUILayout.PropertyField(_xLength);
            EditorGUILayout.PropertyField(_yLength);
            EditorGUILayout.PropertyField(_tilesColor);

            //show save lists
            EditorGUILayout.PropertyField(_tileRotationSave);
            EditorGUILayout.PropertyField(_otherTileRotationSave);

            EditorGUILayout.PropertyField(_newTileRotation);
            EditorGUILayout.PropertyField(_newOtherTileRotation);

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
