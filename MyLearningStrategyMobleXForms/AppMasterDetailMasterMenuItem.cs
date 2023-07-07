using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLearningStrategyMobleXForms
{
    public enum DetailPageOptions
    {
            CreateInstance,
            TabbedPage
    }
    public class AppMasterDetailMasterMenuItem
    {
        public AppMasterDetailMasterMenuItem()
        {
            TargetType = typeof(AppMasterDetailMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public Type TargetType { get; set; }
        public DetailPageOptions PageOptions { get; set; }
    }
}