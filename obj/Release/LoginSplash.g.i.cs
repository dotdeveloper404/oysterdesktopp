﻿#pragma checksum "..\..\LoginSplash.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "ED48D8741722067CB5F82856A69F128B4F00BEFE8160F55FDE38048EE32E5800"
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
using SharpVectors.Converters;
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
    /// LoginSplash
    /// </summary>
    public partial class LoginSplash : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 46 "..\..\LoginSplash.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Slid1;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\LoginSplash.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCreateAccount;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\LoginSplash.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Slid2;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\LoginSplash.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Slid3;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\LoginSplash.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel Slid4;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\LoginSplash.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNext;
        
        #line default
        #line hidden
        
        
        #line 176 "..\..\LoginSplash.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button GotoLogin;
        
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
            System.Uri resourceLocater = new System.Uri("/OysterVPN;component/loginsplash.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\LoginSplash.xaml"
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
            this.Slid1 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            
            #line 53 "..\..\LoginSplash.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnCreateAccount = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\LoginSplash.xaml"
            this.btnCreateAccount.Click += new System.Windows.RoutedEventHandler(this.btnCreateAccount_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Slid2 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 5:
            this.Slid3 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 6:
            this.Slid4 = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.btnNext = ((System.Windows.Controls.Button)(target));
            
            #line 128 "..\..\LoginSplash.xaml"
            this.btnNext.Click += new System.Windows.RoutedEventHandler(this.btnNext_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.GotoLogin = ((System.Windows.Controls.Button)(target));
            
            #line 176 "..\..\LoginSplash.xaml"
            this.GotoLogin.Click += new System.Windows.RoutedEventHandler(this.GotoLogin_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

