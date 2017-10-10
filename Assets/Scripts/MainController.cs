    using System;
    using System.Collections;
    using UnityEngine;
 

    public class MainController : MonoBehaviour {

        #region Enums

        public enum TOGGLE_MODE : int
        {
            TARGET_OBJECT = KeyCode.F1,
            TARGET_CAMERA = KeyCode.F2
        }

        public enum KEYBOARD_INPUT : int {
            SPEED_MODIFIER = KeyCode.LeftShift,

            FORWARD_1 = KeyCode.W,
            BACKWARDS_1 = KeyCode.S,
            LEFT_STRAFE_1 = KeyCode.A,
            RIGHT_STRAFE_1 = KeyCode.D,

            FORWARD_2 = KeyCode.UpArrow,
            BACKWARDS_2 = KeyCode.DownArrow,
            LEFT_STRAFE_2 = KeyCode.LeftArrow,
            RIGHT_STRAFE_2 = KeyCode.RightArrow,

            LEFT_ROTATE = KeyCode.Q,
            RIGHT_ROTATE = KeyCode.E
        }

        /// <summary>
        ///     enable the rotation of the camera if there is right click
        /// </summary>
        public enum MOUSE_INPUT : int {
            PITCH_MOUSE = KeyCode.Mouse1,
            YAW_MOUSE = KeyCode.Mouse1
        }

        #endregion

        #region Members

        //TODO:
        public Transform TargetObject;

        [Range(1f, 20f)] public float ObjectRotationSpeed = 5f;
        [Range(1f, 20f)] public float CameraRotationThreshold = 0.5f;
        public TOGGLE_MODE ToggleMode = TOGGLE_MODE.TARGET_CAMERA;


        /// <summary>
        /// reference to the main camera
        /// </summary>
        private Transform g_Camera;

        /// <summary>
        /// Used for calculating the rotation of the mouse
        /// </summary>
        private Vector2 g_LastMousePosition;

         public float _edgeOfCursor = 100f;
        #endregion

        #region Unity_Functions

        public void Start() {
            //set target camera
            g_Camera = Camera.main.transform;
        }
    
        // Update is called once per frame
        void Update () {

            float speedMod = Input.GetKey((KeyCode)KEYBOARD_INPUT.SPEED_MODIFIER) ? 3f : 1f;

                 if (Input.GetKeyDown((KeyCode)TOGGLE_MODE.TARGET_OBJECT))
                {
                    ToggleMode = TOGGLE_MODE.TARGET_OBJECT;
                    Debug.Log("Object Selected");
                }
                else if (Input.GetKeyDown((KeyCode)TOGGLE_MODE.TARGET_CAMERA))
                {
                    ToggleMode = TOGGLE_MODE.TARGET_CAMERA;
                    Debug.Log("Camera Selected");
                }

                // Handle Mouse

                foreach (MOUSE_INPUT mi in Enum.GetValues(typeof(MOUSE_INPUT))) {
                    if(Input.GetKey((KeyCode)mi)) {
                        float horDelta = g_LastMousePosition.x - Input.mousePosition.x;
                        float verDelta = g_LastMousePosition.y - Input.mousePosition.y;

                        if (Mathf.Abs(horDelta) > CameraRotationThreshold) {

               
                            g_Camera.Rotate(Vector3.up, horDelta * speedMod * ObjectRotationSpeed * Time.deltaTime, Space.World);
                        }
                        if (Mathf.Abs(verDelta) > CameraRotationThreshold) {
                 
                            g_Camera.Rotate(g_Camera.right, -verDelta * speedMod * ObjectRotationSpeed * Time.deltaTime, Space.World);
                        }

                    }
                }
            
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Input.GetKeyDown(KeyCode.Mouse0) && Physics.Raycast(ray, out hit, _edgeOfCursor))
                {
                  if (hit.transform.CompareTag("Obstacle"))
                  {
                        TargetObject = hit.transform;
                  }
                }


            // Handle Keyboard

            if (ToggleMode == TOGGLE_MODE.TARGET_OBJECT && TargetObject != null)
            {
                foreach (KEYBOARD_INPUT val in Enum.GetValues(typeof(KEYBOARD_INPUT)))
                {
                    if (Input.GetKey((KeyCode)val))
                    {
                        switch (val)
                        {
                        case KEYBOARD_INPUT.FORWARD_2:

                            TargetObject.transform.position+= new Vector3(0f,0f,speedMod * ObjectRotationSpeed * Time.deltaTime);
                                break;

                        case KEYBOARD_INPUT.BACKWARDS_2:

                            TargetObject.transform.position += new Vector3(0f, 0f, -speedMod * ObjectRotationSpeed * Time.deltaTime);
                            break;

                        case KEYBOARD_INPUT.LEFT_STRAFE_2:

                            TargetObject.transform.position += new Vector3(-speedMod * ObjectRotationSpeed * Time.deltaTime,0f,0f);
                            break;

                        case KEYBOARD_INPUT.RIGHT_STRAFE_2:

                            TargetObject.transform.position += new Vector3(speedMod * ObjectRotationSpeed * Time.deltaTime, 0f, 0f);
                            break;
                        case KEYBOARD_INPUT.LEFT_ROTATE:
                            TargetObject.Rotate(Vector3.up, -(speedMod * Time.deltaTime * ObjectRotationSpeed), Space.World);
                            break;
                        case KEYBOARD_INPUT.RIGHT_ROTATE:
                            TargetObject.Rotate(Vector3.up, speedMod * Time.deltaTime * ObjectRotationSpeed, Space.World);
                            break;
                  

                        }
                    }

                }
            }
            else if( g_Camera != null) {
                    foreach (KEYBOARD_INPUT val in Enum.GetValues(typeof(KEYBOARD_INPUT))) {
                        if(Input.GetKey((KeyCode) val)) {
                            switch(val) {
                                case KEYBOARD_INPUT.FORWARD_1:
                                    g_Camera.position += (g_Camera.forward * Time.deltaTime * speedMod);
                                    break;
                                case KEYBOARD_INPUT.BACKWARDS_1:
                                    g_Camera.position -= (g_Camera.forward * Time.deltaTime * speedMod);
                                    break;
                                case KEYBOARD_INPUT.LEFT_STRAFE_1:
                                    g_Camera.position -= (g_Camera.right * Time.deltaTime * speedMod);
                                    break;
                                case KEYBOARD_INPUT.RIGHT_STRAFE_1:
                                    g_Camera.position += (g_Camera.right * Time.deltaTime * speedMod);
                                    break;
                                case KEYBOARD_INPUT.LEFT_ROTATE:
                                    g_Camera.Rotate(Vector3.up, -(speedMod * Time.deltaTime * ObjectRotationSpeed), Space.World);
                                    break;
                                case KEYBOARD_INPUT.RIGHT_ROTATE:
                                    g_Camera.Rotate(Vector3.up, speedMod * Time.deltaTime * ObjectRotationSpeed, Space.World);
                                    break;
                            }
                        }
                    }
                }

            //TODO:
            if (TargetObject != null) {
            
                    Debug.DrawRay(TargetObject.position, Vector3.forward, Color.blue);
                    Debug.DrawRay(TargetObject.position, Vector3.right, Color.red);
                    Debug.DrawRay(TargetObject.position, Vector3.up, Color.green);
           
            }

            g_LastMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        }



        #endregion


    }
