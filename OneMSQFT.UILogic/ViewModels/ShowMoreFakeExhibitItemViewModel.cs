using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ShowMoreFakeExhibitItemViewModel : ExhibitItemViewModel
    {
        public bool IsShowMoreItem = true;

        public ShowMoreFakeExhibitItemViewModel(IExhibit exhibitModel, bool isShowMoreItem) : base(exhibitModel)
        {
            IsShowMoreItem = isShowMoreItem;
        }
    }
}
