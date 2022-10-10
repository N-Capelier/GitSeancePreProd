using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    [CustomEditor(typeof(FloorProfile))]
    public class FloorProfileInspector : Editor
    {
        /// <summary>
        /// TODO List :
        /// - OnEnable()
        /// - OnInspectorGUI()
        /// - OpenWindow()
        /// </summary>

        SerializedProperty _roomDisplays, _connections, _floor, _roomProfiles, _currentRoomTypeSelected, _roomTypeColors;

        private void OnEnable()
        {
            _roomDisplays = serializedObject.FindProperty("_roomDisplays");
            _connections = serializedObject.FindProperty("_connections");
            _floor = serializedObject.FindProperty("_floor");
            _roomProfiles = serializedObject.FindProperty("_roomProfiles");
            _currentRoomTypeSelected = serializedObject.FindProperty("_currentRoomTypeSelected");
            _roomTypeColors = serializedObject.FindProperty("_roomTypeColors");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Open Window")) OpenWindow();
            EditorGUILayout.PropertyField(_roomDisplays);
            EditorGUILayout.PropertyField(_connections);
            EditorGUILayout.PropertyField(_roomProfiles);

            serializedObject.ApplyModifiedProperties();
        }

        private void OpenWindow()
        {
            FloorGeneratorWindow myWindow = EditorWindow.GetWindow(typeof(FloorGeneratorWindow)) as FloorGeneratorWindow;

            myWindow.InitializeWindow(target as FloorProfile); //fonction a créé
            myWindow.Show(); //fonction de base d'unity
        }

    }
}
