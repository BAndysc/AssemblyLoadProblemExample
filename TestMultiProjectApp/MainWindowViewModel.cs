using Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestMultiProjectApp
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly Lazy<IProvider> provider;

        private ContentControl _testView;

        public ContentControl TestView
        {
            get => _testView;
            set => SetProperty(ref _testView, value);
        }

        public ICommand ClickCommand
        {
            get;
            private set;
        }

        public MainWindowViewModel(Lazy<IProvider> provider)
        {
            ClickCommand = new DelegateCommand(ClickedMethod);
            this.provider = provider;
        }
        
        private void ClickedMethod()
        {
            TestView = provider.Value.GetView();
        }
    }
}
