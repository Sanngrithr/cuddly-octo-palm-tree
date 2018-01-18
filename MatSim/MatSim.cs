using System;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.Core.GUI;

namespace FuseeApp
{

    [FuseeApplication(Name = "Matsim", Description = "Yet another FUSEE App.")]
    public class Matsim : RenderCanvas
    {
        // Horizontal and vertical rotation Angles for the displayed object
        private static float _angleHorz = M.PiOver4, _angleVert;
        
        // Horizontal and vertical angular speed
        private static float _angleVelHorz, _angleVelVert;

        // Overall speed factor. Change this to adjust how fast the rotation reacts to input
        private const float RotationSpeed = 7;

        // Damping factor
        private const float Damping = 0.8f;

        private SceneContainer _worldScene;
        private SceneRenderer _sceneRenderer;
        private bool _keys;



        private float3 _targetPoint;
        private float3 _startPoint;
        private float _angle;
        private float _staticAngle;
        public float _maxframes;
        private float _counter;

        private TransformComponent _ufoTransform;
        private TransformComponent _earthTransform;

        private float phi_start;
        private float phi_end;
        private float phi_delta;
        private float theta_start;
        private float theta_end;
        private Vector3D _startVector;
        private Vector3D _targetVector;
        private float3 _uxv;
        private float degtest;



        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.2f, 0.2f, 0.2f, 1);
            
            // Load the World model
            _worldScene = AssetStorage.Get<SceneContainer>("Earth2.fus");

            _ufoTransform = _worldScene.Children.FindNodes(node => node.Name == "UFO")?.FirstOrDefault()?.GetTransform();
            _earthTransform = _worldScene.Children.FindNodes(node => node.Name == "Earth")?.FirstOrDefault()?.GetTransform();

            _earthTransform.Scale = new float3(12, 12, 12);

//Lösungsansatz Translation
            /*_targetPoint = new float3(0, 0, -1);
            _startPoint = new float3(0, 1, 0);
            _ufoTransform.Translation = new float3(0,1,0);

            _uxv = new float3(0,0,0);
            _uxv.x = (_startPoint.y * _targetPoint.z) - (_startPoint.z * _targetPoint.y);
            _uxv.y = (_startPoint.z * _targetPoint.x) - (_startPoint.x * _targetPoint.z);
            _uxv.z = (_startPoint.x * _targetPoint.y) - (_startPoint.y * _targetPoint.x);

            _maxframes = 380;
            _angle = (M.Pi/2)/_maxframes;
            _staticAngle = (M.Pi/2)/_maxframes;
            _counter = 0;
            xplaceholder = new float3(0,0,0);
            yplaceholder = new float3(0,0,0);
            zplaceholder = new float3(0,0,0);
            */
            // Wrap a SceneRenderer around the model.


//Lösungsansatz Rotation
            theta_start = 0;
            phi_start = M.Pi/4;

            theta_end = M.Pi/2;
            phi_end = M.Pi;
            
            phi_delta = phi_end - phi_start;

            _startVector = new Vector3D(theta_start, phi_start);
            _targetVector = new Vector3D(theta_end, phi_end);

            _startPoint = new float3(_startVector.x, _startVector.y, _startVector.z);
            _targetPoint = new float3(_targetVector.x, _targetVector.y, _targetVector.z);

            //Bestimme den Winkel zwischen Start- und Zielvektor in Bogenmaß
            _maxframes = 300;
            _staticAngle = _startVector.angleRad(_targetVector)/_maxframes;
            _angle = 0;

            _ufoTransform.Translation = _startPoint;

