﻿#pragma checksum "..\..\..\..\Forms\TrangChu\TrangChu.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B7856E617484E4F4E28ACE97D1962797A578F72C40020C6DBD933E60AA143759"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using QLTraSua.Forms.TrangChu;
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


namespace QLTraSua.Forms.TrangChu {
    
    
    /// <summary>
    /// TrangChu
    /// </summary>
    public partial class TrangChu : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid chinhchu;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDatMon;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDoanhThu;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNhanVien;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCapNhatMon;
        
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
            System.Uri resourceLocater = new System.Uri("/QLTraSua;component/forms/trangchu/trangchu.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
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
            
            #line 14 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Seting);
            
            #line default
            #line hidden
            return;
            case 2:
            this.chinhchu = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.btnDatMon = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
            this.btnDatMon.Click += new System.Windows.RoutedEventHandler(this.Button_Click_DatMon);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnDoanhThu = ((System.Windows.Controls.Button)(target));
            
            #line 78 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
            this.btnDoanhThu.Click += new System.Windows.RoutedEventHandler(this.Button_Click_DoanhThu);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnNhanVien = ((System.Windows.Controls.Button)(target));
            
            #line 100 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
            this.btnNhanVien.Click += new System.Windows.RoutedEventHandler(this.Button_Click_NhanVien);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnCapNhatMon = ((System.Windows.Controls.Button)(target));
            
            #line 122 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
            this.btnCapNhatMon.Click += new System.Windows.RoutedEventHandler(this.Button_Click_CapNhatMon);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 144 "..\..\..\..\Forms\TrangChu\TrangChu.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

