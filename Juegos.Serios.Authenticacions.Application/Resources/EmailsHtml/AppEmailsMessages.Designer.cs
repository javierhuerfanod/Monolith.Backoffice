﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Juegos.Serios.Authenticacions.Application.Resources.EmailsHtml {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class AppEmailsMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppEmailsMessages() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Juegos.Serios.Authenticacions.Application.Resources.EmailsHtml.AppEmailsMessages", typeof(AppEmailsMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
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
        ///   Busca una cadena traducida similar a &lt;!DOCTYPE html&gt;
        ///&lt;html lang=&quot;es&quot;&gt;
        ///&lt;head&gt;
        ///    &lt;meta charset=&quot;UTF-8&quot;&gt;
        ///    &lt;meta name=&quot;viewport&quot; content=&quot;width=device-width, initial-scale=1.0&quot;&gt;
        ///    &lt;title&gt;Correo electrónico de recuperación de contraseña&lt;/title&gt;
        ///&lt;/head&gt;
        ///&lt;body&gt;
        ///    &lt;div style=&quot;font-family: Arial, sans-serif;&quot;&gt;
        ///
        ///        &lt;h2&gt;Correo electrónico de recuperación de contraseña&lt;/h2&gt;
        ///
        ///        &lt;p&gt;Hola {name}  {lastName},&lt;/p&gt;
        ///
        ///        &lt;p&gt;Hemos recibido una solicitud para restablecer tu contraseña. Tu nueva contraseña temporal es:&lt;/p&gt;
        ///     [resto de la cadena truncado]&quot;;.
        /// </summary>
        internal static string Emails_RecoveryPassword_Body {
            get {
                return ResourceManager.GetString("Emails_RecoveryPassword_Body", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Recuperacion de contraseña.
        /// </summary>
        internal static string Emails_RecoveryPassword_Subject {
            get {
                return ResourceManager.GetString("Emails_RecoveryPassword_Subject", resourceCulture);
            }
        }
    }
}
