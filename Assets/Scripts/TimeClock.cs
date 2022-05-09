using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TimeClock : MonoBehaviour
{
   [SerializeField]
   private Transform _arrow;

   public async UniTask Tick()
   {
      var targetRotation = _arrow.eulerAngles + Vector3.back * 30;
      if (targetRotation.z <= -360)
      {
         targetRotation.z = 0;
      }
      await _arrow.DORotateQuaternion(Quaternion.Euler(targetRotation), GameTime.MovementTimeModificator).SetEase(Ease.Linear);
   }
}
