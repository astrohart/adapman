﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace adapman.Properties {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("adapman.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must specify the SSID of the wifi network you want to connect to..
        /// </summary>
        public static string ConnectSSIDReq {
            get {
                return ResourceManager.GetString("ConnectSSIDReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Must specify the SSID of the wifi network you want to disconnect from..
        /// </summary>
        public static string DiisconnectSSIDReq {
            get {
                return ResourceManager.GetString("DiisconnectSSIDReq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage:
        ///   adapman [-da] [-ea] [-cw:ssid &quot;ssid&quot;] [-cw:pwd &quot;password&quot;] [-dw &quot;SSID&quot;]
        ///
        ///OPTIONS:
        ///    NOTE: Only one switch may be applied at any time, unless the &apos;cw&apos; switches are used, in which case,
        ///    both are required.
        ///
        ///    -da                Disables ALL of the network adapters installed on the system.
        ///    -ea                Enables ALL of the network adapters installed on the system.
        ///    -cw:ssid       Connects to the Wi-Fi network with the given SSID (The Wi-Fi adapter must be
        ///                  [rest of string was truncated]&quot;;.
        /// </summary>
        public static string UsageMessage {
            get {
                return ResourceManager.GetString("UsageMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &apos;value&apos; parameter must be non-blank..
        /// </summary>
        public static string ValueParamReq {
            get {
                return ResourceManager.GetString("ValueParamReq", resourceCulture);
            }
        }
    }
}
