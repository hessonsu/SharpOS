// 
// (C) 2006-2007 The SharpOS Project Team (http://sharpos.sourceforge.net)
//
// Authors:
//	Mircea-Cristian Racasan <darx_kies@gmx.net>
//
// Licensed under the terms of the GNU GPL v3,
//  with Classpath Linking Exception for Libraries
//

using System;
using System.Text;

namespace SharpOS.AOT.Attributes {
	[AttributeUsage (AttributeTargets.Method)]
	public sealed class LabelAttribute : Attribute {
		public LabelAttribute (string label)
		{
		}
	}
}
