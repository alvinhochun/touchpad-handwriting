﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TouchPadHandwriting.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TouchPadHandwriting.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An instance of {0} is already running. Please check the system tray..
        /// </summary>
        internal static string InstanceAlreadyRunning {
            get {
                return ResourceManager.GetString("InstanceAlreadyRunning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No suitable handwriting recognizers system is found on your computer. This program will now terminate..
        /// </summary>
        internal static string NoSuitableRecognizers {
            get {
                return ResourceManager.GetString("NoSuitableRecognizers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Double-press the {0} key or press {1} to enable handwriting..
        /// </summary>
        internal static string TipReadyText {
            get {
                return ResourceManager.GetString("TipReadyText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to selected.
        /// </summary>
        internal static string TipReadyTextSelectedKey {
            get {
                return ResourceManager.GetString("TipReadyTextSelectedKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ready to try handwriting?.
        /// </summary>
        internal static string TipReadyTitle {
            get {
                return ResourceManager.GetString("TipReadyTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select a handwriting recognizer here..
        /// </summary>
        internal static string TipRecognizerText {
            get {
                return ResourceManager.GetString("TipRecognizerText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What language would you like to recognize?.
        /// </summary>
        internal static string TipRecognizerTitle {
            get {
                return ResourceManager.GetString("TipRecognizerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Now write on the TouchPad!.
        /// </summary>
        internal static string TipWriteText {
            get {
                return ResourceManager.GetString("TipWriteText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Write!.
        /// </summary>
        internal static string TipWriteTitle {
            get {
                return ResourceManager.GetString("TipWriteTitle", resourceCulture);
            }
        }
    }
}
