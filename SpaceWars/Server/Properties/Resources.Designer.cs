﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Server.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Server.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;SpaceSettings&gt;
        ///  &lt;UniverseSize&gt;750&lt;/UniverseSize&gt;
        ///  &lt;MSPerFrame&gt;16&lt;/MSPerFrame&gt;
        ///  &lt;EngineStrength&gt;0.08&lt;/EngineStrength&gt;
        ///  &lt;ProjectileSpeed&gt;15&lt;/ProjectileSpeed&gt;
        ///  &lt;ShotDelay&gt;6&lt;/ShotDelay&gt;&lt;!--This is in framse--&gt;
        ///  &lt;RespawnRate&gt;300&lt;/RespawnRate&gt;&lt;!--This is in frames--&gt;
        ///  &lt;StarSize&gt;35&lt;/StarSize&gt;
        ///  &lt;ShipSize&gt;20&lt;/ShipSize&gt;
        ///
        ///  &lt;Star&gt;
        ///    &lt;x&gt;0&lt;/x&gt;
        ///    &lt;y&gt;0&lt;/y&gt;
        ///    &lt;mass&gt;0.01&lt;/mass&gt;
        ///  &lt;/Star&gt;
        ///&lt;/SpaceSettings&gt;
        ///.
        /// </summary>
        public static string settings {
            get {
                return ResourceManager.GetString("settings", resourceCulture);
            }
        }
    }
}