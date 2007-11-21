// 
// (C) 2006-2007 The SharpOS Project Team (http://sharpos.sourceforge.net)
//
// Authors:
//	Mircea-Cristian Racasan <darx_kies@gmx.net>
//	William Lahti <xfurious@gmail.com>
//
// Licensed under the terms of the GNU GPL v3,
//  with Classpath Linking Exception for Libraries
//

using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using SharpOS.AOT.IR.Operands;

namespace SharpOS.AOT.IR.Instructions {
	[Serializable]
	public class Jump : SharpOS.AOT.IR.Instructions.Instruction {
		public Jump ()
			: base (null)
		{
		}

		public Jump (Operands.Boolean condition)
			: base (condition)
		{
		}

		/// <summary>
		/// Dumps the instruction.
		/// </summary>
		/// <param name="dumpProcessor">The dump processor.</param>
		public override void Dump (DumpProcessor dumpProcessor)
		{
			if (this.Value != null)
				dumpProcessor.AddElement ("jump", this.FormatedIndex + "Jump " + this.Value.ToString (), true, true, false);
			
			else
				dumpProcessor.AddElement ("jump", this.FormatedIndex + "Jump", true, true, false);
		}
	}
}