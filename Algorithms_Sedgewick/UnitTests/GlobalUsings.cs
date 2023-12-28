#pragma warning disable SA1200
global using System;
#pragma warning restore SA1200

global using NUnit.Framework;

/*	This is so that I can still say Assert.IsTrue etc. instead of ClassicAssert.IsTrue etc. after upgrading NUnit. 
	TODO: change to use the new Assert.That(value, Is.True) syntax.
*/
global using Assert = NUnit.Framework.Legacy.ClassicAssert;
