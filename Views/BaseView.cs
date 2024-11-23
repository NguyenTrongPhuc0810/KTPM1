using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceConfig.Views
{
    internal class BaseView<TControl, TModel> : System.Mvc.IView
        where TControl : System.Windows.FrameworkElement, new()
    {
        public TControl MainContent { get; set; }
        public TModel Model { get; set; }
        public object Content => MainContent;

        protected virtual void RenderCore() { }
        public void Render(object model)
        {
            Model = (TModel)model;
            MainContent = new TControl();

            MainContent.DataContext = Model;

            RenderCore();
        }
    }
}
