// 
// (C) 2006-2007 The SharpOS Project Team (http://sharpos.sourceforge.net)
//
// Authors:
//	William Lahti <xfurious@gmail.com>
//
// Licensed under the terms of the GNU GPL v3,
//  with Classpath Linking Exception for Libraries
//

namespace SharpOS
{
	public enum KernelStage: uint {
		Init = 0,
		RuntimeInit,
		UserInit,
		Active,
		SingleUser,
		Stopping,
		Stop,
		Halt,
		
		Unknown = 0xFFFFFFFF
	}
}