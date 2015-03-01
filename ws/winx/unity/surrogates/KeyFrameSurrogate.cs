using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using BehaviourMachine;

namespace ws.winx.unity.surrogates{

	public class KeyFrameSurrogate : ISerializationSurrogate
	{
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			var vector = (Keyframe)obj;
		
			info.AddValue("t", vector.time);
			info.AddValue ("v", vector.value);

			//Debug.Log ("Keyframe t:" + vector.time + " v:" + vector.value);

		}
		
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			//Debug.Log ("Set Keyframe t:" +(float)info.GetValue("t",typeof(float))+ " v:" + (float)info.GetValue("v",typeof(float))
			//           +" obj:"+obj);



			Keyframe keyframe=(Keyframe)obj;
			keyframe.time=(float)info.GetValue("t",typeof(float));
			keyframe.value=(float)info.GetValue("v",typeof(float));


			return null;

			//return new Keyframe(,(float)info.GetValue("v",typeof(float)));
		}
	}
}
