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
using System.Collections.Generic;
using System.Linq;

using System.Collections;


namespace ws.winx.csharp.extensions
{
	public static class IArrayExtensions
	{

		public static String JoinToString<T>(this IEnumerable<T> array)
		{
			
			return string.Join (", ", array.Select<T,string> (a => a.ToString ()).ToArray ());
		}
	}
}