            _sceneRenderer = new SceneRenderer(_worldScene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Mouse and keyboard movement
            if (Keyboard.LeftRightAxis != 0 || Keyboard.UpDownAxis != 0)
            {
                _keys = true;
            }

            if (Mouse.LeftButton)
            {
                _keys = false;
                _angleVelHorz = -RotationSpeed * Mouse.XVel * DeltaTime * 0.0005f;
                _angleVelVert = -RotationSpeed * Mouse.YVel * DeltaTime * 0.0005f;
            }
            else if (Touch.GetTouchActive(TouchPoints.Touchpoint_0))
            {
                _keys = false;
                var touchVel = Touch.GetVelocity(TouchPoints.Touchpoint_0);
                _angleVelHorz = -RotationSpeed * touchVel.x * DeltaTime * 0.0005f;
                _angleVelVert = -RotationSpeed * touchVel.y * DeltaTime * 0.0005f;
            }
            else
            {
                if (_keys)
                {
                    _angleVelHorz = -RotationSpeed * Keyboard.LeftRightAxis * DeltaTime;
                    _angleVelVert = -RotationSpeed * Keyboard.UpDownAxis * DeltaTime;
                }
                else
                {
                    var curDamp = (float)System.Math.Exp(-Damping * DeltaTime);
                    _angleVelHorz *= curDamp;
                    _angleVelVert *= curDamp;
                }
            }

            _angleHorz += _angleVelHorz;
            _angleVert += _angleVelVert;

            // Create the camera matrix and set it as the current ModelView transformation
            var mtxRot = float4x4.CreateRotationX(_angleVert) * float4x4.CreateRotationY(_angleHorz);
            var mtxCam = float4x4.LookAt(0, 20, Mouse.Wheel * 5 - 300, 0, 0, 0, 0, 1, 0);
            RC.ModelView = mtxCam * mtxRot;



//Lösungsansatz Translation
           /* _ufoTransform.Translation = new float3(
                (_ufoTransform.Translation.x +
                    ((_uxv.x * _uxv.x) + M.Cos(_angle) * (1 - (_uxv.x * _uxv.x))) +
                    (_uxv.x * _uxv.y * (1 - M.Cos(_angle)) + _uxv.z * M.Sin(_angle)) +
                    (_uxv.x * _uxv.z * (1 - M.Cos(_angle)) - _uxv.y * M.Sin(_angle))
                    ),
                (_ufoTransform.Translation.y +
                    (_uxv.x * _uxv.y * (1 - M.Cos(_angle)) - _uxv.z * M.Sin(_angle)) +
                    ((_uxv.y * _uxv.y) + M.Cos(_angle) * (1 - (_uxv.y * _uxv.y))) +
                    (_uxv.y * _uxv.z * (1 - M.Cos(_angle)) + _uxv.x * M.Sin(_angle))
                    ),
                 (_ufoTransform.Translation.z +
                    (_uxv.x * _uxv.z * (1 - M.Cos(_angle)) + _uxv.y * M.Sin(_angle)) +
                    (_uxv.y * _uxv.z * (1 - M.Cos(_angle)) - _uxv.x * M.Sin(_angle)) +
                    ((_uxv.z * _uxv.z) + M.Cos(_angle) * (1 - (_uxv.z * _uxv.z)))
                    )
            );
            */
           /* xplaceholder = new float3(
                    (_uxv.x * _uxv.y * (1 - M.Cos(_angle)) - _uxv.z * M.Sin(_angle)),
                    ((_uxv.y * _uxv.y) + M.Cos(_angle) * (1 - (_uxv.y * _uxv.y))),
                    (_uxv.y * _uxv.z * (1 - M.Cos(_angle)) + _uxv.x * M.Sin(_angle))
                    );

            yplaceholder = new float3(
                    (_uxv.x * _uxv.z * (1 - M.Cos(_angle)) + _uxv.y * M.Sin(_angle)),
                    (_uxv.y * _uxv.z * (1 - M.Cos(_angle)) - _uxv.x * M.Sin(_angle)),
                    ((_uxv.z * _uxv.z) + M.Cos(_angle) * (1 - (_uxv.z * _uxv.z)))
                    );
            zplaceholder = new float3(
                    ((_uxv.x * _uxv.x) + M.Cos(_angle) * (1 - (_uxv.x * _uxv.x))),
                    (_uxv.x * _uxv.y * (1 - M.Cos(_angle)) + _uxv.z * M.Sin(_angle)),
                    (_uxv.x * _uxv.z * (1 - M.Cos(_angle)) - _uxv.y * M.Sin(_angle))
                    );

            xplaceholder.Normalize();
            yplaceholder.Normalize();
            zplaceholder.Normalize();
            _ufoTransform.Translation = new float3(
                xplaceholder.x + xplaceholder.y + xplaceholder.z,
                yplaceholder.x + yplaceholder.y + yplaceholder.z,
                -(zplaceholder.x + zplaceholder.y + zplaceholder.z)
            );

            _ufoTransform.Translation.Normalize();
            
            if(_counter <= _maxframes){
                _angle = _angle + _staticAngle;
                _counter++;
            };
            */

            if(_counter <= _maxframes)
            {
                
                //Rotiere den Start-Vektor auf die z-y-Ebene
                _startPoint = new float3(_startPoint.x * M.Cos(phi_start) + _startPoint.z * (M.Sin(-(phi_start))),
                                         _startPoint.y,
                                         _startPoint.x * M.Sin(phi_start) + _startPoint.z * M.Cos(phi_start)
                                        );

                //Rotiere den Start-Vektor auf die z-x-Ebene
                _startPoint = new float3(_startPoint.x,
                                         _startPoint.y * M.Cos(theta_start) + _startPoint.z * M.Sin(theta_start),
                                         _startPoint.y * M.Sin(-(theta_start)) + _startPoint.z * M.Cos(theta_start)
                                        );
                //Rotiere den Ziel Vektor auf die z-x-Ebene
                _startPoint = new float3(_startPoint.x * M.Cos(phi_delta) + _startPoint.y * M.Sin(phi_delta),
                                         _startPoint.x * M.Sin(-(phi_delta)) + _startPoint.y * M.Cos(phi_delta),
                                         _startPoint.z
                                        );
                //Rotiere den Start-Vektor um die y-Achse in Richtung des Zielvektors in Inkrementen
                _startPoint = new float3(_startPoint.x * M.Cos(_angle) + _startPoint.z * (M.Sin(-(_angle))),
                                         _startPoint.y,
                                         _startPoint.x * M.Sin(_angle) + _startPoint.z * M.Cos(_angle)
                                        );
                //Rotationen auf die Drehachsen werden wieder aufgehoben
                _startPoint = new float3(_startPoint.x * M.Cos(-(phi_delta)) + _startPoint.y * M.Sin(-(phi_delta)),
                                         _startPoint.x * M.Sin(phi_delta) + _startPoint.y * M.Cos(-(phi_delta)),
                                         _startPoint.z
                                        );
                
                _startPoint = new float3(_startPoint.x,
                                         _startPoint.y * M.Cos(-(theta_start)) + _startPoint.z * M.Sin(-(theta_start)),
                                         _startPoint.y * M.Sin(theta_start) + _startPoint.z * M.Cos(-(theta_start))
                                        );

                _startPoint = new float3(_startPoint.x * M.Cos(-(phi_start)) + _startPoint.z * (M.Sin(phi_start)),
                                         _startPoint.y,
                                         _startPoint.x * M.Sin(-(phi_start)) + _startPoint.z * M.Cos(-(phi_start))
                                        );

                //Position des Ufo's wird aktualisiert
                _ufoTransform.Translation = new float3(_startPoint.x, _startPoint.y, _startPoint.z);
                
/*
                _startVector.rotatate(_angle, _uxv);
                _ufoTransform.Translation = new float3(_startVector.x, _startVector.y, _startVector.z);
                _ufoTransform.Translation.Normalize();
*/              
                _angle = _angle + _staticAngle;
                //Frame-Zähler wird erhöht
                _counter++;
            };

            // Render the scene loaded in Init()
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }
        private InputDevice Creator(IInputDeviceImp device)
        {
            throw new NotImplementedException();
        }

        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width/(float) Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}