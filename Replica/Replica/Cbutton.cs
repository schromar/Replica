using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Replica
{
    public partial class Cbutton : Component
    {
        public Cbutton()
        {
            InitializeComponent();
        }

        public Cbutton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
