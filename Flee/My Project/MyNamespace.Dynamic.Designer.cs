using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Flee.My {
	internal static partial class MyProject {
		internal partial class MyForms {
			[EditorBrowsable(EditorBrowsableState.Never)]
			public GameForm m_MainForm;

			public GameForm MainForm {
				[DebuggerHidden]
				get {
					m_MainForm = Create__Instance__(m_MainForm);
					return m_MainForm;
				}

				[DebuggerHidden]
				set {
					if (ReferenceEquals(value, m_MainForm))
						return;
					if (value is object)
						throw new ArgumentException("Property can only be set to Nothing");
					Dispose__Instance__(ref m_MainForm);
				}
			}
		}
	}
}