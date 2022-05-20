using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    public class RoomGeneratorWindow : EditorWindow
    {
        SerializedObject _serializedObject;
        RoomProfile _currentRoom;


        SerializedProperty _roomID, _xLength, _yLength, _tiles, _tilesWeight, _currentTileSelected, _tilesColor;

        //editor window var
        private bool isMouseLeftDown;
        private bool isMouseLeftDownProcessed;
        private bool isMouseRightDown;
        private bool isMouseRightDownProcessed;


        public void InitializeWindow(RoomProfile correspondingLevel)
        {
            _currentRoom = correspondingLevel;
            _serializedObject = new SerializedObject(_currentRoom);

            _xLength = _serializedObject.FindProperty("_xLength");
            _yLength = _serializedObject.FindProperty("_yLength");
            _tiles = _serializedObject.FindProperty("_tiles");
            _tilesWeight = _serializedObject.FindProperty("_tilesWeight");
            _currentTileSelected = _serializedObject.FindProperty("_currentTileSelected");
            _tilesColor = _serializedObject.FindProperty("_tilesColor");

            isMouseLeftDown = false;
            isMouseRightDown = false;
            isMouseLeftDownProcessed = false;
            isMouseRightDownProcessed = false;
        }


        private void OnGUI()
        {
            _serializedObject.Update();

            ProcessEvents();

            //affichage de l'objet level actuel
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.EndDisabledGroup();

            //
            EditorGUILayout.Space(5);

            #region Chunk Selector

            _currentRoom._xLength = EditorGUILayout.IntField("Width", _currentRoom._xLength);

            EditorGUILayout.Space(10);

            _currentRoom._yLength = EditorGUILayout.IntField("Height", _currentRoom._yLength);

            //Sols brush :
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Ground Brush : " + _currentTileSelected.enumValueIndex + " => " + _currentTileSelected.enumNames[_currentTileSelected.enumValueIndex]);


            //changer de couleur
            if (GUILayout.Button("<<"))
            {
                //Debug.Log("Changer tile left");
                if (_currentTileSelected.enumValueIndex != 0) _currentTileSelected.enumValueIndex--;
                else _currentTileSelected.enumValueIndex = (int)TileManager.Tiles.total - 1;
            }
            if (GUILayout.Button(">>"))
            {
                //Debug.Log("Changer tile right");
                if (_currentTileSelected.enumValueIndex != (int)TileManager.Tiles.total - 1) _currentTileSelected.enumValueIndex++;
                else _currentTileSelected.enumValueIndex = 0;
            }


            EditorGUILayout.Space(10);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            // Liste

            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(_tilesColor);

            if (GUILayout.Button("Clear Grid"))
            {
                if (EditorUtility.DisplayDialog("Attention", "Etes vous sur de vouloir vider la grille ?", "Oui", "Non"))
                {
                    _tiles.ClearArray();
                    _tiles.arraySize = _currentRoom._xLength * _currentRoom._yLength;
                    _tilesWeight.ClearArray();
                    _tilesWeight.arraySize = _currentRoom._xLength * _currentRoom._yLength;
                    for (int i = 0; i < _tiles.arraySize; i++)
                    {
                        _tilesWeight.GetArrayElementAtIndex(i).intValue = 0;
                    }
                }
            }

            _serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(_currentRoom);

            if (_currentRoom._xLength < 0 || _currentRoom._yLength < 0) return;

            #endregion

            #region Level view

            //trad from Simon's function
            float marginRatio = 0.2f;
            float widthSpace = 3f;
            float heightSpace = widthSpace;

            EditorGUILayout.Space();
            Rect nextRect = EditorGUILayout.GetControlRect();
            if (nextRect.y == 0) return;

            // Define cell width to be responsive
            float sumSpaceWidth = widthSpace * _currentRoom._xLength;
            float totalWidth = position.width - sumSpaceWidth;
            float gridWidth = totalWidth * (1f - 2f * marginRatio);
            float cellWidth = gridWidth / _currentRoom._xLength;

            // Define cell height to be responsive
            float sumSpaceHeight = _currentRoom._yLength * heightSpace;
            float totalHeight = position.height - sumSpaceHeight - nextRect.y;
            float gridHeight = totalHeight * (1f - 2f * marginRatio);
            float cellHeight = gridHeight / _currentRoom._yLength;
            Rect gridMap = new Rect(totalWidth * marginRatio, nextRect.y + gridHeight * marginRatio, gridWidth, gridHeight);

            // Draw grid
            float curY = gridMap.y;
            for (int i = 0; i < _currentRoom._yLength; ++i)
            {
                float curX = gridMap.x;
                for (int j = 0; j < _currentRoom._xLength; ++j)
                {
                    Rect cell = new Rect(curX, curY, cellWidth, cellHeight);
                    int index = j * _currentRoom._yLength + i;

                    //detec if left mouse pressed
                    bool isPaintingOverThis = isMouseLeftDown && cell.Contains(Event.current.mousePosition);
                    if (isPaintingOverThis)
                    {
                        _tiles.GetArrayElementAtIndex(index).enumValueIndex = _currentTileSelected.enumValueIndex;
                        _tilesWeight.GetArrayElementAtIndex(index).intValue = 0;
                        isMouseLeftDownProcessed = true;
                    }
                    bool isChangingEntityCount = isMouseRightDown && !isMouseRightDownProcessed && cell.Contains(Event.current.mousePosition);
                    if (isChangingEntityCount)
                    {
                        _tilesWeight.GetArrayElementAtIndex(index).intValue++;
                        if (_tilesWeight.GetArrayElementAtIndex(index).intValue > 4)
                            _tilesWeight.GetArrayElementAtIndex(index).intValue = 0;

                        isMouseRightDownProcessed = true;
                    }


                    int enumIndexInPalette = _tiles.GetArrayElementAtIndex(index).enumValueIndex;
                    Color col = _tilesColor.GetArrayElementAtIndex(enumIndexInPalette).colorValue;
                    EditorGUI.DrawRect(cell, col);

                    //draw nb of entity
                    int nbOfEntity = _tilesWeight.GetArrayElementAtIndex(index).intValue;

                    if (nbOfEntity > 0)
                    {
                        if (nbOfEntity >= 4)
                        {
                            Rect r4d = new Rect(curX + (cellWidth / 8) + (cellWidth / 2), curY + (cellWidth / 8) + (cellHeight / 2), cellWidth / 4, cellHeight / 4);
                            EditorGUI.DrawRect(r4d, Color.black);
                        }
                        if (nbOfEntity >= 3)
                        {
                            Rect r3c = new Rect(curX + (cellWidth / 8), curY + (cellWidth / 8) + (cellHeight / 2), cellWidth / 4, cellHeight / 4);
                            EditorGUI.DrawRect(r3c, Color.black);
                        }
                        if (nbOfEntity >= 2)
                        {
                            Rect r2b = new Rect(curX + (cellWidth / 8) + (cellWidth / 2), curY + (cellWidth / 8), cellWidth / 4, cellHeight / 4);
                            EditorGUI.DrawRect(r2b, Color.black);
                        }
                        if (nbOfEntity >= 1)
                        {
                            Rect r1 = new Rect(curX + (cellWidth / 8), curY + (cellWidth / 8), cellWidth / 4, cellHeight / 4);
                            EditorGUI.DrawRect(r1, Color.black);
                        }
                    }

                    curX += cellWidth + widthSpace;
                }
                curY += cellHeight + heightSpace;
            }
            Repaint();
            EditorUtility.SetDirty(_currentRoom);

            #endregion


            _serializedObject.ApplyModifiedProperties();

            Repaint(); // pour la mouse_position (Event)
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && !isMouseLeftDownProcessed)
                isMouseLeftDown = true;
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                isMouseLeftDown = false;
                isMouseLeftDownProcessed = false;
            }

            if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && !isMouseRightDownProcessed)
                isMouseRightDown = true;
            if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
            {
                isMouseRightDown = false;
                isMouseRightDownProcessed = false;
            }
        }

    }
}
