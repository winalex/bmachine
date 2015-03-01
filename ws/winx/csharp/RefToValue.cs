// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;


namespace ws.winx.csharp
{
	public class RefToValue<T>
	{
		private Func<T> getter;
		private Action<T> setter;
		public RefToValue(Func<T> getter, Action<T> setter)
		{
			this.getter = getter;
			this.setter = setter;
		}
		public T Value
		{
			get { return getter(); }
			set { setter(value); }
		}
	}



}
