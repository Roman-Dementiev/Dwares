﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ACE.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageSmall : TabbedPage
    {
        public TabbedPageSmall ()
        {
            InitializeComponent();
        }
    }
}