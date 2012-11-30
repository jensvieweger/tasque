// 
// NativeApplication.cs
//  
// Author:
//       Antonius Riha <antoniusriha@gmail.com>
// 
// Copyright (c) 2012 Antonius Riha
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Diagnostics;

namespace Tasque
{
	public abstract class NativeApplication : INativeApplication
	{
		public abstract string ConfDir { get; }

		public void Exit (int exitcode)
		{
			OnExit (exitcode);

			if (Exiting != null)
				Exiting (this, EventArgs.Empty);

			Environment.Exit (exitcode);
		}

		public virtual void Initialize (string[] args)
		{
			if (IsRemoteInstanceRunning ()) {
				Logger.Info ("Another instance of Tasque is already running.");
				Exit (0);
			}
			
			RemoteInstanceKnocked += delegate { ShowMainWindow (); };
		}

		public virtual void InitializeIdle () {}

		protected virtual void OnExit (int exitCode) {}

		public virtual void OpenUrl (string url)
		{
			Process.Start (url);
		}

		public abstract void QuitMainLoop ();

		public abstract void StartMainLoop ();

		public event EventHandler Exiting;

		/// <summary>
		/// Determines whether this a remote instance is running.
		/// </summary>
		/// <returns>
		/// <c>true</c> if a remote instance is running; otherwise, <c>false</c>.
		/// </returns>
		protected abstract bool IsRemoteInstanceRunning ();

		protected abstract void ShowMainWindow ();
		
		protected abstract event EventHandler RemoteInstanceKnocked;
		
		#region IDisposable implementation
		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		protected virtual void Dispose (bool disposing) {}
		
		~NativeApplication ()
		{
			Dispose (false);
		}
		#endregion
	}
}
