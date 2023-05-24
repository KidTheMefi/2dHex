using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using ScriptableScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DiceCubePrototype
{
    public enum DiceState
    {
        Default, Rolling, CanBeSelected, Used
    }
    public class DiceCube : MonoBehaviour
    {
        
        [SerializeField]
        private Rigidbody _rigidbody;

        private Vector3 _startPosition;
        [SerializeField]
        private List<CubeDiceFace> _cubeDiceFaces;
        private DiceState _diceState;
        public DiceState DiceState => _diceState; 

        public List<CubeDiceFace> CubeDiceFaces => _cubeDiceFaces;
        private CubeDiceFace _faceOnTop;

        private CancellationTokenSource _onDestroyCTS;
        public RuneScriptable RuneOnTop => _faceOnTop != null ? _faceOnTop.RuneScriptable : null;

        private Vector3 layPosition;


        public void Setup(CubeSetup cubeSetup, Vector3 localPosition)
        {
            transform.localPosition = localPosition;
            _startPosition = transform.position;
            _cubeDiceFaces = GetComponentsInChildren<CubeDiceFace>().ToList();
            
            if (cubeSetup == null) return;
            var setup = cubeSetup.GetCubeRuneScriptable();
            for (int i = 0; i < setup.Length; i++)
            {
                if (i < _cubeDiceFaces.Count)
                {
                    _cubeDiceFaces[i].SetRune(setup[i]);
                }
            }
        }
        public void BackToStartPosition()
        {
            _rigidbody.useGravity = false;
            _rigidbody.freezeRotation = true;
            RemoveVelocity();
            foreach (var face in _cubeDiceFaces)
            {
                face.ResetFace();
            }
            transform.position = _startPosition;
        }
        
        public async UniTask RollDice()
        {
            _diceState = DiceState.Rolling;
            _rigidbody.useGravity = false;
            _rigidbody.freezeRotation = false;
            RemoveVelocity();
            foreach (var face in _cubeDiceFaces)
            {
                face.ResetFace();
            }
            transform.position = _startPosition;
            //await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(0, 0.3f)));
            //Vector3 forceVector3 = new Vector3(Random.Range(0.5f, 2f), Random.Range(0, 1f), Random.Range(-0.5f, 0.5f));
            Vector3 forceVector3 = new Vector3(0, Random.Range(0, 1f), Random.Range(-0.5f, 0.5f));
            float forceVectorValue = 2;
            forceVector3 = Vector3.up *forceVectorValue + Vector3.forward*forceVectorValue*0.15f + Vector3.left*Random.Range(-0.1f, 0.1f);
            _rigidbody.AddForce(forceVector3 * 3, ForceMode.VelocityChange);
            float rollForceValue = 0.5f;
            _rigidbody.AddTorque(new Vector3(Random.Range(-rollForceValue, rollForceValue), Random.Range(-rollForceValue, rollForceValue), Random.Range(-rollForceValue, rollForceValue)), ForceMode.Impulse);
            _rigidbody.useGravity = true;
            await WaitingForRollEnd();
        }

        private void RemoveVelocity()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private async UniTask WaitingForRollEnd()
        {
            _onDestroyCTS = new CancellationTokenSource();
            await UniTask.Delay(TimeSpan.FromSeconds(0.2));
            await UniTask.WaitUntil(() => _rigidbody.angularVelocity == Vector3.zero && _rigidbody.velocity == Vector3.zero, cancellationToken: _onDestroyCTS.Token);

            if (_onDestroyCTS.IsCancellationRequested)
            {
                return;
            }
            CheckDiceFace();
        }

        private void CheckDiceFace()
        {
            var raycastHits = Physics.RaycastAll(transform.position, new Vector3(0,1,-1), 12f);
            foreach (var hit in raycastHits)
            {
                if (hit.collider.TryGetComponent<CubeDiceFace>(out var face))
                {
                    _faceOnTop = face;
                    SetCubeState(DiceState.CanBeSelected);
                    return;
                }
            }
        }
        
        public void SetCubeState(DiceState state)
        {
            _diceState = state;
            if (_diceState == DiceState.CanBeSelected)
            {
                _faceOnTop.FaceOnTop();
            }
            else
            {
                _faceOnTop.ResetFace();
            }
        }

        private void OnMouseDown()
        {
            if (_diceState == DiceState.CanBeSelected )
            {
                CubeDiceSignal.GetInstance().InvokeMouseDown(this);
            }
        }

        public void Selected(bool value)
        {
            _faceOnTop.Selected(value);
        }

        private void OnDestroy()
        {
            _onDestroyCTS?.Cancel();
        }
    }
}