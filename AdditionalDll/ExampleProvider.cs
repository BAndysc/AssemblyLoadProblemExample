using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdditionalDll
{
    [AutoRegister]
    public class ExampleProvider : IProvider
    {
        public ContentControl GetView()
        {
            return new MainWindow();
        }
    }
}
