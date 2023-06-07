using System;
using UnityEngine;

namespace GameEvents.MapObjectDescriptionSignal
{

    public interface IDescriptionSignalInvoker
    {
        public void HideDescriptionInvoke();
        public void ShowDescriptionInvoke(MapObjectDescriptionSignal.DescriptionStruct description);
    }
    
    public interface IDescriptionSignal
    {
        public event Action<MapObjectDescriptionSignal.DescriptionStruct> ShowDescription;
        public event Action HideDescription;
    }
    
    public class MapObjectDescriptionSignal : IDescriptionSignal, IDescriptionSignalInvoker
    {
        public event Action<DescriptionStruct> ShowDescription = delegate(DescriptionStruct description) { };
        public event Action HideDescription = delegate { };

        
        public void HideDescriptionInvoke()
        {
            HideDescription.Invoke();
        }
        
        void IDescriptionSignalInvoker.ShowDescriptionInvoke(DescriptionStruct descriptionStruct)
        {
            ShowDescription.Invoke(descriptionStruct);
        }
        
        
        public struct DescriptionStruct
        {
            public string name { get; }
            public string description { get;  }
            public Sprite spritePicture { get; }
            
            
            public DescriptionStruct(string name, string description, Sprite spritePicture)
            {
                this.name = name;
                this.description = description;
                this.spritePicture = spritePicture;
            }
        }
    }
}