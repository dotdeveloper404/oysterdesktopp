#pragma checksum "..\..\SplitTunnel.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "47C386DE0046505D74FECBFC758F3DA0916F43118FF7AFFCEA8E9D8C58B3125F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using OysterVPN;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace OysterVPN {
    
    
    /// <summary>
    /// SplitTunnel
    /// </summary>
    public partial class SplitTunnel : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\SplitTunnel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SitesList;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\SplitTunnel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\SplitTunnel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton AllAppsUseVpn;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\SplitTunnel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton DontUseVpn;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\SplitTunnel.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton UseVpn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/OysterVPN;component/splittunnel.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SplitTunnel.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.SitesList = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\SplitTunnel.xaml"
            this.btnSave.Click += new System.Windows.RoutedEventHandler(this.btnSave_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.AllAppsUseVpn = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 4:
            this.DontUseVpn = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 5:
            this.UseVpn = ((System.Windows.Controls.RadioButton)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

