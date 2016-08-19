using System.Collections.Generic;
using PythogorasSquare.WebApp.ViewModels.Qualities;

namespace PythogorasSquare.WebApp.ViewModels.Home
{
    public class PsychoMatrixViewModel
    {
        public IReadOnlyList<QualityViewModel> Qualities { get; }


        public PsychoMatrixViewModel(IReadOnlyList<QualityViewModel> qualities)
        {
            Qualities = qualities;
        }
    }
}