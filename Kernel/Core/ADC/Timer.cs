// created on 7/17/2007 at 4:10 PM
//
// (C) 2006-2007 The SharpOS Project Team (http://sharpos.sourceforge.net)
//
// Authors:
//	William Lahti <xfurious@gmail.com>
//
// Licensed under the terms of the GNU GPL v3,
//  with Classpath Linking Exception for Libraries
//

using SharpOS.AOT;
using AOTAttr = SharpOS.AOT.Attributes;

namespace SharpOS.ADC {

	public unsafe class Timer {
		[AOTAttr.ADCStub]
		public static EventRegisterStatus RegisterTimerEvent (uint func)
		{
			return EventRegisterStatus.NotSupported;
		}
		
		[AOTAttr.ADCStub]
		public static ushort GetFrequency ()
		{
			return 0;
		}
	}
}
