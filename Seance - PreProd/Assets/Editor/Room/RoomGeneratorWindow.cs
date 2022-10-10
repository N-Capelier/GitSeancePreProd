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


        SerializedProperty _roomID, _xLength, _yLength, _tiles, _tilesWeight, _currentTileSelected, _tilesColor, _tileRotationSave, _otherTileRotationSave, _newTileRotation, _newOtherTileRotation;

        //editor window var
        private bool isMouseLeftDown;
        private bool isMouseLeftDownProcessed;
        private bool isMouseRightDown;
        private bool isMouseRightDownProcessed;
        private bool isEButtontDown;
        private bool isEButtontDownProcessed;

        private bool isRotationLayerActive;


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

            _tileRotationSave = _serializedObject.FindProperty("_tileRotationSave");
            _otherTileRotationSave = _serializedObject.FindProperty("_otherTileRotationSave");
            _newTileRotation = _serializedObject.FindProperty("_newTileRotation");
            _newOtherTileRotation = _serializedObject.FindProperty("_newOtherTileRotation");

            isMouseLeftDown = false;
            isMouseRightDown = false;
            isMouseLeftDownProcessed = false;
            isMouseRightDownProcessed = false;

            isRotationLayerActive = true;

        }


        private void OnGUI()
        {
            _serializedObject.Update();

            ProcessEvents();

            //affichage de l'objet level actuel
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.EndDisabledGroup();

            //for rotation save
            /*if (_tileRotationSave.arraySize < 1)
            {
                _tileRotationSave.arraySize = _currentRoom._xLength * _currentRoom._yLength;
            }
            if (_otherTileRotationSave.arraySize < 1)
            {
                _otherTileRotationSave.arraySize = _currentRoom._xLength * _currentRoom._yLength;
            }*/

            if (_tileRotationSave.arraySize < 1)
            {
                Debug.Log("initialise save list");
                _tileRotationSave.arraySize = _xLength.intValue * _yLength.intValue;
                _otherTileRotationSave.arraySize = _xLength.intValue * _yLength.intValue;
            }
            if (_newTileRotation.arraySize < 1)
            {
                Debug.Log("initialise new save list");
                _newTileRotation.arraySize = _xLength.intValue * _yLength.intValue;
                _newOtherTileRotation.arraySize = _xLength.intValue * _yLength.intValue;
                for (int i = 0; i < _newTileRotation.arraySize; i++)
                {
                    _newTileRotation.GetArrayElementAtIndex(i).intValue = 4;
                    _newOtherTileRotation.GetArrayElementAtIndex(i).intValue = 4;
                }
            }

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

                    //for rotation save
                    _newTileRotation.arraySize = _xLength.intValue * _yLength.intValue;
                    _newOtherTileRotation.arraySize = _xLength.intValue * _yLength.intValue;
                    for (int i = 0; i < _newTileRotation.arraySize; i++)
                    {
                        _newTileRotation.GetArrayElementAtIndex(i).intValue = 0;
                        _newOtherTileRotation.GetArrayElementAtIndex(i).intValue = 4;
                    }

                }
            }

            EditorGUILayout.Space(5);

            isRotationLayerActive = EditorGUILayout.Toggle("Show rotation layer", isRotationLayerActive);

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

                        //rotation
                        if (_currentTileSelected.enumValueIndex == (int)TileManager.Tiles.basicTile)
                        {
                            _newTileRotation.GetArrayElementAtIndex(index).intValue = 0;
                            _newOtherTileRotation.GetArrayElementAtIndex(index).intValue = 4;
                        }
                        else if (_currentTileSelected.enumValueIndex == (int)TileManager.Tiles.empty)
                        {
                            _newTileRotation.GetArrayElementAtIndex(index).intValue = 4;
                            _newOtherTileRotation.GetArrayElementAtIndex(index).intValue = 4;
                        }
                        else
                        {
                            _newTileRotation.GetArrayElementAtIndex(index).intValue = 0;
                            _newOtherTileRotation.GetArrayElementAtIndex(index).intValue = 0;
                        }

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

                    bool isRotatingtTile = isRotationLayerActive && isEButtontDown && !isEButtontDownProcessed && cell.Contains(Event.current.mousePosition);
                    if (isRotatingtTile)
                    {
                        /*_tilesWeight.GetArrayElementAtIndex(index).intValue++;
                        if (_tilesWeight.GetArrayElementAtIndex(index).intValue > 4)
                            _tilesWeight.GetArrayElementAtIndex(index).intValue = 0;*/

                        //values in order: NESO = 0123 ; 4 = null

                        if (_tiles.GetArrayElementAtIndex(index).enumValueIndex == (int)TileManager.Tiles.basicTile)
                        {
                            //for ground: _tileRotationSave
                            _newTileRotation.GetArrayElementAtIndex(index).intValue++;
                            if (_newTileRotation.GetArrayElementAtIndex(index).intValue > 3)
                                _newTileRotation.GetArrayElementAtIndex(index).intValue = 0;

                            _newOtherTileRotation.GetArrayElementAtIndex(index).intValue = 4;

                        }
                        else if (_tiles.GetArrayElementAtIndex(index).enumValueIndex == (int)TileManager.Tiles.empty)
                        {
                            //nothing to handle
                            _newTileRotation.GetArrayElementAtIndex(index).intValue = 4;
                            _newOtherTileRotation.GetArrayElementAtIndex(index).intValue = 4;
                        }
                        else
                        {
                            //for wall: _otherTileRotationSave
                            _newTileRotation.GetArrayElementAtIndex(index).intValue = 4;

                            _newOtherTileRotation.GetArrayElementAtIndex(index).intValue++;
                            if (_newOtherTileRotation.GetArrayElementAtIndex(index).intValue > 3)
                                _newOtherTileRotation.GetArrayElementAtIndex(index).intValue = 0;

                        }
                        isEButtontDownProcessed = true;
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

                    //draw rotation display
                    if (isRotationLayerActive)
                    {

                        if (_tiles.GetArrayElementAtIndex(index).enumValueIndex == (int)TileManager.Tiles.basicTile)
                        {

                            switch (_newTileRotation.GetArrayElementAtIndex(index).intValue)
                            {
                                case 0:
                                    Rect rotRec = new Rect(curX + (cellWidth * 0.4f), curY, cellWidth * 0.2f, cellHeight * 0.1f);
                                    EditorGUI.DrawRect(rotRec, Color.white);
                                    break;
                                case 1:
                                    Rect rotRec2 = new Rect(curX + (cellWidth * 0.9f), curY + (cellHeight * 0.4f), cellWidth * 0.1f, cellHeight * 0.2f);
                                    EditorGUI.DrawRect(rotRec2, Color.white);
                                    break;
                                case 2:
                                    Rect rotRec3 = new Rect(curX + (cellWidth * 0.4f), curY + (cellHeight * 0.9f), cellWidth * 0.2f, cellHeight * 0.1f);
                                    EditorGUI.DrawRect(rotRec3, Color.white);
                                    break;
                                case 3:
                                    Rect rotRec4 = new Rect(curX, curY + (cellHeight * 0.4f), cellWidth * 0.1f, cellHeight * 0.2f);
                                    EditorGUI.DrawRect(rotRec4, Color.white);
                                    break;
                            }

                        }
                        else if (_tiles.GetArrayElementAtIndex(index).enumValueIndex == (int)TileManager.Tiles.empty)
                        {
                            //nothing to display
                        }
                        else if (_tiles.GetArrayElementAtIndex(index).enumValueIndex == (int)TileManager.Tiles.wall ||
                                _tiles.GetArrayElementAtIndex(index).enumValueIndex == (int)TileManager.Tiles.door)
                        {
                            switch (_newOtherTileRotation.GetArrayElementAtIndex(index).intValue)
                            {
                                case 0:
                                    Rect rotRec = new Rect(curX, curY + (cellHeight * 0.4f), cellWidth, cellHeight * 0.2f);
                                    EditorGUI.DrawRect(rotRec, Color.gray);
                                    break;
                                case 1:
                                    Rect rotRec2 = new Rect(curX + (cellWidth * 0.4f), curY, cellWidth * 0.2f, cellHeight);
                                    EditorGUI.DrawRect(rotRec2, Color.gray);

                                    break;
                                case 2:
                                    Rect rotRec3 = new Rect(curX, curY + (cellHeight * 0.4f), cellWidth, cellHeight * 0.2f);
                                    EditorGUI.DrawRect(rotRec3, Color.gray);
                                    break;
                                case 3:
                                    Rect rotRec4 = new Rect(curX + (cellWidth * 0.4f), curY, cellWidth * 0.2f, cellHeight);
                                    EditorGUI.DrawRect(rotRec4, Color.gray);
                                    break;
                            }
                        }
                        else if (_tiles.GetArrayElementAtIndex(index).enumValueIndex == (int)TileManager.Tiles.angle)
                        {
                            switch (_newOtherTileRotation.GetArrayElementAtIndex(index).intValue)
                            {
                                case 0:
                                    Rect rotRec11 = new Rect(curX, curY + (cellHeight * 0.4f), cellWidth * 0.6f, cellHeight * 0.2f);
                                    Rect rotRec12 = new Rect(curX + (cellWidth * 0.4f), curY + (cellHeight * 0.4f), cellWidth * 0.2f, cellHeight * 0.6f);
                                    EditorGUI.DrawRect(rotRec11, Color.gray);
                                    EditorGUI.DrawRect(rotRec12, Color.gray);
                                    break;
                                case 1:
                                    Rect rotRec21 = new Rect(curX, curY + (cellHeight * 0.4f), cellWidth * 0.6f, cellHeight * 0.2f);
                                    Rect rotRec22 = new Rect(curX + (cellWidth * 0.4f), curY, cellWidth * 0.2f, cellHeight * 0.6f);
                                    EditorGUI.DrawRect(rotRec21, Color.gray);
                                    EditorGUI.DrawRect(rotRec22, Color.gray);
                                    break;
                                case 2:
                                    Rect rotRec31 = new Rect(curX + (cellWidth * 0.4f), curY, cellWidth * 0.2f, cellHeight * 0.6f);
                                    Rect rotRec32 = new Rect(curX + (cellWidth * 0.4f), curY + (cellHeight * 0.4f), cellWidth * 0.6f, cellHeight * 0.2f);
                                    EditorGUI.DrawRect(rotRec31, Color.gray);
                                    EditorGUI.DrawRect(rotRec32, Color.gray);
                                    break;
                                case 3:
                                    Rect rotRec41 = new Rect(curX + (cellWidth * 0.4f), curY + (cellHeight * 0.4f), cellWidth * 0.2f, cellHeight * 0.6f);
                                    Rect rotRec42 = new Rect(curX + (cellWidth * 0.4f), curY + (cellHeight * 0.4f), cellWidth * 0.6f, cellHeight * 0.2f);
                                    EditorGUI.DrawRect(rotRec41, Color.gray);
                                    EditorGUI.DrawRect(rotRec42, Color.gray);
                                    break;
                            }
                        }
                        else
                        {

                            switch (_newOtherTileRotation.GetArrayElementAtIndex(index).intValue)
                            {
                                case 0:
                                    Rect rotRec = new Rect(curX + (cellWidth * 0.4f), curY, cellWidth * 0.2f, cellHeight * 0.1f);
                                    EditorGUI.DrawRect(rotRec, Color.white);
                                    break;
                                case 1:
                                    Rect rotRec2 = new Rect(curX + (cellWidth * 0.9f), curY + (cellHeight * 0.4f), cellWidth * 0.1f, cellHeight * 0.2f);
                                    EditorGUI.DrawRect(rotRec2, Color.white);

                                    break;
                                case 2:
                                    Rect rotRec3 = new Rect(curX + (cellWidth * 0.4f), curY + (cellHeight * 0.9f), cellWidth * 0.2f, cellHeight * 0.1f);
                                    EditorGUI.DrawRect(rotRec3, Color.white);
                                    break;
                                case 3:
                                    Rect rotRec4 = new Rect(curX, curY + (cellHeight * 0.4f), cellWidth * 0.1f, cellHeight * 0.2f);
                                    EditorGUI.DrawRect(rotRec4, Color.white);
                                    break;
                            }

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

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E && !isEButtontDownProcessed)
                isEButtontDown = true;
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.E)
            {
                isEButtontDown = false;
                isEButtontDownProcessed = false;
            }


        }

    }
}
