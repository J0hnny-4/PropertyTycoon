using System;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace UI.Board
{
    public class MainCamera : MonoBehaviour
    {
        public Camera _mainCamera;
        private bool _isFollowing;
        private char character;

        ///<summary>
        ///When a key is pressed, it assings a charater
        ///to the key press 
        ///</summary>
        public void OnKey(InputAction.CallbackContext ctx)
        {
            ctx.ReadValue<float>();
            _isFollowing = ctx.performed || ctx.started;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                character = 'l';
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                character = 'r';
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                character = 'u';
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                character = 'd';
            }
            else if (Input.GetKeyDown(KeyCode.PageDown))
            {
                character = 'p';
            }
            else if (Input.GetKeyDown(KeyCode.PageUp))
            {
                character = 'c';
            }



        }

        ///<summary>
        ///Assings the key that was pressed and moves the cameras
        ///position in the game. 
        ///</summary> 
        private void LateUpdate()
        {
            if (!_isFollowing) return;

            if (character == 'l')
            {
                _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x - 1,
                    _mainCamera.transform.position.y, _mainCamera.transform.position.z);
            }

            else if (character == 'r')
            {
                _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x + 1,
                    _mainCamera.transform.position.y, _mainCamera.transform.position.z);
            }
            else if (character == 'u')
            {
                _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, _mainCamera.transform.position.z + 1);
            }
            else if (character == 'd')
            {
                _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y, _mainCamera.transform.position.z - 1);
            }
            else if (character == 'p')
            {
                _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y - 1, _mainCamera.transform.position.z);
            }
            else if (character == 'c')
            {
                _mainCamera.transform.position = new Vector3(_mainCamera.transform.position.x, _mainCamera.transform.position.y + 1, _mainCamera.transform.position.z);
            }
        }




    }
}

